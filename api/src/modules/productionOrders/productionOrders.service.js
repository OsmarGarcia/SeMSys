'use strict';

const oracledb = require('oracledb');
const { withTransaction, withConnection } = require('../../db/transaction');
const repo = require('./productionOrders.repository');
const sequences = require('../../shared/sequences.service');
const { explodeFormula } = require('../../shared/formula.service');
const stock = require('../../shared/stock.service');
const { round } = require('../../utils/rounding');
const { toBrDateTime, toBrDate } = require('../../utils/dates');
const { NotFoundError, ConflictError, BusinessError } = require('../../utils/errors');
const env = require('../../config/env');

const POSICAO_LABELS = {
  L: 'AGUARDANDO_INICIO',
  P: 'EM_PRODUCAO',
  F: 'FECHADA',
  C: 'CANCELADA'
};

function describePosicao(posicao) {
  return POSICAO_LABELS[posicao] || posicao;
}

function mapHeader(row, numOp) {
  return {
    numOp: String(numOp),
    codProd: row.CODPROD,
    descricao: row.DESCRICAO,
    quantidade: Number(row.QT),
    metodo: row.METODO,
    posicao: row.POSICAO,
    posicaoDescricao: describePosicao(row.POSICAO),
    kit: row.KIT
  };
}

async function assertOrderExists(connection, numOp) {
  const current = await repo.getForValidation(connection, numOp);
  if (!current) {
    throw new NotFoundError(`Ordem de produção ${numOp} não encontrada.`, 'PRODUCTION_ORDER_NOT_FOUND');
  }
  return current;
}

/**
 * Lista OPs por posição. Para POSICAO='L' (aguardando início), decora cada OP
 * com `stockShortage` — mesma regra usada para colorir de vermelho a grade em
 * `PesquisarOPsIniciar` (frmManutencaoOP.vb): necessidade > estoque e o
 * insumo não é semiacabado.
 */
async function listOrders({ position, codFilial, startDateFrom, startDateTo }) {
  return withConnection(async (connection) => {
    const orders = await repo.search(connection, { position, codFilial, startDateFrom, startDateTo });
    if (position !== 'L') return orders;

    const decorated = [];
    for (const order of orders) {
      const insumos = await repo.getInsumosComEstoque(connection, order.NUMOP);
      const stockShortage = insumos.some((item) => {
        const necessario = round(Number(item.QTNECESSIDADE), 2);
        const disponivel = round(Number(item.ESTOQUE), 2);
        return necessario > disponivel && item.TIPOMERC !== 'SA';
      });
      decorated.push({ ...order, stockShortage });
    }
    return decorated;
  });
}

async function getOrder(numOp) {
  return withConnection(async (connection) => {
    const header = await repo.getHeader(connection, numOp);
    if (!header) {
      throw new NotFoundError(`Ordem de produção ${numOp} não encontrada.`, 'PRODUCTION_ORDER_NOT_FOUND');
    }
    return mapHeader(header, numOp);
  });
}

async function getItems(numOp, { codFilialEstoque = env.defaults.codFilialEstoque } = {}) {
  return withConnection(async (connection) => {
    await assertOrderExists(connection, numOp);
    return repo.getItems(connection, { numOp, codFilialEstoque });
  });
}

async function getMovements(numOp) {
  return withConnection(async (connection) => {
    await assertOrderExists(connection, numOp);
    return repo.getMovements(connection, numOp);
  });
}

async function getLabel(numOp, { offset = 0 } = {}) {
  return withConnection(async (connection) => {
    const label = await repo.getLabelData(connection, { numOp, offset });
    if (!label) {
      throw new NotFoundError(`Ordem de produção ${numOp} não encontrada.`, 'PRODUCTION_ORDER_NOT_FOUND');
    }
    return label;
  });
}

/**
 * Núcleo de `GerarProgramacao` (frmProgramarProducao.vb): numeração, lote,
 * PEPROGOP+PCOPC+PCOBSOP e explosão da fórmula
 * (PEPROGITENS+PCOPI+PCCOMPOSICAOFRACAO). Recebe a `connection` já aberta
 * para poder ser reaproveitado dentro da transação de outro caso de uso
 * (ex.: `productionSchedules.service.js` ao gerar OPs a partir da grade) sem
 * abrir uma segunda transação em uma conexão separada.
 */
async function createOrderWithConnection(connection, { codProd, metodo, qtProduzir, dtPrevInicio, codFilial = env.defaults.codFilialProducao }, user) {
  const qtd = round(qtProduzir, 3);
  const dtPrevInicioBr = dtPrevInicio ? toBrDateTime(dtPrevInicio) : toBrDateTime(new Date());

  const numOp = await sequences.nextNumOp(connection);
  const { numLote } = await sequences.nextNumLote(connection, {
    codProd,
    codFilial,
    dtPrevInicio: dtPrevInicioBr
  });

  await repo.insertPeprogop(connection, {
    numOp, codFilial, codProd, metodo, qtProduzir: qtd, codFunc: user.matricula, numLote, dtPrevInicio: dtPrevInicioBr
  });
  await repo.insertPcopc(connection, {
    numOp, codFilial, codProd, metodo, qtProduzir: qtd, codFunc: user.matricula, numLote, dtPrevInicio: dtPrevInicioBr
  });
  await repo.insertPcobsop(connection, { numOp, codFunc: user.matricula });

  // Variante de arredondamento equivalente a `frmProgramarProducao.BuscarFormula`.
  const insumos = await explodeFormula(connection, {
    codProdMaster: codProd, metodo, codFilial, qty: qtd, unitDecimals: 3, decimals: 3
  });

  let numSeq = 1;
  for (const insumo of insumos) {
    await repo.insertFormulaItem(connection, {
      numOp,
      numSeq,
      codProd: insumo.codProd,
      codProdMaster: codProd,
      qtNecessidade: insumo.qtNecessidade,
      codFunc: user.matricula
    });
    numSeq += 1;
  }

  return { numOp, numLote, codProd, metodo, qtProduzir: qtd, dtPrevInicio: dtPrevInicioBr, itens: insumos };
}

/**
 * Cria uma nova Ordem de Produção — réplica de `GerarProgramacao`
 * (frmProgramarProducao.vb): numeração, lote, PEPROGOP+PCOPC+PCOBSOP e
 * explosão da fórmula (PEPROGITENS+PCOPI+PCCOMPOSICAOFRACAO), tudo em uma
 * única transação.
 */
async function createOrder(input, user) {
  return withTransaction((connection) => createOrderWithConnection(connection, input, user));
}

/**
 * Chama a function PL/SQL `Reprogramar_OP_Func`, exigindo retorno 'SUCESSO'.
 *
 * Atenção: o VB.NET original passa a data como `DtPickerHoraInicial.Value.ToString()`
 * (formatação dependente da cultura do Windows do posto que executa o app).
 * Aqui padronizamos para 'DD/MM/YYYY HH24:MI:SS', consistente com o resto da
 * API — confirme com quem mantém `Reprogramar_OP_Func` que esse é de fato o
 * formato de string esperado antes de usar em produção.
 */
async function callReprogramarOPFunc(connection, { numOp, novaQtProduzir, numLote, dtPrevInicio }) {
  const result = await connection.execute(
    `DECLARE
       v_status VARCHAR2(32767);
     BEGIN
       v_status := Reprogramar_OP_Func(:numOp, :novaQt, :numLote, :dtPrevInicio);
       :vStatus := v_status;
     END;`,
    {
      numOp,
      novaQt: novaQtProduzir,
      numLote,
      dtPrevInicio,
      vStatus: { dir: oracledb.BIND_OUT, type: oracledb.STRING, maxSize: 32767 }
    }
  );

  const status = result.outBinds.vStatus;
  if (status !== 'SUCESSO') {
    throw new BusinessError(`Erro ao reprogramar a OP ${numOp}: ${status}`, 'REPROGRAM_FAILED', { numOp, status });
  }
  return status;
}

/**
 * Núcleo de `ReprogramarOPWinthor`. Recebe `connection` para poder ser
 * chamado a partir da transação de outro caso de uso (ex.: ao incluir na
 * grade de programação uma OP existente cuja quantidade/data mudou).
 */
async function reprogramOrderWithConnection(connection, numOp, { novaQtProduzir, numLote, dtPrevInicio }) {
  const current = await assertOrderExists(connection, numOp);
  const tipoLote = current.TIPOLOTE;
  const qtAtual = Number(current.QTPRODUZIR);
  const isLoteEspecial = tipoLote === 'TAMPICO' || tipoLote === 'JULIANO';

  if (qtAtual === Number(novaQtProduzir) && !isLoteEspecial) {
    return { numOp, changed: false, message: 'Quantidade já é a mesma; nada a reprogramar.' };
  }

  if (qtAtual === 0 || current.POSICAO === 'C' || current.POSICAO === 'F') {
    throw new ConflictError(
      `Não é possível reprogramar a OP ${numOp}: OP inexistente, com quantidade zerada, fechada ou cancelada.`,
      'ORDER_NOT_PROGRAMMABLE',
      { posicao: current.POSICAO, qtProduzir: qtAtual }
    );
  }

  let numLoteFinal = numLote || current.NUMLOTE;
  const dtPrevInicioBr = toBrDateTime(dtPrevInicio || current.DTPREVINICIO);

  if (isLoteEspecial) {
    const loteResult = await connection.execute(
      `SELECT FNC_PROXNUMLOTE(:codProd, TO_DATE(:data,'DD/MM/YYYY')) AS PROXNUMLOTE FROM DUAL`,
      { codProd: current.CODPRODMASTER, data: toBrDate(dtPrevInicioBr) }
    );
    numLoteFinal = String(loteResult.rows[0].PROXNUMLOTE);
  }

  await callReprogramarOPFunc(connection, {
    numOp,
    novaQtProduzir,
    numLote: numLoteFinal,
    dtPrevInicio: dtPrevInicioBr
  });

  return { numOp, changed: true, numLote: numLoteFinal, qtProduzir: novaQtProduzir, dtPrevInicio: dtPrevInicioBr };
}

/**
 * Reprograma quantidade/lote/data de início de uma OP existente — réplica de
 * `ReprogramarOPWinthor`.
 */
async function reprogramOrder(numOp, body) {
  return withTransaction((connection) => reprogramOrderWithConnection(connection, numOp, body));
}

/**
 * Recalcula os itens/insumos de uma OP a partir da fórmula atual (ex.: após
 * trocar o método) — réplica de `RecalcularItensOP` (frmManutencaoOP.vb).
 */
async function recalculateItems(numOp, { metodo } = {}) {
  return withTransaction(async (connection) => {
    const current = await assertOrderExists(connection, numOp);

    if (current.POSICAO === 'C' || current.POSICAO === 'F') {
      throw new ConflictError(
        `Não é possível recalcular itens da OP ${numOp}: OP fechada ou cancelada.`,
        'ORDER_NOT_EDITABLE',
        { posicao: current.POSICAO }
      );
    }

    const metodoFinal = metodo || current.METODO;

    await repo.deleteFormulaItems(connection, numOp);

    // Variante de arredondamento equivalente a `frmManutencaoOP.BuscarFormula`.
    const insumos = await explodeFormula(connection, {
      codProdMaster: current.CODPRODMASTER,
      metodo: metodoFinal,
      codFilial: env.defaults.codFilialProducao,
      qty: Number(current.QTPRODUZIR),
      unitDecimals: 6,
      decimals: 3
    });

    let numSeq = 1;
    for (const insumo of insumos) {
      await repo.insertFormulaItem(connection, {
        numOp,
        numSeq,
        codProd: insumo.codProd,
        codProdMaster: current.CODPRODMASTER,
        qtNecessidade: insumo.qtNecessidade,
        codFunc: undefined
      });
      numSeq += 1;
    }

    await repo.updateMetodo(connection, { numOp, metodo: metodoFinal });

    return { numOp, metodo: metodoFinal, itens: insumos };
  });
}

/** Núcleo de cancelamento — ver nota em `createOrderWithConnection`. */
async function cancelOrderWithConnection(connection, numOp) {
  const current = await assertOrderExists(connection, numOp);

  if (current.POSICAO === 'C') {
    return { numOp, posicao: 'C', alreadyCancelled: true };
  }
  if (current.POSICAO === 'F') {
    throw new ConflictError(`OP ${numOp} está fechada e não pode ser cancelada.`, 'ORDER_CLOSED');
  }

  await repo.updatePosicaoCancelada(connection, numOp);
  return { numOp, posicao: 'C', alreadyCancelled: false };
}

/** Cancela uma OP no Winthor (POSICAO='C') — idempotente se já cancelada. */
async function cancelOrder(numOp) {
  return withTransaction((connection) => cancelOrderWithConnection(connection, numOp));
}

/**
 * Aloca quantidade necessária entre lotes disponíveis em ordem FEFO
 * (primeiro a vencer, primeiro a sair) — réplica exata do laço de alocação em
 * `IniciarOP` (frmManutencaoOP.vb): preenche um lote por completo antes de
 * passar para o próximo, na ordem em que vieram (já ordenados por
 * DTVALIDADE ASC pela consulta).
 */
function allocateLotsFEFO(lotes, qtNecessidade) {
  let restante = qtNecessidade;
  const alocacoes = [];

  for (const lote of lotes) {
    if (restante <= 0) break;
    const disponivel = Number(lote.QTDISPONIVEL);

    if (disponivel >= restante) {
      alocacoes.push({ ...lote, alocado: restante });
      restante = 0;
      break;
    }

    alocacoes.push({ ...lote, alocado: disponivel });
    restante = round(restante - disponivel, 3);
  }

  return { alocacoes, restanteNaoAlocado: restante };
}

/**
 * Inicia a produção de uma OP — réplica de `IniciarOP` (frmManutencaoOP.vb):
 * valida estoque de todos os insumos, reserva lotes (FEFO) para os
 * controlados por lote, chama PKG_ESTOQUE.RESERVA_INCLUIR e move a OP para
 * POSICAO='P'.
 */
async function startProductionOrderWithConnection(connection, numOp, user, { codFilialProducao = env.defaults.codFilialProducao } = {}) {
  const current = await assertOrderExists(connection, numOp);

  if (current.POSICAO !== 'L') {
    throw new ConflictError(
      `A OP ${numOp} não está aguardando início (posição atual: ${current.POSICAO}).`,
      'ORDER_NOT_WAITING',
      { posicao: current.POSICAO }
    );
  }

  const insumos = await repo.getInsumosComEstoque(connection, numOp);

  const faltantes = insumos
    .map((item) => ({
      codProd: item.CODPROD,
      necessario: round(Number(item.QTNECESSIDADE), 6),
      disponivel: round(Number(item.ESTOQUE), 6),
      tipoMerc: item.TIPOMERC
    }))
    .filter((item) => item.necessario > item.disponivel && item.tipoMerc !== 'SA');

  if (faltantes.length > 0) {
    throw new BusinessError(
      'Insumos sem estoque disponível para iniciar o processo.',
      'INSUFFICIENT_STOCK',
      faltantes
    );
  }

  // Réplica de `UPDATE PCOPI SET RESERVALIBERADA = 'N' WHERE NUMOP = numop`,
  // que no VB.NET original roda (de forma redundante) a cada iteração do
  // laço — movido para antes do laço por ser idempotente por OP, não por item.
  await repo.updateReservaLiberada(connection, numOp);

  for (const item of insumos) {
    const codProd = item.CODPROD;
    const qtNecessidade = round(Number(item.QTNECESSIDADE), 3);
    const qtEstoque = round(Number(item.ESTOQUE), 3);

    // Réplica exata: só reserva o insumo se, no momento da gravação, ainda
    // há saldo suficiente (produtos semiacabados em falta são pulados aqui,
    // mesmo já tendo passado pela validação acima).
    if (qtNecessidade > qtEstoque) continue;

    await repo.updateReservaPendente(connection, { numOp, codProd, qtNecessidade });

    if (item.ESTOQUEPORLOTE === 'S') {
      const lotes = await stock.getLotesDisponiveis(connection, { codProd, codFilial: codFilialProducao });
      const { alocacoes } = allocateLotsFEFO(lotes, qtNecessidade);

      for (const alocacao of alocacoes) {
        if (alocacao.alocado <= 0) continue;
        await repo.reservarLotePorItem(connection, {
          codProd,
          numLote: alocacao.NUMLOTE,
          quantidadeAlocada: alocacao.alocado,
          numOp,
          dtValidade: alocacao.DTVALIDADE
        });
      }
    }

    await stock.reservaIncluir(connection, { numOp, codProd, numSeq: '1' });
  }

  await repo.updateInicioProducao(connection, { numOp, matriculaUsuario: user.matricula });

  return { numOp, posicao: 'P' };
}

/**
 * Inicia a produção de uma OP — réplica de `IniciarOP` (frmManutencaoOP.vb):
 * valida estoque de todos os insumos, reserva lotes (FEFO) para os
 * controlados por lote, chama PKG_ESTOQUE.RESERVA_INCLUIR e move a OP para
 * POSICAO='P'.
 */
async function startProductionOrder(numOp, user, options) {
  return withTransaction((connection) => startProductionOrderWithConnection(connection, numOp, user, options));
}

module.exports = {
  describePosicao,
  listOrders,
  getOrder,
  getItems,
  getMovements,
  getLabel,
  createOrder,
  createOrderWithConnection,
  reprogramOrder,
  reprogramOrderWithConnection,
  recalculateItems,
  cancelOrder,
  cancelOrderWithConnection,
  startProductionOrder,
  startProductionOrderWithConnection,
  allocateLotsFEFO
};

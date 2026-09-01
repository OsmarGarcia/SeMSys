'use strict';

const { withTransaction, withConnection } = require('../../db/transaction');
const repo = require('./productionSchedules.repository');
const productionOrdersRepo = require('../productionOrders/productionOrders.repository');
const productionOrdersService = require('../productionOrders/productionOrders.service');
const sequences = require('../../shared/sequences.service');
const { explodeFormula } = require('../../shared/formula.service');
const stock = require('../../shared/stock.service');
const { round } = require('../../utils/rounding');
const { toBrDateTime } = require('../../utils/dates');
const { NotFoundError, ConflictError, ValidationError, BusinessError } = require('../../utils/errors');
const env = require('../../config/env');

// --- mapeamento DB <-> API -------------------------------------------------

function mapItemFromDb(row) {
  return {
    idPrograma: row.IDPROGRAMA != null ? String(row.IDPROGRAMA) : undefined,
    codProd: row.CODPROD,
    descricao: row.DESCRICAO,
    embalagem: row.EMBALAGEM,
    metodo: row.METODO,
    qtProduzir: Number(row.QTPRODUZIR),
    horaInicial: row.HORAINICIAL,
    horaFinal: row.HORAFINAL,
    tempoTotal: Number(row.TEMPOTOTAL),
    numOp: row.NUMOP || null,
    numLote: row.NUMLOTE || null,
    qtUnitCx: row.QTUNITCX != null ? Number(row.QTUNITCX) : 0,
    linha: row.LINHA
  };
}

function mapItemToDbBinds(item, programa) {
  return {
    codProd: item.codProd,
    descricao: item.descricao,
    qtProduzir: item.qtProduzir,
    numOp: item.numOp || null,
    numLote: item.numLote || null,
    horaInicial: toBrDateTime(item.horaInicial),
    horaFinal: toBrDateTime(item.horaFinal),
    tempoTotal: item.tempoTotal,
    programa,
    metodo: item.metodo,
    qtUnitCx: item.qtUnitCx || 0,
    linha: item.linha,
    embalagem: item.embalagem || ''
  };
}

function mapMaterialFromDb(row) {
  return {
    idPrograma: row.IDPROGRAMA != null ? String(row.IDPROGRAMA) : undefined,
    codProd: row.CODPROD,
    descricao: row.DESCRICAO,
    metodo: row.METODO || null,
    qtProduzir: Number(row.QTPRODUZIR),
    numOp: row.NUMOP || null,
    numLote: row.NUMLOTE || null,
    dtPrevInicioSA: row.DTPREVINICIOSA
  };
}

function mapMaterialToDbBinds(material, programa) {
  return {
    codProd: material.codProd,
    descricao: material.descricao,
    qtProduzir: material.qtProduzir,
    numOp: material.numOp || null,
    numLote: material.numLote || null,
    programa,
    metodo: material.metodo || null,
    dtPrevInicioSA: toBrDateTime(material.dtPrevInicioSA)
  };
}

async function replaceItemsWithConnection(connection, codPrograma, itens) {
  await repo.deleteItens(connection, codPrograma);
  for (const item of itens) {
    await repo.insertItem(connection, mapItemToDbBinds(item, codPrograma));
  }
}

async function replaceMateriaisWithConnection(connection, codPrograma, materiais) {
  await repo.deleteMateriais(connection, codPrograma);
  for (const material of materiais) {
    await repo.insertMaterial(connection, mapMaterialToDbBinds(material, codPrograma));
  }
}

/**
 * Reflui os horários de uma linha de produção — réplica de `ReprogramarOPs`
 * (ModuloFuncoes.vb): reordena por horário de início e recalcula, em
 * sequência, hora final = hora inicial + tempo necessário; o início do
 * próximo item é sempre o final do anterior.
 */
function reflowLine(itensDaLinha) {
  const ordenados = [...itensDaLinha].sort((a, b) => new Date(a.horaInicial) - new Date(b.horaInicial));
  let cursor = null;

  return ordenados.map((item) => {
    const inicio = cursor ? new Date(cursor) : new Date(item.horaInicial);
    const fim = new Date(inicio.getTime() + Number(item.tempoTotal) * 3600 * 1000);
    cursor = fim;
    return { ...item, horaInicial: inicio.toISOString(), horaFinal: fim.toISOString() };
  });
}

/** Recalcula/substitui apenas os itens de uma linha dentro da lista completa do programa. */
function withLineReflowed(todosOsItens, linha, itensDaLinhaAtualizados) {
  const outrasLinhas = todosOsItens.filter((item) => item.linha !== linha);
  return [...outrasLinhas, ...reflowLine(itensDaLinhaAtualizados)];
}

// --- casos de uso -----------------------------------------------------------

/** Cria um novo programa de produção (código sequencial) — réplica do botão "+". */
async function createSchedule() {
  return withConnection(async (connection) => {
    const codPrograma = await sequences.nextCodPrograma(connection);
    return { codPrograma };
  });
}

/**
 * Carrega um programa (itens + materiais) — réplica de `btnCarregarPrograma_Click`,
 * incluindo a atualização "ao vivo" do estoque disponível e tipo de mercadoria
 * de cada material (que no VB.NET original não fica persistida, é sempre
 * recalculada na carga).
 */
async function getSchedule(codPrograma, { codFilialEstoque = env.defaults.codFilialEstoque } = {}) {
  return withConnection(async (connection) => {
    const existe = await repo.existsPrograma(connection, codPrograma);
    if (!existe) {
      throw new NotFoundError(`Programa de produção ${codPrograma} não encontrado.`, 'SCHEDULE_NOT_FOUND');
    }

    const itens = (await repo.getItens(connection, codPrograma)).map(mapItemFromDb);
    const materiaisBrutos = (await repo.getMateriais(connection, codPrograma)).map(mapMaterialFromDb);

    const materiais = [];
    for (const material of materiaisBrutos) {
      const estoqueDisponivel = await stock.getEstoqueDisponivel(connection, {
        codProd: material.codProd,
        codFilial: codFilialEstoque
      });
      materiais.push({ ...material, estoqueDisponivel });
    }

    return { codPrograma, itens, materiais };
  });
}

/**
 * Inclui um item (produto acabado) na grade de uma linha — réplica de
 * `btnIncluir_Click`: se referenciar uma OP existente, valida/reprograma
 * antes de incluir; calcula `tempoTotal` (horas necessárias) a partir da
 * velocidade nominal do produto e da eficiência informada; e refaz o
 * sequenciamento de horários da linha (`ReprogramarOPs`).
 *
 * Simplificação assumida em relação ao original: a heurística de UI que
 * decidia "encaixar no fim da linha ou usar o horário digitado" foi
 * substituída pelo próprio `reflowLine`, que já reordena por horário de
 * início — o resultado final da grade é o mesmo, sem depender de estado de
 * tela.
 */
async function addItem(codPrograma, input) {
  return withTransaction(async (connection) => {
    const itensAtuais = (await repo.getItens(connection, codPrograma)).map(mapItemFromDb);

    let { numOp, numLote } = input;

    if (numOp) {
      if (itensAtuais.some((item) => item.numOp === String(numOp))) {
        throw new ConflictError(`A OP ${numOp} já está incluída na programação.`, 'ORDER_ALREADY_SCHEDULED');
      }

      const current = await productionOrdersRepo.getForValidation(connection, numOp);
      if (!current) {
        throw new ValidationError(`OP ${numOp} não encontrada no Winthor.`);
      }
      if (Number(current.QTPRODUZIR) === 0 || current.POSICAO === 'C' || current.POSICAO === 'F') {
        throw new ConflictError(
          `Não é possível incluir a OP ${numOp} na programação (inexistente, quantidade zerada, fechada ou cancelada).`,
          'ORDER_NOT_PROGRAMMABLE',
          { posicao: current.POSICAO }
        );
      }

      const precisaReprogramar =
        Number(current.QTPRODUZIR) !== input.qtProduzir ||
        toBrDateTime(current.DTPREVINICIO) !== toBrDateTime(input.horaInicial);

      if (precisaReprogramar) {
        const reprogramado = await productionOrdersService.reprogramOrderWithConnection(connection, numOp, {
          novaQtProduzir: input.qtProduzir,
          dtPrevInicio: input.horaInicial
        });
        if (reprogramado.numLote) numLote = reprogramado.numLote;
      }
    }

    let { tempoTotal, qtUnitCx, embalagem } = input;
    if (!tempoTotal) {
      if (input.codProd === '99999') {
        tempoTotal = 0;
      } else {
        const produto = await repo.getProdutoParaProgramacao(connection, { codProd: input.codProd, linha: input.linha });
        if (!produto) throw new ValidationError(`Produto ${input.codProd} não encontrado.`);
        qtUnitCx = qtUnitCx ?? produto.qtUnitCx;
        embalagem = embalagem ?? produto.embalagem;
        // Réplica literal de `horas = qt / velocidadeNominal / eficiencia` (btnIncluir_Click).
        // `eficienciaRaw` deve ser enviado na mesma representação numérica usada
        // pela tela original (ex.: "9700" para 97,00%) — ver nota em
        // productionSchedules.validators.js.
        const eficiencia = Number(input.eficienciaRaw) / 10000;
        tempoTotal = round(input.qtProduzir / produto.velocidadeNominal / eficiencia, 2);
      }
    }

    const novoItem = {
      codProd: input.codProd,
      descricao: input.descricao,
      embalagem: embalagem || '',
      metodo: input.metodo || '-',
      qtProduzir: input.qtProduzir,
      horaInicial: input.horaInicial,
      horaFinal: input.horaInicial,
      tempoTotal,
      numOp: numOp || null,
      numLote: numLote || null,
      qtUnitCx: qtUnitCx || 0,
      linha: input.linha
    };

    const itensDaLinha = [...itensAtuais.filter((item) => item.linha === input.linha), novoItem];
    const itensFinais = withLineReflowed(itensAtuais, input.linha, itensDaLinha);

    await replaceItemsWithConnection(connection, codPrograma, itensFinais);

    return { codPrograma, itens: itensFinais };
  });
}

/**
 * Remove um item da grade — réplica de `Button2_Click` (exclusão): sempre
 * remove da programação; se o item tinha uma OP associada e `cancelInWinthor`
 * for true, cancela também a OP no Winthor (POSICAO='C').
 */
async function removeItem(codPrograma, idPrograma, { cancelInWinthor = false } = {}) {
  return withTransaction(async (connection) => {
    const itensAtuais = (await repo.getItens(connection, codPrograma)).map(mapItemFromDb);
    const alvo = itensAtuais.find((item) => item.idPrograma === String(idPrograma));

    if (!alvo) {
      throw new NotFoundError(`Item ${idPrograma} não encontrado no programa ${codPrograma}.`, 'SCHEDULE_ITEM_NOT_FOUND');
    }

    let ordemCancelada = null;
    if (alvo.numOp && cancelInWinthor) {
      ordemCancelada = await productionOrdersService.cancelOrderWithConnection(connection, alvo.numOp);
    }

    const restantes = itensAtuais.filter((item) => item.idPrograma !== String(idPrograma));
    const itensDaLinha = restantes.filter((item) => item.linha === alvo.linha);
    const itensFinais = withLineReflowed(restantes, alvo.linha, itensDaLinha);

    await replaceItemsWithConnection(connection, codPrograma, itensFinais);

    return { codPrograma, removed: alvo, ordemCancelada, itens: itensFinais };
  });
}

/**
 * Recalcula o MRP (explosão de fórmula multinível) para todos os produtos
 * acabados do programa — réplica de `GerarMRP`/`FormularAcabados`.
 *
 * Sempre recomputa do zero (soma as necessidades por insumo, incluindo
 * explosão recursiva de semiacabados), mas preserva `numOp`/`numLote` de
 * materiais que já tinham uma OP gerada em uma explosão anterior — a
 * reconciliação de quantidade dessa OP existente acontece em
 * `generateOrders`, não aqui.
 *
 * Quando um semiacabado aparece sem método definido (nem em
 * `methodOverrides` nem já resolvido em uma explosão anterior), a chamada
 * falha com 422/`METHOD_REQUIRED_FOR_SEMIACABADO` listando quais produtos
 * precisam de um método — réplica do modal `DefinirMetodoMRP`, que aqui vira
 * uma volta ao cliente em vez de um diálogo modal.
 */
async function explodeMaterials(codPrograma, { methodOverrides = {}, codFilialEstoque = env.defaults.codFilialEstoque, codFilialProducao = env.defaults.codFilialProducao } = {}) {
  return withTransaction(async (connection) => {
    const itens = (await repo.getItens(connection, codPrograma)).map(mapItemFromDb);
    const materiaisExistentes = (await repo.getMateriais(connection, codPrograma)).map(mapMaterialFromDb);
    const existentesPorCodProd = new Map(materiaisExistentes.map((m) => [m.codProd, m]));

    const acumulador = new Map();
    const expandidos = new Set();

    let nivelAtual = itens
      .filter((item) => item.codProd !== '99999')
      .map((item) => ({ codProdMaster: item.codProd, metodo: item.metodo, qty: item.qtProduzir, dtInicio: item.horaInicial }));

    while (nivelAtual.length > 0) {
      for (const entrada of nivelAtual) {
        // Variante de arredondamento e filiais equivalente a `FormularAcabados`
        // (PCCOMPOSICAO pela filial de produção, PCEST pela filial de estoque).
        const linhasFormula = await explodeFormula(connection, {
          codProdMaster: entrada.codProdMaster,
          metodo: entrada.metodo,
          codFilialProducao,
          codFilialEstoque,
          qty: entrada.qty,
          unitDecimals: null,
          decimals: 6
        });

        for (const linha of linhasFormula) {
          const existenteAcumulado = acumulador.get(linha.codProd);
          if (existenteAcumulado) {
            existenteAcumulado.qtNecessidade = round(existenteAcumulado.qtNecessidade + linha.qtNecessidade, 6);
          } else {
            const existentePersistido = existentesPorCodProd.get(linha.codProd);
            acumulador.set(linha.codProd, {
              codProd: linha.codProd,
              descricao: linha.descricao,
              qtNecessidade: linha.qtNecessidade,
              tipoMerc: linha.tipoMerc,
              dtInicio: entrada.dtInicio,
              metodo: null,
              numOp: existentePersistido?.numOp || null,
              numLote: existentePersistido?.numLote || null
            });
          }
        }
      }

      const proximoNivel = [];
      for (const [codProd, material] of acumulador) {
        if (material.tipoMerc !== 'SA' || expandidos.has(codProd)) continue;

        const metodoResolvido = methodOverrides[codProd] || existentesPorCodProd.get(codProd)?.metodo;
        if (!metodoResolvido) continue; // fica pendente; checado após o laço

        material.metodo = metodoResolvido;
        expandidos.add(codProd);
        proximoNivel.push({
          codProdMaster: codProd,
          metodo: metodoResolvido,
          qty: material.qtNecessidade,
          dtInicio: material.dtInicio
        });
      }

      nivelAtual = proximoNivel;
    }

    const pendentesMetodo = [...acumulador.values()]
      .filter((material) => material.tipoMerc === 'SA' && !expandidos.has(material.codProd))
      .map((material) => ({ codProd: material.codProd, descricao: material.descricao }));

    if (pendentesMetodo.length > 0) {
      throw new BusinessError(
        'Informe o método de fórmula para os semiacabados listados e refaça a explosão do MRP.',
        'METHOD_REQUIRED_FOR_SEMIACABADO',
        pendentesMetodo
      );
    }

    const materiaisFinais = [...acumulador.values()].map((material) => ({
      codProd: material.codProd,
      descricao: material.descricao,
      metodo: material.metodo,
      qtProduzir: round(material.qtNecessidade, 6),
      numOp: material.numOp,
      numLote: material.numLote,
      dtPrevInicioSA: material.dtInicio
    }));

    await replaceMateriaisWithConnection(connection, codPrograma, materiaisFinais);

    return { codPrograma, materiais: materiaisFinais };
  });
}

/**
 * Gera as Ordens de Produção no Winthor para os itens/materiais do programa
 * que ainda não têm `numOp` — réplica de `btnProgramar2_Click` (produtos
 * acabados) seguido de `btnProgramarSA_Click` (semiacabados). Materiais sem
 * `metodo` definido são matérias-primas compradas e nunca geram OP (mesmo
 * critério do original: `SubItems(5).Text = ""` pula o item).
 */
async function generateOrders(codPrograma, user) {
  return withTransaction(async (connection) => {
    const itens = (await repo.getItens(connection, codPrograma)).map(mapItemFromDb);
    const materiais = (await repo.getMateriais(connection, codPrograma)).map(mapMaterialFromDb);

    const itensGerados = [];
    const materiaisGerados = [];
    const reprogramados = [];

    for (const item of itens) {
      if (item.numOp || item.codProd === '99999') continue;

      const gerado = await productionOrdersService.createOrderWithConnection(
        connection,
        { codProd: item.codProd, metodo: item.metodo, qtProduzir: item.qtProduzir, dtPrevInicio: item.horaInicial },
        user
      );
      item.numOp = gerado.numOp;
      item.numLote = gerado.numLote;
      itensGerados.push({ codProd: item.codProd, numOp: gerado.numOp, numLote: gerado.numLote });
    }

    for (const material of materiais) {
      if (!material.metodo) continue; // matéria-prima comprada, não gera OP

      if (!material.numOp) {
        const gerado = await productionOrdersService.createOrderWithConnection(
          connection,
          { codProd: material.codProd, metodo: material.metodo, qtProduzir: material.qtProduzir, dtPrevInicio: material.dtPrevInicioSA },
          user
        );
        material.numOp = gerado.numOp;
        material.numLote = gerado.numLote;
        materiaisGerados.push({ codProd: material.codProd, numOp: gerado.numOp, numLote: gerado.numLote });
        continue;
      }

      const opAtual = await productionOrdersRepo.getForValidation(connection, material.numOp);
      if (opAtual && Number(opAtual.QTPRODUZIR) !== material.qtProduzir) {
        await productionOrdersService.reprogramOrderWithConnection(connection, material.numOp, {
          novaQtProduzir: material.qtProduzir,
          dtPrevInicio: material.dtPrevInicioSA
        });
        reprogramados.push({ codProd: material.codProd, numOp: material.numOp, novaQtProduzir: material.qtProduzir });
      }
    }

    await replaceItemsWithConnection(connection, codPrograma, itens);
    await replaceMateriaisWithConnection(connection, codPrograma, materiais);

    return { codPrograma, itensGerados, materiaisGerados, reprogramados };
  });
}

/** Dados formatados para impressão — réplica de `ImprimirPrograma` + `ImprimirSemiAcabado`. */
async function getPrintData(codPrograma) {
  return withConnection(async (connection) => {
    const itens = (await repo.getItens(connection, codPrograma)).map(mapItemFromDb);
    const materiais = (await repo.getMateriais(connection, codPrograma)).map(mapMaterialFromDb);

    const linhasImpressao = [];
    for (const item of itens) {
      let qtMaster = 0;
      let qtPalete = 0;

      if (item.codProd !== '99999') {
        const produto = await repo.getProdutoParaProgramacao(connection, { codProd: item.codProd, linha: item.linha });
        qtMaster = produto ? round(item.qtProduzir / (item.qtUnitCx || 1), 0) : 0;
        qtPalete = produto && produto.qtTotPal ? round(qtMaster / produto.qtTotPal, 0) : 0;
      }

      linhasImpressao.push({
        codigo: item.codProd,
        descricao: item.descricao,
        quantidade: item.qtProduzir,
        metodo: item.metodo,
        horaInicial: item.horaInicial,
        horaFinal: item.horaFinal,
        horaNecessaria: item.tempoTotal,
        numOp: item.numOp,
        numLote: item.numLote,
        programa: codPrograma,
        qtMaster,
        linha: item.linha,
        qtPalete
      });
    }

    // Réplica de `ImprimirSemiAcabado`: só entram materiais com método definido.
    const semiAcabados = materiais
      .filter((material) => material.metodo)
      .map((material) => ({
        codigo: material.codProd,
        descricao: material.descricao,
        quantidade: round(material.qtProduzir, 4),
        numOp: material.numOp,
        numLote: material.numLote
      }));

    return { codPrograma, itens: linhasImpressao, semiAcabados };
  });
}

module.exports = {
  createSchedule,
  getSchedule,
  addItem,
  removeItem,
  explodeMaterials,
  generateOrders,
  getPrintData
};

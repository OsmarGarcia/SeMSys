'use strict';

const { withTransaction, withConnection } = require('../../db/transaction');
const repo = require('./requisitions.repository');
const productionOrdersRepo = require('../productionOrders/productionOrders.repository');
const sequences = require('../../shared/sequences.service');
const stock = require('../../shared/stock.service');
const { round } = require('../../utils/rounding');
const { ConflictError, BusinessError } = require('../../utils/errors');
const env = require('../../config/env');

/** Valida que a OP está em produção (POSICAO='P') — precondição de toda requisição. */
async function assertOrderInProduction(connection, numOp) {
  const current = await productionOrdersRepo.getForValidation(connection, numOp);
  if (!current) {
    throw new ConflictError(`Ordem de produção ${numOp} não encontrada.`, 'PRODUCTION_ORDER_NOT_FOUND');
  }
  if (current.POSICAO !== 'P') {
    throw new ConflictError(
      `A OP ${numOp} não está em produção (posição atual: ${current.POSICAO}) — requisição de insumos não permitida.`,
      'ORDER_NOT_IN_PRODUCTION',
      { posicao: current.POSICAO }
    );
  }
  return current;
}

/**
 * Calcula a necessidade escalada pela quantidade solicitada e identifica
 * insumos sem saldo suficiente (Winthor e/ou WMS, quando aplicável) — réplica
 * da etapa de validação de `RequisitarInsumos`.
 */
async function checkAvailability(connection, { numOp, qty, codFilialEstoque, qtProduzirOriginal }) {
  const disponibilidade = await repo.getDisponibilidade(connection, { numOp, codFilialEstoque });

  const itens = disponibilidade.map((row) => {
    const qtNecessidadeEscalada = round((Number(row.QTNECESSIDADE) * qty) / qtProduzirOriginal, 3);
    return {
      codProd: row.CODPROD,
      descricao: row.DESCRICAO,
      usaWms: row.USAWMS === 'S',
      qtNecessidade: qtNecessidadeEscalada,
      qtDisponivelWinthor: Number(row.QT_DISP_WINTHOR),
      qtDisponivelWms: Number(row.QT_DISP_WMS)
    };
  });

  const faltantes = itens.filter(
    (item) =>
      item.usaWms &&
      (item.qtNecessidade > item.qtDisponivelWinthor || item.qtNecessidade > item.qtDisponivelWms)
  );

  return { itens, faltantes };
}

async function previewRequisition(numOp, { qty, codFilialEstoque = env.defaults.codFilialEstoque }) {
  return withConnection(async (connection) => {
    const current = await assertOrderInProduction(connection, numOp);
    const { itens, faltantes } = await checkAvailability(connection, {
      numOp,
      qty,
      codFilialEstoque,
      qtProduzirOriginal: Number(current.QTPRODUZIR)
    });
    return { numOp, qty, itens, insuficiente: faltantes.length > 0, faltantes };
  });
}

/**
 * Aloca a próxima parcela a requisitar de um insumo controlado por lote.
 *
 * Corrige um bug identificado no VB.NET original (`RequisitarInsumos`): lá, a
 * comparação de quanto requisitar em cada rodada usa a necessidade TOTAL do
 * item (`dt.Rows(i)("QTNECESSIDADE")`) em vez do que efetivamente falta
 * requisitar (`FALTAREQUISITAR`). Isso faz com que, sempre que um insumo
 * precisa de mais de um lote para ser totalmente atendido, a partir da
 * segunda rodada o código requisite o saldo inteiro do lote da vez mesmo
 * quando ele excede o que realmente falta — sobre-requisitando o insumo.
 * Aqui a comparação é feita corretamente contra o restante (`faltaRequisitar`).
 */
function allocateNextLotBatch(lotes, faltaRequisitar) {
  const lote = lotes[0];
  if (!lote) return null;

  const disponivelNoLote = round(Number(lote.QTNECESSIDADE) - Number(lote.QTREQUISITADO), 3);
  const qtRequisitar = round(Math.min(faltaRequisitar, disponivelNoLote), 3);

  return { numLote: lote.NUMLOTE, qtRequisitar };
}

/**
 * Executa a requisição/separação de materiais de uma OP — réplica de
 * `RequisitarInsumos` (frmSeparacaoMaterial.vb): valida disponibilidade,
 * numera a transação, baixa os insumos (respeitando FEFO para os
 * controlados por lote) e efetiva via PKG_ESTOQUE.VENDAS_SAIDA, tudo em uma
 * única transação.
 */
async function executeRequisition(numOp, { qty, codFilialEstoque = env.defaults.codFilialEstoque, codFilialProducao = env.defaults.codFilialProducao }, user) {
  return withTransaction(async (connection) => {
    const current = await assertOrderInProduction(connection, numOp);
    const qtProduzirOriginal = Number(current.QTPRODUZIR);

    const { faltantes } = await checkAvailability(connection, {
      numOp,
      qty,
      codFilialEstoque,
      qtProduzirOriginal
    });

    if (faltantes.length > 0) {
      throw new BusinessError(
        'Os seguintes produtos não têm estoque suficiente para essa movimentação.',
        'INSUFFICIENT_STOCK',
        faltantes
      );
    }

    const numTransVenda = await sequences.nextNumTransVenda(connection);
    const numSeq = await sequences.nextNumSeqForOp(connection, numOp);

    const itensBrutos = await repo.getItensParaRequisitar(connection, { numOp, codFilialEstoque });
    const itens = itensBrutos.map((row) => {
      const qtNecessidade = round((Number(row.QTNECESSIDADE) * qty) / qtProduzirOriginal, 3);
      return { ...row, QTNECESSIDADE: qtNecessidade, FALTAREQUISITAR: qtNecessidade };
    });

    const movimentosGerados = [];

    // Réplica do laço REVALIDAR: repete até todo item ter FALTAREQUISITAR = 0.
    // eslint-disable-next-line no-constant-condition
    while (true) {
      const pendentes = itens.filter((item) => item.FALTAREQUISITAR > 0);
      if (pendentes.length === 0) break;

      for (const item of pendentes) {
        const numTransItem = await sequences.nextNumTransItem(connection);
        let numLote;
        let qtRequisitar;

        if (item.ESTOQUEPORLOTE === 'S') {
          const lotes = await repo.getLotesParaRequisitar(connection, {
            numOp,
            codProd: item.CODPROD,
            codFilialEstoque
          });

          const alocacao = allocateNextLotBatch(lotes, item.FALTAREQUISITAR);
          if (!alocacao) {
            throw new BusinessError(
              `Produto usa controle de lotes e não há mais lotes disponíveis para requisitar na OP ${numOp}.`,
              'NO_LOT_AVAILABLE',
              { codProd: item.CODPROD, descricao: item.DESCRICAO }
            );
          }
          numLote = alocacao.numLote;
          qtRequisitar = alocacao.qtRequisitar;
        } else {
          numLote = '1';
          qtRequisitar = item.FALTAREQUISITAR;
        }

        item.FALTAREQUISITAR = round(item.FALTAREQUISITAR - qtRequisitar, 3);

        await repo.insertPcmov(connection, {
          codProd: item.CODPROD,
          qt: qtRequisitar,
          custoReal: item.CUSTOREAL,
          custoFin: item.CUSTOFIN,
          custoCont: item.CUSTOCONT,
          valorUltEnt: item.VALORULTENT,
          custoUltEnt: item.CUSTOULTENT,
          codFilial: codFilialProducao,
          numLote,
          numOp,
          usuario: user.matricula,
          numTransVenda,
          numTransItem,
          numSeq
        });
        await repo.insertPcmovcomple(connection, numTransItem);

        if (item.ESTOQUEPORLOTE === 'S') {
          await repo.updatePcopiloteRequisitado(connection, {
            numOp,
            numLote,
            codProd: item.CODPROD,
            qt: qtRequisitar
          });
        }

        movimentosGerados.push({ codProd: item.CODPROD, numLote, qt: qtRequisitar });
      }
    }

    await stock.vendasSaida(connection, { numTransVenda });

    const movimentos = await repo.getMovimentosPorTransacao(connection, numTransVenda);
    for (const mov of movimentos) {
      await repo.updateGiroEstoque(connection, {
        codProd: mov.CODPROD,
        codFilial: mov.CODFILIAL,
        qt: Number(mov.QT)
      });
      await repo.updatePcopiPosRequisicao(connection, {
        codProd: mov.CODPROD,
        numOp: mov.NUMOP,
        qt: Number(mov.QT)
      });
    }

    return { numOp, numTransVenda, movimentos: movimentosGerados };
  });
}

/**
 * Divide as quantidades dos insumos da OP por um divisor (ex.: volume/palete)
 * apenas para fins de impressão de requisição parcial — réplica de `DividirOP`.
 *
 * Nota de fidelidade: o VB.NET original lê a coluna `dt.Rows(x)("QT")`, mas a
 * consulta de origem (`PesquisarItensOP`) na verdade retorna a coluna com o
 * nome `QTNECESSIDADE` — ou seja, o código original lançaria uma exceção em
 * tempo de execução (capturada e exibida como "Erro ao realizar divisão da
 * OP") sempre que chamado. Aqui usamos o nome de coluna correto para que a
 * funcionalidade realmente funcione, mantendo a mesma fórmula de divisão.
 */
async function splitRequisition(numOp, { divisor, qty, codFilialEstoque = env.defaults.codFilialEstoque }) {
  return withConnection(async (connection) => {
    await productionOrdersRepo.getForValidation(connection, numOp);
    const itens = await productionOrdersRepo.getItems(connection, { numOp, codFilialEstoque });

    return itens.map((item) => ({
      numOp,
      codProd: item.CODPROD,
      descricao: item.DESCRICAO,
      numLote: item.NUMLOTE,
      qt: round((Number(item.QTNECESSIDADE) * divisor) / qty, 3)
    }));
  });
}

module.exports = { previewRequisition, executeRequisition, splitRequisition, allocateNextLotBatch };

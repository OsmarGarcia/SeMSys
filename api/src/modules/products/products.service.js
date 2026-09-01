'use strict';

const { withTransaction, withConnection } = require('../../db/transaction');
const repo = require('./products.repository');
const { explodeFormula, getMetodosDisponiveis } = require('../../shared/formula.service');
const stock = require('../../shared/stock.service');
const { NotFoundError } = require('../../utils/errors');
const env = require('../../config/env');

async function getProduct(codProd) {
  return withConnection(async (connection) => {
    const produto = await repo.getProduto(connection, codProd);
    if (!produto) throw new NotFoundError(`Produto ${codProd} não encontrado.`, 'PRODUCT_NOT_FOUND');

    return {
      codProd: String(produto.CODPROD),
      descricao: produto.DESCRICAO,
      embalagem: produto.EMBALAGEM,
      qtUnitCx: Number(produto.QTUNITCX),
      tipoMerc: produto.TIPOMERC,
      controlaLote: produto.ESTOQUEPORLOTE === 'S',
      prefixoLote: produto.PREFIXOLOTE,
      tipoLote: produto.TIPOLOTE,
      velocidadeNominalM30: Number(produto.DESCRICAO1),
      velocidadeNominalOutrasLinhas: Number(produto.DESCRICAO2)
    };
  });
}

async function getMethods(codProd, { branch = env.defaults.codFilialProducao } = {}) {
  return withConnection((connection) => getMetodosDisponiveis(connection, { codProd, codFilial: branch }));
}

async function getFormula(codProd, { method, branch = env.defaults.codFilialProducao, qty, unitDecimals, decimals = 6 }) {
  return withConnection((connection) =>
    explodeFormula(connection, {
      codProdMaster: codProd,
      metodo: method,
      codFilial: branch,
      qty,
      unitDecimals: unitDecimals ?? null,
      decimals
    })
  );
}

async function getStock(codProd, { branch = env.defaults.codFilialEstoque } = {}) {
  return withConnection(async (connection) => ({
    codProd,
    codFilial: branch,
    estoqueDisponivel: await stock.getEstoqueDisponivel(connection, { codProd, codFilial: branch })
  }));
}

async function recalculateStock(codProd, { branch = env.defaults.codFilialEstoque } = {}) {
  return withTransaction(async (connection) => {
    await stock.recalcularReserva(connection, { codProd, codFilial: branch });
    return { codProd, codFilial: branch, recalculado: true };
  });
}

async function getLots(codProd, { branch = env.defaults.codFilialEstoque } = {}) {
  return withConnection((connection) => stock.getLotesDisponiveis(connection, { codProd, codFilial: branch }));
}

module.exports = { getProduct, getMethods, getFormula, getStock, recalculateStock, getLots };

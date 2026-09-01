'use strict';

const service = require('./products.service');

async function getOne(req, res) {
  res.json({ data: await service.getProduct(req.params.codProd) });
}

async function getMethods(req, res) {
  res.json({ data: await service.getMethods(req.params.codProd, req.query) });
}

async function getFormula(req, res) {
  res.json({ data: await service.getFormula(req.params.codProd, req.query) });
}

async function getStock(req, res) {
  res.json({ data: await service.getStock(req.params.codProd, req.query) });
}

async function recalculateStock(req, res) {
  res.json({ data: await service.recalculateStock(req.params.codProd, req.body) });
}

async function getLots(req, res) {
  res.json({ data: await service.getLots(req.params.codProd, req.query) });
}

module.exports = { getOne, getMethods, getFormula, getStock, recalculateStock, getLots };

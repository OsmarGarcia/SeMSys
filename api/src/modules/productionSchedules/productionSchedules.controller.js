'use strict';

const service = require('./productionSchedules.service');

async function create(_req, res) {
  const result = await service.createSchedule();
  res.status(201).json({ data: result });
}

async function getOne(req, res) {
  const result = await service.getSchedule(req.params.codPrograma, req.query);
  res.json({ data: result });
}

async function addItem(req, res) {
  const result = await service.addItem(req.params.codPrograma, req.body);
  res.status(201).json({ data: result });
}

async function removeItem(req, res) {
  const result = await service.removeItem(req.params.codPrograma, req.params.itemId, req.query);
  res.json({ data: result });
}

async function explodeMaterials(req, res) {
  const result = await service.explodeMaterials(req.params.codPrograma, req.body);
  res.json({ data: result });
}

async function generateOrders(req, res) {
  const result = await service.generateOrders(req.params.codPrograma, req.user);
  res.json({ data: result });
}

async function printData(req, res) {
  const result = await service.getPrintData(req.params.codPrograma);
  res.json({ data: result });
}

module.exports = { create, getOne, addItem, removeItem, explodeMaterials, generateOrders, printData };

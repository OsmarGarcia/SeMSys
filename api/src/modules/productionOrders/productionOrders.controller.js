'use strict';

const service = require('./productionOrders.service');

async function list(req, res) {
  const orders = await service.listOrders(req.query);
  res.json({ data: orders });
}

async function getOne(req, res) {
  const order = await service.getOrder(req.params.numOp);
  res.json({ data: order });
}

async function getItems(req, res) {
  const items = await service.getItems(req.params.numOp, req.query);
  res.json({ data: items });
}

async function getMovements(req, res) {
  const movements = await service.getMovements(req.params.numOp);
  res.json({ data: movements });
}

async function getLabel(req, res) {
  const label = await service.getLabel(req.params.numOp, req.query);
  res.json({ data: label });
}

async function create(req, res) {
  const order = await service.createOrder(req.body, req.user);
  res.status(201).json({ data: order });
}

async function reprogram(req, res) {
  const result = await service.reprogramOrder(req.params.numOp, req.body);
  res.status(result.changed === false ? 200 : 200).json({ data: result });
}

async function recalculateItems(req, res) {
  const result = await service.recalculateItems(req.params.numOp, req.body);
  res.json({ data: result });
}

async function cancel(req, res) {
  const result = await service.cancelOrder(req.params.numOp);
  res.json({ data: result });
}

async function start(req, res) {
  const result = await service.startProductionOrder(req.params.numOp, req.user);
  res.json({ data: result });
}

module.exports = {
  list,
  getOne,
  getItems,
  getMovements,
  getLabel,
  create,
  reprogram,
  recalculateItems,
  cancel,
  start
};

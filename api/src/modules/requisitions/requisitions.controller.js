'use strict';

const service = require('./requisitions.service');

async function preview(req, res) {
  const result = await service.previewRequisition(req.params.numOp, req.query);
  res.json({ data: result });
}

async function create(req, res) {
  const result = await service.executeRequisition(req.params.numOp, req.body, req.user);
  res.status(201).json({ data: result });
}

async function split(req, res) {
  const result = await service.splitRequisition(req.params.numOp, req.query);
  res.json({ data: result });
}

module.exports = { preview, create, split };

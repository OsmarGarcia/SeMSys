'use strict';

const { Router } = require('express');
const controller = require('./productionSchedules.controller');
const validators = require('./productionSchedules.validators');
const validate = require('../../middleware/validate');
const { requireAuth } = require('../../middleware/auth');
const asyncHandler = require('../../utils/asyncHandler');

const router = Router();

router.use(requireAuth);

router.post('/', asyncHandler(controller.create));

router.get(
  '/:codPrograma',
  validate(validators.codProgramaParam, 'params'),
  validate(validators.scheduleQuery, 'query'),
  asyncHandler(controller.getOne)
);

router.post(
  '/:codPrograma/items',
  validate(validators.codProgramaParam, 'params'),
  validate(validators.addItemBody, 'body'),
  asyncHandler(controller.addItem)
);

router.delete(
  '/:codPrograma/items/:itemId',
  validate(validators.itemParam, 'params'),
  validate(validators.removeItemQuery, 'query'),
  asyncHandler(controller.removeItem)
);

router.post(
  '/:codPrograma/materials/explode',
  validate(validators.codProgramaParam, 'params'),
  validate(validators.explodeBody, 'body'),
  asyncHandler(controller.explodeMaterials)
);

router.post(
  '/:codPrograma/generate-orders',
  validate(validators.codProgramaParam, 'params'),
  asyncHandler(controller.generateOrders)
);

router.get(
  '/:codPrograma/print',
  validate(validators.codProgramaParam, 'params'),
  asyncHandler(controller.printData)
);

module.exports = router;

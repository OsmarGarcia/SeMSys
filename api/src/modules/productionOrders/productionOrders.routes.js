'use strict';

const { Router } = require('express');
const controller = require('./productionOrders.controller');
const validators = require('./productionOrders.validators');
const validate = require('../../middleware/validate');
const { requireAuth } = require('../../middleware/auth');
const asyncHandler = require('../../utils/asyncHandler');
const requisitionsRoutes = require('../requisitions/requisitions.routes');

const router = Router();

router.use(requireAuth);

router.get('/', validate(validators.searchQuery, 'query'), asyncHandler(controller.list));

router.post('/', validate(validators.createOrderBody, 'body'), asyncHandler(controller.create));

router.get(
  '/:numOp',
  validate(validators.numOpParam, 'params'),
  asyncHandler(controller.getOne)
);

router.get(
  '/:numOp/items',
  validate(validators.numOpParam, 'params'),
  validate(validators.itemsQuery, 'query'),
  asyncHandler(controller.getItems)
);

router.get(
  '/:numOp/movements',
  validate(validators.numOpParam, 'params'),
  asyncHandler(controller.getMovements)
);

router.get(
  '/:numOp/label',
  validate(validators.numOpParam, 'params'),
  validate(validators.labelQuery, 'query'),
  asyncHandler(controller.getLabel)
);

router.post(
  '/:numOp/reprogram',
  validate(validators.numOpParam, 'params'),
  validate(validators.reprogramBody, 'body'),
  asyncHandler(controller.reprogram)
);

router.post(
  '/:numOp/recalculate-items',
  validate(validators.numOpParam, 'params'),
  validate(validators.recalculateItemsBody, 'body'),
  asyncHandler(controller.recalculateItems)
);

router.post(
  '/:numOp/cancel',
  validate(validators.numOpParam, 'params'),
  asyncHandler(controller.cancel)
);

router.post(
  '/:numOp/start',
  validate(validators.numOpParam, 'params'),
  asyncHandler(controller.start)
);

// /production-orders/:numOp/requisitions/*  (ver módulo requisitions)
router.use('/:numOp/requisitions', validate(validators.numOpParam, 'params'), requisitionsRoutes);

module.exports = router;

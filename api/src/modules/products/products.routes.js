'use strict';

const { Router } = require('express');
const controller = require('./products.controller');
const validators = require('./products.validators');
const validate = require('../../middleware/validate');
const { requireAuth } = require('../../middleware/auth');
const asyncHandler = require('../../utils/asyncHandler');

const router = Router();

router.use(requireAuth);
router.use('/:codProd', validate(validators.codProdParam, 'params'));

router.get('/:codProd', asyncHandler(controller.getOne));

router.get('/:codProd/methods', validate(validators.branchQuery, 'query'), asyncHandler(controller.getMethods));

router.get('/:codProd/formula', validate(validators.formulaQuery, 'query'), asyncHandler(controller.getFormula));

router.get('/:codProd/stock', validate(validators.branchQuery, 'query'), asyncHandler(controller.getStock));

router.post('/:codProd/stock/recalculate', validate(validators.branchQuery, 'body'), asyncHandler(controller.recalculateStock));

router.get('/:codProd/lots', validate(validators.branchQuery, 'query'), asyncHandler(controller.getLots));

module.exports = router;

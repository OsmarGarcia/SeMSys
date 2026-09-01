'use strict';

const { Router } = require('express');
const controller = require('./requisitions.controller');
const validators = require('./requisitions.validators');
const validate = require('../../middleware/validate');
const asyncHandler = require('../../utils/asyncHandler');

// mergeParams: true — precisa enxergar :numOp definido no router pai (production-orders).
const router = Router({ mergeParams: true });

router.get('/preview', validate(validators.previewQuery, 'query'), asyncHandler(controller.preview));

router.get('/split', validate(validators.splitQuery, 'query'), asyncHandler(controller.split));

router.post('/', validate(validators.createBody, 'body'), asyncHandler(controller.create));

module.exports = router;

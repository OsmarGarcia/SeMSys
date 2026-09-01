'use strict';

const { z } = require('zod');

const previewQuery = z.object({
  qty: z.coerce.number().positive('qty deve ser maior que zero.'),
  codFilialEstoque: z.string().optional()
});

const createBody = z.object({
  qty: z.number().positive('qty deve ser maior que zero.'),
  codFilialEstoque: z.string().optional(),
  codFilialProducao: z.string().optional()
});

const splitQuery = z.object({
  divisor: z.coerce.number().positive('divisor deve ser maior que zero.'),
  qty: z.coerce.number().positive('qty deve ser maior que zero.'),
  codFilialEstoque: z.string().optional()
});

module.exports = { previewQuery, createBody, splitQuery };

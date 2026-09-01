'use strict';

const { z } = require('zod');

const codProdParam = z.object({
  codProd: z.string().min(1)
});

const branchQuery = z.object({
  branch: z.string().optional()
});

const formulaQuery = z.object({
  method: z.string().min(1, 'method é obrigatório.'),
  branch: z.string().optional(),
  qty: z.coerce.number().positive('qty deve ser maior que zero.'),
  unitDecimals: z.coerce.number().int().optional(),
  decimals: z.coerce.number().int().optional()
});

module.exports = { codProdParam, branchQuery, formulaQuery };

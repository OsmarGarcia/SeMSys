'use strict';

const { z } = require('zod');

const numOpParam = z.object({
  numOp: z.string().regex(/^\d+$/, 'numOp deve ser numérico.')
});

const searchQuery = z
  .object({
    position: z.enum(['L', 'P', 'F', 'C']),
    codFilial: z.string().optional(),
    startDateFrom: z.string().datetime().optional().or(z.string().date().optional()),
    startDateTo: z.string().datetime().optional().or(z.string().date().optional())
  })
  .refine(
    (data) => data.position !== 'L' || (data.startDateFrom && data.startDateTo),
    {
      message: 'startDateFrom e startDateTo são obrigatórios ao filtrar position=L (aguardando início).',
      path: ['startDateFrom']
    }
  );

const createOrderBody = z.object({
  codProd: z.string().min(1, 'codProd é obrigatório.'),
  metodo: z.string().min(1, 'metodo é obrigatório.'),
  qtProduzir: z.number().positive('qtProduzir deve ser maior que zero.'),
  dtPrevInicio: z.string().datetime().optional(),
  codFilial: z.string().optional()
});

const reprogramBody = z.object({
  novaQtProduzir: z.number().positive('novaQtProduzir deve ser maior que zero.'),
  numLote: z.string().optional(),
  dtPrevInicio: z.string().datetime().optional()
});

const recalculateItemsBody = z.object({
  metodo: z.string().optional()
});

const itemsQuery = z.object({
  codFilialEstoque: z.string().optional()
});

const labelQuery = z.object({
  offset: z.coerce.number().int().default(0)
});

module.exports = {
  numOpParam,
  searchQuery,
  createOrderBody,
  reprogramBody,
  recalculateItemsBody,
  itemsQuery,
  labelQuery
};

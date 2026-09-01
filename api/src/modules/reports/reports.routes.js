'use strict';

const { Router } = require('express');
const { z } = require('zod');
const { requireAuth } = require('../../middleware/auth');
const validate = require('../../middleware/validate');
const asyncHandler = require('../../utils/asyncHandler');
const { withConnection } = require('../../db/transaction');
const repo = require('./reports.repository');
const { toBrDate } = require('../../utils/dates');
const { round } = require('../../utils/rounding');

const router = Router();

router.use(requireAuth);

const query = z.object({
  startDate: z.string().datetime().or(z.string().date()),
  endDate: z.string().datetime().or(z.string().date()),
  // Defaults réplicam exatamente os filtros fixos do VB.NET original
  // (`CODFILIAL IN (1,4)`, `CODEPTO IN ('30','40')`); expostos como parâmetro
  // apenas para permitir ajuste pontual sem alterar o comportamento padrão.
  branches: z
    .string()
    .optional()
    .transform((value) => (value ? value.split(',').map((v) => v.trim()) : ['1', '4'])),
  departments: z
    .string()
    .optional()
    .transform((value) => (value ? value.split(',').map((v) => v.trim()) : ['30', '40']))
});

router.get(
  '/production-total',
  validate(query, 'query'),
  asyncHandler(async (req, res) => {
    const { startDate, endDate, branches, departments } = req.query;

    const rows = await withConnection((connection) =>
      repo.getProducaoTotal(connection, {
        dtInicio: toBrDate(startDate),
        dtFim: toBrDate(endDate),
        filiais: branches,
        departamentos: departments
      })
    );

    const data = rows.map((row) => ({
      ...row,
      QTMASTER: row.QTUNITCX ? round(Number(row.QT) / Number(row.QTUNITCX), 4) : null
    }));

    res.json({ data });
  })
);

module.exports = router;

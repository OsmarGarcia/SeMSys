'use strict';

const { Router } = require('express');
const { requireAuth } = require('../../middleware/auth');
const asyncHandler = require('../../utils/asyncHandler');
const { withConnection } = require('../../db/transaction');

const router = Router();

router.use(requireAuth);

/** Lista filiais cadastradas — réplica da consulta usada em `frmManutencaoOP_Load`. */
router.get(
  '/',
  asyncHandler(async (_req, res) => {
    const codigos = await withConnection(async (connection) => {
      const result = await connection.execute('SELECT DISTINCT CODIGO FROM PCFILIAL ORDER BY CODIGO');
      return result.rows.map((row) => row.CODIGO);
    });
    res.json({ data: codigos });
  })
);

module.exports = router;

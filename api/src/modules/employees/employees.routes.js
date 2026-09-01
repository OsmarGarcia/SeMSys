'use strict';

const { Router } = require('express');
const { requireAuth } = require('../../middleware/auth');
const asyncHandler = require('../../utils/asyncHandler');
const { withConnection } = require('../../db/transaction');
const { NotFoundError } = require('../../utils/errors');

const router = Router();

router.use(requireAuth);

/** Nome do funcionário por matrícula (PCEMPR) — uso interno em relatórios/apontamentos. */
router.get(
  '/:matricula',
  asyncHandler(async (req, res) => {
    const { matricula } = req.params;
    const funcionario = await withConnection(async (connection) => {
      const result = await connection.execute(
        'SELECT MATRICULA AS MATRICULA, NOME AS NOME FROM PCEMPR WHERE MATRICULA = :matricula',
        { matricula }
      );
      return result.rows[0] || null;
    });

    if (!funcionario) {
      throw new NotFoundError(`Funcionário ${matricula} não encontrado.`, 'EMPLOYEE_NOT_FOUND');
    }

    res.json({ data: { matricula: funcionario.MATRICULA, nome: funcionario.NOME } });
  })
);

module.exports = router;

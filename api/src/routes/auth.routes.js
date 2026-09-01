'use strict';

const { Router } = require('express');
const jwt = require('jsonwebtoken');
const { z } = require('zod');
const env = require('../config/env');
const validate = require('../middleware/validate');
const asyncHandler = require('../utils/asyncHandler');
const { ForbiddenError } = require('../utils/errors');

const router = Router();

const devTokenBody = z.object({
  matricula: z.string().min(1),
  nome: z.string().optional(),
  codFilial: z.string().optional(),
  expiresIn: z.string().optional().default('8h')
});

/**
 * Emissão de token SOMENTE para desenvolvimento/testes locais — NUNCA habilitar
 * em produção (`ENABLE_DEV_TOKEN=false`). Em produção, o token deve vir de
 * quem autentica de fato o usuário contra o cadastro de funcionários do
 * Winthor (PCEMPR) — ver comentário em `middleware/auth.js`.
 */
router.post(
  '/dev-token',
  validate(devTokenBody, 'body'),
  asyncHandler(async (req, res) => {
    if (!env.auth.enableDevToken) {
      throw new ForbiddenError('Emissão de token de desenvolvimento está desabilitada (ENABLE_DEV_TOKEN=false).');
    }

    const { matricula, nome, codFilial, expiresIn } = req.body;
    const token = jwt.sign(
      { matricula, nome, codFilial },
      env.auth.jwtSecret,
      env.auth.expectedIssuer ? { expiresIn, issuer: env.auth.expectedIssuer } : { expiresIn }
    );

    res.json({ data: { token, expiresIn } });
  })
);

module.exports = router;

'use strict';

const jwt = require('jsonwebtoken');
const env = require('../config/env');
const { UnauthorizedError } = require('../utils/errors');

/**
 * Autenticação via JWT.
 *
 * Esta API não reimplementa o login do Winthor: ela espera receber um token já
 * emitido por quem autentica o usuário contra o cadastro de funcionários
 * (PCEMPR) — normalmente o mesmo serviço de SSO/gateway usado pelo restante do
 * ERP. O token precisa conter, no mínimo:
 *   { matricula: "1234", nome: "FULANO DE TAL", codFilial: "1" }
 *
 * Esses três campos substituem, na API, o `My.Settings.UsuarioWinthor` /
 * `My.Settings.NomeWinthor` / `My.Settings.CodFilialProducao` lidos hoje da
 * configuração local do executável — e são usados para preencher
 * CODFUNCLANC / CODFUNCINICIO / CODFUNCREQ nas tabelas do Winthor, nunca
 * aceitos como campo livre vindo do corpo da requisição (ver seção 6, item 8
 * do plano de API).
 */
function requireAuth(req, _res, next) {
  const header = req.headers.authorization || '';
  const [scheme, token] = header.split(' ');

  if (scheme !== 'Bearer' || !token) {
    return next(new UnauthorizedError('Envie um token Bearer válido no cabeçalho Authorization.'));
  }

  try {
    const payload = jwt.verify(
      token,
      env.auth.jwtSecret,
      env.auth.expectedIssuer ? { issuer: env.auth.expectedIssuer } : {}
    );

    if (!payload.matricula) {
      return next(new UnauthorizedError('Token não contém a matrícula do usuário (matricula).'));
    }

    req.user = {
      matricula: String(payload.matricula),
      nome: payload.nome || '',
      codFilial: payload.codFilial ? String(payload.codFilial) : undefined
    };

    return next();
  } catch (err) {
    return next(new UnauthorizedError(`Token inválido ou expirado: ${err.message}`));
  }
}

module.exports = { requireAuth };

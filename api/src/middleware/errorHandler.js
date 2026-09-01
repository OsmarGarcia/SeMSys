'use strict';

const { AppError } = require('../utils/errors');
const logger = require('../logger');

/**
 * Traduz erros ORA-xxxxx e códigos de infraestrutura do oracledb em algo que não
 * vaza detalhes internos do banco para o cliente, mas ainda é logado por inteiro.
 */
function mapOracleError(err) {
  const oraCode = /ORA-\d{5}/.exec(err.message || '')?.[0];

  if (oraCode === 'ORA-00001') {
    return new AppError('Registro duplicado — violação de chave única.', {
      status: 409,
      code: 'DUPLICATE_KEY'
    });
  }
  if (oraCode === 'ORA-02291' || oraCode === 'ORA-02292') {
    return new AppError('Operação viola uma restrição de integridade referencial.', {
      status: 409,
      code: 'INTEGRITY_CONSTRAINT'
    });
  }
  if (err.message && err.message.startsWith('NJS-040')) {
    return new AppError('Conexão com o Oracle indisponível no momento.', {
      status: 503,
      code: 'DATABASE_UNAVAILABLE'
    });
  }
  if (err.message && err.message.includes('DPI-1067')) {
    return new AppError('Tempo limite de execução no banco excedido.', {
      status: 504,
      code: 'DATABASE_TIMEOUT'
    });
  }

  return new AppError('Erro inesperado ao acessar o banco de dados.', {
    status: 500,
    code: 'DATABASE_ERROR'
  });
}

// eslint-disable-next-line no-unused-vars
function errorHandler(err, req, res, _next) {
  let appError = err;

  if (!(err instanceof AppError)) {
    const looksLikeOracleError = typeof err.message === 'string' && /ORA-|NJS-|DPI-/.test(err.message);
    appError = looksLikeOracleError ? mapOracleError(err) : new AppError(err.message || 'Erro interno.');
  }

  const logPayload = {
    err,
    status: appError.status,
    code: appError.code,
    path: req.path,
    method: req.method,
    user: req.user?.matricula
  };

  if (appError.status >= 500) {
    logger.error(logPayload, 'Erro não tratado na requisição');
  } else {
    logger.warn(logPayload, 'Requisição rejeitada');
  }

  res.status(appError.status).json({
    error: appError.code,
    message: appError.message,
    details: appError.details
  });
}

module.exports = errorHandler;

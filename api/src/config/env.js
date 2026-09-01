'use strict';

require('dotenv').config();

function required(name, fallback) {
  const value = process.env[name] ?? fallback;
  if (value === undefined) {
    throw new Error(`Variável de ambiente obrigatória ausente: ${name}`);
  }
  return value;
}

function toInt(value, fallback) {
  const parsed = Number.parseInt(value, 10);
  return Number.isFinite(parsed) ? parsed : fallback;
}

function toBool(value, fallback) {
  if (value === undefined) return fallback;
  return String(value).toLowerCase() === 'true';
}

const env = {
  nodeEnv: process.env.NODE_ENV || 'development',
  port: toInt(process.env.PORT, 3000),

  oracle: {
    user: required('ORACLE_USER'),
    password: required('ORACLE_PASSWORD'),
    connectString: required('ORACLE_CONNECT_STRING'),
    poolMin: toInt(process.env.ORACLE_POOL_MIN, 2),
    poolMax: toInt(process.env.ORACLE_POOL_MAX, 10),
    poolIncrement: toInt(process.env.ORACLE_POOL_INCREMENT, 1),
    poolTimeout: toInt(process.env.ORACLE_POOL_TIMEOUT, 60),
    queryTimeoutMs: toInt(process.env.ORACLE_QUERY_TIMEOUT_MS, 30000)
  },

  defaults: {
    codFilialProducao: required('DEFAULT_CODFILIAL_PRODUCAO', '1'),
    codFilialEstoque: required('DEFAULT_CODFILIAL_ESTOQUE', '1')
  },

  auth: {
    jwtSecret: required('JWT_SECRET'),
    expectedIssuer: process.env.JWT_EXPECTED_ISSUER || undefined,
    enableDevToken: toBool(process.env.ENABLE_DEV_TOKEN, false)
  },

  cors: {
    allowedOrigins: (process.env.CORS_ALLOWED_ORIGINS || '*')
      .split(',')
      .map((origin) => origin.trim())
      .filter(Boolean)
  },

  logLevel: process.env.LOG_LEVEL || 'info'
};

module.exports = env;

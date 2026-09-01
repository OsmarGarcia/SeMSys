'use strict';

const oracledb = require('oracledb');
const env = require('../config/env');
const logger = require('../logger');

// Modo "thin" (padrão do driver a partir da v6) — não exige Oracle Instant Client
// instalado na máquina, o que facilita implantar a API em qualquer ambiente.
oracledb.outFormat = oracledb.OUT_FORMAT_OBJECT;
oracledb.autoCommit = false; // cada caso de uso controla explicitamente commit/rollback
oracledb.fetchAsString = [oracledb.CLOB];

const POOL_ALIAS = 'winthor';

async function createPool() {
  await oracledb.createPool({
    poolAlias: POOL_ALIAS,
    user: env.oracle.user,
    password: env.oracle.password,
    connectString: env.oracle.connectString,
    poolMin: env.oracle.poolMin,
    poolMax: env.oracle.poolMax,
    poolIncrement: env.oracle.poolIncrement,
    poolTimeout: env.oracle.poolTimeout,
    queueTimeout: env.oracle.queryTimeoutMs
  });
  logger.info({ connectString: env.oracle.connectString }, 'Pool Oracle criado');
}

async function closePool() {
  try {
    await oracledb.getPool(POOL_ALIAS).close(10);
    logger.info('Pool Oracle encerrado');
  } catch (err) {
    if (err.message && err.message.includes('NJS-047')) return; // pool já fechado
    logger.error({ err }, 'Erro ao encerrar o pool Oracle');
  }
}

/**
 * Obtém uma conexão do pool. O chamador é responsável por liberar a conexão
 * (connection.close()) em um bloco finally.
 */
async function getConnection() {
  const pool = oracledb.getPool(POOL_ALIAS);
  const connection = await pool.getConnection();
  connection.callTimeout = env.oracle.queryTimeoutMs;
  return connection;
}

module.exports = {
  oracledb,
  createPool,
  closePool,
  getConnection,
  POOL_ALIAS
};

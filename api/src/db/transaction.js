'use strict';

const { getConnection } = require('./pool');
const logger = require('../logger');

/**
 * Executa `work(connection)` dentro de uma única transação Oracle.
 *
 * Isso replica o padrão usado em todo o VB.NET original (Oratransaction.BeginTransaction /
 * Commit / Rollback envolvendo várias instruções): cada caso de uso da API deve ser
 * atômico — ou tudo é gravado, ou nada é.
 *
 * `work` recebe a conexão já aberta e deve usar `connection.execute(...)` sem
 * `autoCommit`; o commit/rollback é feito aqui, uma única vez, ao final.
 */
async function withTransaction(work) {
  const connection = await getConnection();
  try {
    const result = await work(connection);
    await connection.commit();
    return result;
  } catch (err) {
    try {
      await connection.rollback();
    } catch (rollbackErr) {
      logger.error({ err: rollbackErr }, 'Falha ao executar rollback da transação');
    }
    throw err;
  } finally {
    try {
      await connection.close();
    } catch (closeErr) {
      logger.error({ err: closeErr }, 'Falha ao liberar conexão Oracle de volta ao pool');
    }
  }
}

/**
 * Executa `work(connection)` em uma conexão dedicada, sem gerenciar transação
 * (uso típico: leituras/GET que não escrevem nada).
 */
async function withConnection(work) {
  const connection = await getConnection();
  try {
    return await work(connection);
  } finally {
    try {
      await connection.close();
    } catch (closeErr) {
      logger.error({ err: closeErr }, 'Falha ao liberar conexão Oracle de volta ao pool');
    }
  }
}

module.exports = { withTransaction, withConnection };

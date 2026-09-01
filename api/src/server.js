'use strict';

const env = require('./config/env');
const logger = require('./logger');
const createApp = require('./app');
const { createPool, closePool } = require('./db/pool');

async function main() {
  await createPool();

  const app = createApp();
  const server = app.listen(env.port, () => {
    logger.info({ port: env.port, env: env.nodeEnv }, 'SeMSys API no ar');
  });

  const shutdown = async (signal) => {
    logger.info({ signal }, 'Encerrando SeMSys API...');
    server.close(async () => {
      await closePool();
      process.exit(0);
    });
    // Força encerramento se não fechar de forma limpa em 10s.
    setTimeout(() => process.exit(1), 10000).unref();
  };

  process.on('SIGINT', () => shutdown('SIGINT'));
  process.on('SIGTERM', () => shutdown('SIGTERM'));
}

main().catch((err) => {
  logger.error({ err }, 'Falha ao iniciar a SeMSys API');
  process.exit(1);
});

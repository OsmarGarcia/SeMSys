'use strict';

const express = require('express');
const helmet = require('helmet');
const cors = require('cors');
const compression = require('compression');
const pinoHttp = require('pino-http');

const env = require('./config/env');
const logger = require('./logger');
const errorHandler = require('./middleware/errorHandler');
const notFound = require('./middleware/notFound');

const authRoutes = require('./routes/auth.routes');
const productionOrdersRoutes = require('./modules/productionOrders/productionOrders.routes');
const productionSchedulesRoutes = require('./modules/productionSchedules/productionSchedules.routes');
const productsRoutes = require('./modules/products/products.routes');
const branchesRoutes = require('./modules/branches/branches.routes');
const employeesRoutes = require('./modules/employees/employees.routes');
const reportsRoutes = require('./modules/reports/reports.routes');

function createApp() {
  const app = express();

  app.disable('x-powered-by');
  app.use(helmet());
  app.use(
    cors({
      origin: env.cors.allowedOrigins.includes('*') ? true : env.cors.allowedOrigins
    })
  );
  app.use(compression());
  app.use(express.json({ limit: '2mb' }));
  app.use(pinoHttp({ logger, autoLogging: env.nodeEnv !== 'test' }));

  app.get('/health', (_req, res) => {
    res.json({ status: 'ok', env: env.nodeEnv, timestamp: new Date().toISOString() });
  });

  app.use('/auth', authRoutes);
  app.use('/production-orders', productionOrdersRoutes);
  app.use('/production-schedules', productionSchedulesRoutes);
  app.use('/products', productsRoutes);
  app.use('/branches', branchesRoutes);
  app.use('/employees', employeesRoutes);
  app.use('/reports', reportsRoutes);

  app.use(notFound);
  app.use(errorHandler);

  return app;
}

module.exports = createApp;

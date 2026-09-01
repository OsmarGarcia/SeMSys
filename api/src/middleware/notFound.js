'use strict';

function notFound(req, res) {
  res.status(404).json({
    error: 'ROUTE_NOT_FOUND',
    message: `Rota não encontrada: ${req.method} ${req.originalUrl}`
  });
}

module.exports = notFound;

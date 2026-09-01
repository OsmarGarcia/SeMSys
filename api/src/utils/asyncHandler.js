'use strict';

/**
 * Envolve um handler assíncrono de rota Express, encaminhando qualquer rejeição
 * para o middleware de erro central (evita "UnhandledPromiseRejection" e o
 * try/catch repetido em cada controller).
 */
function asyncHandler(fn) {
  return function wrapped(req, res, next) {
    Promise.resolve(fn(req, res, next)).catch(next);
  };
}

module.exports = asyncHandler;

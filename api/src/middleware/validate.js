'use strict';

const { ValidationError } = require('../utils/errors');

/**
 * Valida `req[part]` (body/query/params) contra um schema Zod e substitui
 * `req[part]` pelo resultado já parseado/tipado (ex.: strings de query
 * convertidas para número), para os controllers não repetirem `Number(...)`.
 */
function validate(schema, part = 'body') {
  return function validateMiddleware(req, _res, next) {
    const result = schema.safeParse(req[part]);
    if (!result.success) {
      const details = result.error.issues.map((issue) => ({
        path: issue.path.join('.'),
        message: issue.message
      }));
      return next(new ValidationError('Dados de entrada inválidos.', details));
    }
    req[part] = result.data;
    return next();
  };
}

module.exports = validate;

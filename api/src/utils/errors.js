'use strict';

/**
 * Erro de negócio "de primeira classe": carrega um código estável (para o cliente
 * decidir o que fazer programaticamente) e detalhes estruturados, replicando as
 * mensagens de MessageBox.Show do VB.NET só que como dado, não texto solto.
 *
 * Contrato de resposta (ver docs/oracle-integration-rest-api-plan.md, seção 7):
 * { "error": "INSUFFICIENT_STOCK", "message": "...", "details": [...] }
 */
class AppError extends Error {
  constructor(message, { status = 500, code = 'INTERNAL_ERROR', details = undefined } = {}) {
    super(message);
    this.name = this.constructor.name;
    this.status = status;
    this.code = code;
    this.details = details;
    Error.captureStackTrace?.(this, this.constructor);
  }
}

class ValidationError extends AppError {
  constructor(message, details) {
    super(message, { status: 400, code: 'VALIDATION_ERROR', details });
  }
}

class UnauthorizedError extends AppError {
  constructor(message = 'Autenticação necessária.') {
    super(message, { status: 401, code: 'UNAUTHORIZED' });
  }
}

class ForbiddenError extends AppError {
  constructor(message = 'Usuário sem permissão para esta operação.') {
    super(message, { status: 403, code: 'FORBIDDEN' });
  }
}

class NotFoundError extends AppError {
  constructor(message = 'Recurso não encontrado.', code = 'NOT_FOUND') {
    super(message, { status: 404, code });
  }
}

/**
 * Estado do recurso não permite a operação solicitada (ex.: OP cancelada/fechada,
 * tentativa de requisitar OP que não está em produção). Equivale às validações de
 * POSICAO feitas antes de qualquer INSERT/UPDATE no VB.NET original.
 */
class ConflictError extends AppError {
  constructor(message, code = 'CONFLICT', details) {
    super(message, { status: 409, code, details });
  }
}

/**
 * Regra de negócio bloqueou a operação (estoque insuficiente, retorno de PL/SQL
 * diferente de "OK"/"SUCESSO" etc). HTTP 422 — a requisição é válida, mas o
 * domínio recusa executá-la.
 */
class BusinessError extends AppError {
  constructor(message, code = 'BUSINESS_RULE_VIOLATION', details) {
    super(message, { status: 422, code, details });
  }
}

module.exports = {
  AppError,
  ValidationError,
  UnauthorizedError,
  ForbiddenError,
  NotFoundError,
  ConflictError,
  BusinessError
};

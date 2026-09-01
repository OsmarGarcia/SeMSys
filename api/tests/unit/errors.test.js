'use strict';

const { AppError, ValidationError, ConflictError, BusinessError, NotFoundError } = require('../../src/utils/errors');

describe('classes de erro', () => {
  test('AppError usa defaults sensatos', () => {
    const err = new AppError('deu ruim');
    expect(err.status).toBe(500);
    expect(err.code).toBe('INTERNAL_ERROR');
  });

  test('ValidationError é 400 com detalhes', () => {
    const err = new ValidationError('campo inválido', [{ path: 'qty', message: 'obrigatório' }]);
    expect(err.status).toBe(400);
    expect(err.code).toBe('VALIDATION_ERROR');
    expect(err.details).toHaveLength(1);
  });

  test('ConflictError aceita código customizado', () => {
    const err = new ConflictError('OP fechada', 'ORDER_CLOSED');
    expect(err.status).toBe(409);
    expect(err.code).toBe('ORDER_CLOSED');
  });

  test('BusinessError é 422', () => {
    const err = new BusinessError('estoque insuficiente', 'INSUFFICIENT_STOCK', [{ codProd: '123' }]);
    expect(err.status).toBe(422);
    expect(err.details[0].codProd).toBe('123');
  });

  test('NotFoundError é 404', () => {
    const err = new NotFoundError('não achei');
    expect(err.status).toBe(404);
  });
});

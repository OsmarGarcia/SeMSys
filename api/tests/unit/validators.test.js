'use strict';

const { createOrderBody, searchQuery } = require('../../src/modules/productionOrders/productionOrders.validators');

describe('productionOrders.validators', () => {
  test('createOrderBody exige qtProduzir positivo', () => {
    const result = createOrderBody.safeParse({ codProd: '123', metodo: 'PADRAO', qtProduzir: -1 });
    expect(result.success).toBe(false);
  });

  test('createOrderBody aceita payload mínimo válido', () => {
    const result = createOrderBody.safeParse({ codProd: '123', metodo: 'PADRAO', qtProduzir: 100 });
    expect(result.success).toBe(true);
  });

  test('searchQuery exige startDateFrom/startDateTo quando position=L', () => {
    const semDatas = searchQuery.safeParse({ position: 'L' });
    expect(semDatas.success).toBe(false);

    const comDatas = searchQuery.safeParse({
      position: 'L',
      startDateFrom: '2026-01-01',
      startDateTo: '2026-01-31'
    });
    expect(comDatas.success).toBe(true);
  });

  test('searchQuery não exige datas para position=P', () => {
    const result = searchQuery.safeParse({ position: 'P' });
    expect(result.success).toBe(true);
  });
});

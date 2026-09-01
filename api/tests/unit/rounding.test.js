'use strict';

const { round, floorTo } = require('../../src/utils/rounding');

describe('round', () => {
  test('arredonda para o número de casas informado', () => {
    expect(round(1.2345, 2)).toBe(1.23);
    expect(round(1.005, 2)).toBe(1.01);
    expect(round(10, 3)).toBe(10);
  });

  test('retorna 0 para valores não numéricos', () => {
    expect(round('abc', 2)).toBe(0);
    expect(round(undefined, 2)).toBe(0);
  });

  test('decimals=0 arredonda para inteiro', () => {
    expect(round(4.6, 0)).toBe(5);
  });
});

describe('floorTo', () => {
  test('trunca em vez de arredondar', () => {
    expect(floorTo(4.9, 0)).toBe(4);
    expect(floorTo(2.567, 2)).toBe(2.56);
  });
});

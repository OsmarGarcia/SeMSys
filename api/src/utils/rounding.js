'use strict';

/**
 * Arredondamento "meio para cima" (o mesmo comportamento do ROUND do Oracle),
 * usado para reproduzir os cálculos de quantidade/custo que no VB.NET eram
 * feitos dentro do próprio SQL (ROUND(...)) ou via Math.Round.
 */
function round(value, decimals = 0) {
  const factor = 10 ** decimals;
  const num = Number(value);
  if (!Number.isFinite(num)) return 0;
  return Math.round((num + Number.EPSILON) * factor) / factor;
}

/**
 * Trunca casas decimais (equivalente a Math.Floor usado no VB.NET para
 * qtd de turnos/dias inteiros ao recalcular horas de programação).
 */
function floorTo(value, decimals = 0) {
  const factor = 10 ** decimals;
  return Math.floor(Number(value) * factor) / factor;
}

module.exports = { round, floorTo };

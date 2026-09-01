'use strict';

const { ValidationError } = require('./errors');

/**
 * Formata uma data (Date ou string ISO) para o padrão 'DD/MM/YYYY' esperado
 * pelas rotinas Oracle do Winthor (TO_DATE(..., 'DD/MM/YYYY')).
 */
function toBrDate(value) {
  const date = value instanceof Date ? value : new Date(value);
  if (Number.isNaN(date.getTime())) {
    throw new ValidationError(`Data inválida: ${value}`);
  }
  const dd = String(date.getDate()).padStart(2, '0');
  const mm = String(date.getMonth() + 1).padStart(2, '0');
  const yyyy = date.getFullYear();
  return `${dd}/${mm}/${yyyy}`;
}

/**
 * Formata para 'DD/MM/YYYY HH24:MI:SS', usado nos INSERT/UPDATE de
 * DTPREVINICIO/HORAINICIAL/HORAFINAL.
 */
function toBrDateTime(value) {
  const date = value instanceof Date ? value : new Date(value);
  if (Number.isNaN(date.getTime())) {
    throw new ValidationError(`Data/hora inválida: ${value}`);
  }
  const dd = String(date.getDate()).padStart(2, '0');
  const mm = String(date.getMonth() + 1).padStart(2, '0');
  const yyyy = date.getFullYear();
  const hh = String(date.getHours()).padStart(2, '0');
  const mi = String(date.getMinutes()).padStart(2, '0');
  const ss = String(date.getSeconds()).padStart(2, '0');
  return `${dd}/${mm}/${yyyy} ${hh}:${mi}:${ss}`;
}

module.exports = { toBrDate, toBrDateTime };

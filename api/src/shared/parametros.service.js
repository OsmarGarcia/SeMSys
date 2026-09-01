'use strict';

/** Casas decimais configuradas para arredondamento de quantidades de estoque (PCCONSUM.NUMCASASDECESTOQUE). */
async function getCasasDecimaisEstoque(connection) {
  const result = await connection.execute('SELECT NVL(NUMCASASDECESTOQUE,1) AS CASAS FROM PCCONSUM');
  return Number(result.rows[0]?.CASAS ?? 1);
}

module.exports = { getCasasDecimaisEstoque };

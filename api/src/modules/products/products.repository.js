'use strict';

async function getProduto(connection, codProd) {
  const result = await connection.execute(
    `SELECT CODPROD AS CODPROD,
            DESCRICAO AS DESCRICAO,
            EMBALAGEM AS EMBALAGEM,
            QTUNITCX AS QTUNITCX,
            TIPOMERC AS TIPOMERC,
            NVL(ESTOQUEPORLOTE,'N') AS ESTOQUEPORLOTE,
            PREFIXOLOTE AS PREFIXOLOTE,
            DESCRICAO7 AS TIPOLOTE,
            NVL(DESCRICAO1,0) AS DESCRICAO1,
            NVL(DESCRICAO2,0) AS DESCRICAO2
       FROM PCPRODUT WHERE CODPROD = :codProd`,
    { codProd }
  );
  return result.rows[0] || null;
}

module.exports = { getProduto };

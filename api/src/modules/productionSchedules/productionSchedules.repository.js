'use strict';

/**
 * Acesso a dados do programa de produção por linha (tabela própria do SeMSys,
 * `SMPROGRAMAPRODUCAO`) — réplica das operações de `frmProgramarProducao.vb`
 * (SalvarPrograma / AlterarPrograma / btnCarregarPrograma_Click).
 *
 * Estratégia de persistência: em vez de reproduzir `AlterarPrograma`
 * (UPDATE por linha com muitos campos condicionais), toda escrita passa por
 * "substituir tudo" — apaga e reinsere os itens/materiais do programa — que é
 * exatamente o que `SalvarPrograma` já fazia a cada clique em "Salvar" no
 * VB.NET original. Isso simplifica a API sem mudar o resultado observável.
 */

async function existsPrograma(connection, codPrograma) {
  const result = await connection.execute(
    'SELECT COUNT(*) AS CONTAGEM FROM SMPROGRAMAPRODUCAO WHERE PROGRAMA = :codPrograma',
    { codPrograma }
  );
  return Number(result.rows[0].CONTAGEM) > 0;
}

/** Itens (produtos acabados) do programa, uma linha por OP planejada. */
async function getItens(connection, codPrograma) {
  const result = await connection.execute(
    `SELECT IDPROGRAMA, CODPROD, DESCRICAO, EMBALAGEM, METODO, QTPRODUZIR,
            HORAINICIAL, HORAFINAL, TEMPOTOTAL, NUMOP, NUMLOTE, QTUNITCX, LINHA
       FROM SMPROGRAMAPRODUCAO
      WHERE PROGRAMA = :codPrograma AND STATUS IS NULL AND TIPO IS NULL
      ORDER BY LINHA, HORAINICIAL`,
    { codPrograma }
  );
  return result.rows;
}

/** Materiais (semiacabados/matérias-primas) explodidos do programa (TIPO='MP'). */
async function getMateriais(connection, codPrograma) {
  const result = await connection.execute(
    `SELECT IDPROGRAMA, CODPROD, DESCRICAO, METODO, QTPRODUZIR, NUMOP, NUMLOTE, DTPREVINICIOSA
       FROM SMPROGRAMAPRODUCAO
      WHERE PROGRAMA = :codPrograma AND STATUS IS NULL AND TIPO IS NOT NULL
      ORDER BY IDPROGRAMA`,
    { codPrograma }
  );
  return result.rows;
}

async function deleteItens(connection, codPrograma) {
  await connection.execute('DELETE FROM SMPROGRAMAPRODUCAO WHERE PROGRAMA = :codPrograma AND TIPO IS NULL', {
    codPrograma
  });
}

async function deleteMateriais(connection, codPrograma) {
  await connection.execute('DELETE FROM SMPROGRAMAPRODUCAO WHERE PROGRAMA = :codPrograma AND TIPO IS NOT NULL', {
    codPrograma
  });
}

async function insertItem(connection, item) {
  await connection.execute(
    `INSERT INTO SMPROGRAMAPRODUCAO
       (CODPROD, DESCRICAO, QTPRODUZIR, NUMOP, NUMLOTE, HORAINICIAL, HORAFINAL, TEMPOTOTAL,
        PROGRAMA, METODO, QTUNITCX, LINHA, EMBALAGEM)
     VALUES
       (:codProd, :descricao, :qtProduzir, :numOp, :numLote,
        TO_DATE(:horaInicial,'DD/MM/YYYY HH24:MI:SS'), TO_DATE(:horaFinal,'DD/MM/YYYY HH24:MI:SS'),
        :tempoTotal, :programa, :metodo, :qtUnitCx, :linha, :embalagem)`,
    item
  );
}

async function insertMaterial(connection, material) {
  await connection.execute(
    `INSERT INTO SMPROGRAMAPRODUCAO
       (CODPROD, DESCRICAO, QTPRODUZIR, NUMOP, NUMLOTE, PROGRAMA, METODO, DTPREVINICIOSA, TIPO)
     VALUES
       (:codProd, :descricao, :qtProduzir, :numOp, :numLote, :programa, :metodo,
        TO_DATE(:dtPrevInicioSA,'DD/MM/YYYY HH24:MI:SS'), 'MP')`,
    material
  );
}

/** Dados do produto usados ao incluir na grade — réplica de `CapturarDadosProduto`. */
async function getProdutoParaProgramacao(connection, { codProd, linha }) {
  const result = await connection.execute(
    `SELECT NVL(DESCRICAO1,0) AS DESCRICAO1, NVL(DESCRICAO2,0) AS DESCRICAO2,
            QTUNITCX, EMBALAGEM, DESCRICAO, NVL(QTTOTPAL,0) AS QTTOTPAL
       FROM PCPRODUT WHERE CODPROD = :codProd`,
    { codProd }
  );
  const row = result.rows[0];
  if (!row) return null;
  return {
    velocidadeNominal: Number(linha === 'M30' ? row.DESCRICAO1 : row.DESCRICAO2),
    qtUnitCx: Number(row.QTUNITCX),
    embalagem: row.EMBALAGEM,
    descricao: row.DESCRICAO,
    qtTotPal: Number(row.QTTOTPAL)
  };
}

module.exports = {
  existsPrograma,
  getItens,
  getMateriais,
  deleteItens,
  deleteMateriais,
  insertItem,
  insertMaterial,
  getProdutoParaProgramacao
};

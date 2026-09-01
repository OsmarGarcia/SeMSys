'use strict';

/**
 * Acesso a dados de Ordens de Produção (PCOPC/PCOPI/PCOPILOTE/PCMOV/PEPROGOP...).
 * Nenhuma regra de negócio aqui além da forma da consulta — orquestração fica em
 * `productionOrders.service.js`.
 */

async function search(connection, { position, codFilial, startDateFrom, startDateTo }) {
  const binds = { position };
  const conditions = ['A.CODPRODMASTER = B.CODPROD', 'A.POSICAO = :position'];

  if (codFilial) {
    binds.codFilial = codFilial;
    conditions.push('A.CODFILIAL = :codFilial');
  }
  if (startDateFrom && startDateTo) {
    binds.startDateFrom = startDateFrom;
    binds.startDateTo = startDateTo;
    conditions.push("A.DTPREVINICIO BETWEEN TO_DATE(:startDateFrom,'DD/MM/YYYY') AND TO_DATE(:startDateTo,'DD/MM/YYYY')");
  }
  // Regra original de `PesquisarOPsIniciar`: só considera OPs ainda não totalmente produzidas.
  if (position === 'L') {
    conditions.push('NVL(A.QTPRODUZIDA,0) <= NVL(A.QTPRODUZIR,0)');
  }

  const result = await connection.execute(
    `SELECT A.NUMOP AS NUMOP,
            A.NUMLOTE AS NUMLOTE,
            A.CODPRODMASTER AS CODPROD,
            B.DESCRICAO AS DESCRICAO,
            A.QTPRODUZIR AS QTPRODUZIR,
            A.POSICAO AS POSICAO,
            A.DTPREVINICIO AS DTPREVINICIO
       FROM PCOPC A, PCPRODUT B
      WHERE ${conditions.join('\n        AND ')}
      ORDER BY A.NUMOP`,
    binds
  );
  return result.rows;
}

/** Cabeçalho da OP — réplica de `PesquisarCabecalhoOP`. */
async function getHeader(connection, numOp) {
  const result = await connection.execute(
    `SELECT TO_CHAR(A.CODPRODMASTER) AS CODPROD,
            B.DESCRICAO AS DESCRICAO,
            ROUND(A.QTPRODUZIR,2) AS QT,
            A.METODO AS METODO,
            A.POSICAO AS POSICAO,
            CASE WHEN (SELECT DISTINCT(MODOPREPARO) FROM PCCOMPOSICAO
                        WHERE CODPRODMASTER = A.CODPRODMASTER AND METODO = A.METODO AND CODFILIAL = A.CODFILIAL) IS NULL
                 THEN 'N/A'
                 ELSE (SELECT DISTINCT(MODOPREPARO) FROM PCCOMPOSICAO
                        WHERE CODPRODMASTER = A.CODPRODMASTER AND METODO = A.METODO AND CODFILIAL = A.CODFILIAL)
            END AS KIT
       FROM PCOPC A, PCPRODUT B
      WHERE A.CODPRODMASTER = B.CODPROD
        AND A.NUMOP = :numOp`,
    { numOp }
  );
  return result.rows[0] || null;
}

/**
 * Dados mínimos da OP usados em toda validação de estado (start/reprogram/cancel) —
 * réplica das diversas consultas `SELECT ... FROM PCOPC WHERE NUMOP = :NUMOP`
 * espalhadas por `frmProgramarProducao.vb`/`frmManutencaoOP.vb`.
 */
async function getForValidation(connection, numOp) {
  const result = await connection.execute(
    `SELECT NVL(A.QTPRODUZIR,0) AS QTPRODUZIR,
            A.POSICAO AS POSICAO,
            A.CODPRODMASTER AS CODPRODMASTER,
            A.DTPREVINICIO AS DTPREVINICIO,
            A.METODO AS METODO,
            (SELECT DESCRICAO7 FROM PCPRODUT WHERE PCPRODUT.CODPROD = A.CODPRODMASTER) AS TIPOLOTE
       FROM PCOPC A
      WHERE A.NUMOP = :numOp`,
    { numOp }
  );
  return result.rows[0] || null;
}

/** Itens/insumos necessários da OP — união PCOPI (sem lote) + PCOPILOTE (com lote). */
async function getItems(connection, { numOp, codFilialEstoque }) {
  const result = await connection.execute(
    `SELECT TO_CHAR(A.CODPROD) AS CODPROD,
            B.DESCRICAO AS DESCRICAO,
            '1' AS NUMLOTE,
            ROUND(A.QTNECESSIDADE,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS QTNECESSIDADE,
            ROUND(C.CUSTOREAL,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOREAL,
            ROUND(C.CUSTOFIN,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOFIN,
            ROUND(C.CUSTOCONT,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOCONT,
            ROUND(C.VALORULTENT,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS VALORULTENT,
            ROUND(C.CUSTOULTENT,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOULTENT,
            'N' AS ESTOQUEPORLOTE
       FROM PCOPI A, PCPRODUT B, PCEST C
      WHERE A.CODPROD = B.CODPROD
        AND A.CODPROD = C.CODPROD
        AND C.CODFILIAL = :codFilialEstoque
        AND NVL(B.ESTOQUEPORLOTE,'N') = 'N'
        AND A.NUMOP = :numOp
      UNION ALL
      SELECT TO_CHAR(A.CODPROD) AS CODPROD,
            B.DESCRICAO AS DESCRICAO,
            A.NUMLOTE AS NUMLOTE,
            ROUND(A.QT,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS QTNECESSIDADE,
            ROUND(C.CUSTOREAL,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOREAL,
            ROUND(C.CUSTOFIN,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOFIN,
            ROUND(C.CUSTOCONT,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOCONT,
            ROUND(C.VALORULTENT,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS VALORULTENT,
            ROUND(C.CUSTOULTENT,(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOULTENT,
            'S' AS ESTOQUEPORLOTE
       FROM PCOPILOTE A, PCPRODUT B, PCEST C
      WHERE A.CODPROD = B.CODPROD
        AND A.CODPROD = C.CODPROD
        AND C.CODFILIAL = :codFilialEstoque
        AND A.NUMOP = :numOp
        AND NVL(B.ESTOQUEPORLOTE,'N') = 'S'`,
    { numOp, codFilialEstoque }
  );
  return result.rows;
}

/**
 * Necessidade x estoque disponível por insumo da OP — réplica da consulta usada
 * tanto para colorir a grade de OPs aguardando início quanto na validação de
 * `IniciarOP`.
 */
async function getInsumosComEstoque(connection, numOp) {
  const result = await connection.execute(
    `SELECT A.CODPROD AS CODPROD,
            A.QTNECESSIDADE AS QTNECESSIDADE,
            (SELECT PKG_ESTOQUE.ESTOQUE_DISPONIVEL(A.CODPROD, B.CODFILIAL, 'V') FROM DUAL) AS ESTOQUE,
            (SELECT TIPOMERC FROM PCPRODUT WHERE PCPRODUT.CODPROD = A.CODPROD) AS TIPOMERC,
            (SELECT NVL(PCPRODUT.ESTOQUEPORLOTE,'N') FROM PCPRODUT WHERE CODPROD = A.CODPROD) AS ESTOQUEPORLOTE
       FROM PCOPI A, PCOPC B
      WHERE A.NUMOP = B.NUMOP
        AND A.NUMOP = :numOp`,
    { numOp }
  );
  return result.rows;
}

/**
 * Apontamentos/movimentos (PCMOV) de uma OP. Nota: a coluna DESCRICAO é
 * consultada diretamente de PCMOV, réplica literal de `PesquisarApontamentos`
 * (frmSeparacaoMaterial.vb) — confirmar com o DBA se essa coluna de fato
 * existe no schema, pois PCMOV normalmente não carrega descrição do produto.
 */
async function getMovements(connection, numOp) {
  const result = await connection.execute(
    `SELECT DTMOV AS DTMOV,
            NVL(NUMSEQ,0) AS SEQ_APONTAMENTO,
            CODPROD AS CODPROD,
            DESCRICAO AS DESCRICAO,
            QT AS QT,
            CODFILIAL AS CODFILIAL,
            CODOPER AS CODOPER,
            NUMTRANSVENDA AS NUMTRANSVENDA,
            (SELECT MATRICULA || ' - ' || NOME FROM PCEMPR WHERE MATRICULA = PCMOV.CODUSUR) AS FUNCIONARIO
       FROM PCMOV
      WHERE NUMOP = :numOp
      ORDER BY NUMTRANSVENDA`,
    { numOp }
  );
  return result.rows;
}

async function insertPeprogop(connection, { numOp, codFilial, codProd, metodo, qtProduzir, codFunc, numLote, dtPrevInicio }) {
  await connection.execute(
    `INSERT INTO PEPROGOP
       (NUMOP, CODFILIAL, CODPRODMASTER, METODO, NUMSEQ, QTPRODUZIR, DTLANC,
        CODFUNCLANC, POSICAO, ADEQUACAO, QTHORAS, NUMLOTE, DTPREVINICIO)
     VALUES
       (:numOp, :codFilial, :codProd, :metodo, '1', :qtProduzir, SYSDATE,
        :codFunc, 'L', NULL, 0, :numLote, TO_DATE(:dtPrevInicio,'DD/MM/YYYY HH24:MI:SS'))`,
    { numOp, codFilial, codProd, metodo, qtProduzir, codFunc, numLote, dtPrevInicio }
  );
}

async function insertPcopc(connection, { numOp, codFilial, codProd, metodo, qtProduzir, codFunc, numLote, dtPrevInicio }) {
  await connection.execute(
    `INSERT INTO PCOPC
       (NUMOP, NUMOPCENTRAL, CODFILIAL, CODPRODMASTER, METODO, QTPRODUZIR, DTLANC,
        CODFUNCLANC, POSICAO, NUMLOTE, QTORIGINAL, DTPREVINICIO, REPROCESSO)
     VALUES
       (:numOp, :numOp, :codFilial, :codProd, :metodo, :qtProduzir, SYSDATE,
        :codFunc, 'L', :numLote, :qtProduzir, TO_DATE(:dtPrevInicio,'DD/MM/YYYY HH24:MI:SS'), 'N')`,
    { numOp, codFilial, codProd, metodo, qtProduzir, codFunc, numLote, dtPrevInicio }
  );
}

async function insertPcobsop(connection, { numOp, codFunc, observacao = 'ORDEM DE PRODUCAO GERADA COM SUCESSO' }) {
  await connection.execute(
    `INSERT INTO PCOBSOP (NUMOP, OBS, ROTINALANC, CODFUNCLANC, DATALANC)
     VALUES (:numOp, :observacao, 'SEMSYS-API', :codFunc, SYSDATE)`,
    { numOp, observacao, codFunc }
  );
}

/** Grava um insumo explodido da fórmula nas três tabelas (PEPROGITENS/PCOPI/PCCOMPOSICAOFRACAO). */
async function insertFormulaItem(connection, { numOp, numSeq, codProd, codProdMaster, qtNecessidade, codFunc }) {
  await connection.execute(
    `INSERT INTO PEPROGITENS (NUMOP, CODPROD, NUMSEQ, QTNECESSIDADE, DTLANC, CODOPER, CODFUNCLANC)
     VALUES (:numOp, :codProd, :numSeq, :qtNecessidade, SYSDATE, 'SP', :codFunc)`,
    { numOp, codProd, numSeq, qtNecessidade, codFunc }
  );

  await connection.execute(
    `INSERT INTO PCOPI (NUMOP, CODPROD, QTNECESSIDADE, QTREQUISITADO, FRACAOUMIDA, ACEITAREQACIMAPREV)
     VALUES (:numOp, :codProd, :qtNecessidade, 0, 'A', 'N')`,
    { numOp, codProd, qtNecessidade }
  );

  await connection.execute(
    `INSERT INTO PCCOMPOSICAOFRACAO
       (NUMOP, CODPROD, CODPRODMASTER, QTNECESSIDADE, QTREQUISITADO, ACEITAREQACIMAPREV, NUMETAPA, FRACAOUMIDA)
     VALUES (:numOp, :codProd, :codProdMaster, :qtNecessidade, 0, 'N', 0, 'A')`,
    { numOp, codProd, codProdMaster, qtNecessidade }
  );
}

/** Apaga os itens gerados da fórmula (usado antes de recalcular com novo método/quantidade). */
async function deleteFormulaItems(connection, numOp) {
  await connection.execute('DELETE FROM PCOPI WHERE NUMOP = :numOp', { numOp });
  await connection.execute('DELETE FROM PCCOMPOSICAOFRACAO WHERE NUMOP = :numOp', { numOp });
}

async function updateMetodo(connection, { numOp, metodo }) {
  await connection.execute('UPDATE PCOPC SET METODO = :metodo WHERE NUMOP = :numOp', { metodo, numOp });
}

async function updatePosicaoCancelada(connection, numOp) {
  await connection.execute("UPDATE PCOPC SET POSICAO = 'C' WHERE NUMOP = :numOp", { numOp });
}

async function updateInicioProducao(connection, { numOp, matriculaUsuario }) {
  await connection.execute(
    `UPDATE PCOPC SET POSICAO = 'P', DTINICIO = TRUNC(SYSDATE), CODFUNCINICIO = :matriculaUsuario
      WHERE NUMOP = :numOp`,
    { matriculaUsuario, numOp }
  );
  await connection.execute("UPDATE PCOPI SET BAIXAVIRTUAL = 'N' WHERE NUMOP = :numOp", { numOp });
}

async function updateReservaPendente(connection, { numOp, codProd, qtNecessidade }) {
  await connection.execute(
    `UPDATE PCOPI SET QTRESERVALTERAR = :qtNecessidade
      WHERE NUMOP = :numOp AND CODPROD = :codProd AND NOT QTNECESSIDADE < 0`,
    { qtNecessidade, numOp, codProd }
  );
}

async function updateReservaLiberada(connection, numOp) {
  await connection.execute("UPDATE PCOPI SET RESERVALIBERADA = 'N' WHERE NUMOP = :numOp", { numOp });
}

/** Grava a alocação de um lote físico para a OP durante o início de produção. */
async function reservarLotePorItem(connection, { codProd, numLote, quantidadeAlocada, numOp, dtValidade }) {
  await connection.execute(
    'UPDATE PCLOTE SET QTTEMPINDUSTRIA = :quantidadeAlocada WHERE CODPROD = :codProd AND NUMLOTE = :numLote',
    { quantidadeAlocada, codProd, numLote }
  );

  await connection.execute(
    `INSERT INTO PCOPILOTE
       (CODPROD, NUMLOTEORI, NUMLOTE, QT, QTREQUISITADO, NUMOP, NUMSEQ, DTVALIDADE, FRACAOUMIDA)
     VALUES
       (:codProd, :numLote, :numLote, :quantidadeAlocada, 0, :numOp, '1', :dtValidade, 'A')`,
    { codProd, numLote, quantidadeAlocada, numOp, dtValidade }
  );

  await connection.execute(
    'UPDATE PCLOTE SET QTTEMPINDUSTRIA = 0 WHERE NUMLOTE = :numLote AND CODPROD = :codProd',
    { numLote, codProd }
  );
}

/**
 * Dados para impressão de etiqueta/COD128 — réplica de `ClassOrdemProducao.ImprimirOP`.
 * `offset` é somado ao número de lote atual quando o tipo de lote não é
 * JULIANO/TAMPICO (mesmo parâmetro `offset` recebido pela rotina original).
 */
async function getLabelData(connection, { numOp, offset }) {
  const result = await connection.execute(
    `SELECT
        NUMOP,
        NUMLOTE,
        '010'||LPAD(CODAUXILIAR,13,0)||'12'||DT_VAL||'11'||DT_FAB||'3100'||LPAD(QTTOTPAL,6,'0')||'10'||NUMLOTE AS COD128,
        CODPRODMASTER,
        DATA_VALIDADE,
        DESCRICAO,
        EMBALAGEM,
        QTTOTPAL,
        LASTROPAL,
        ALTURAPAL,
        PRAZOVAL,
        QTPRODUZIR
     FROM (
        SELECT
          A.CODPRODMASTER,
          TO_CHAR(SYSDATE) AS DATA_FABRICACAO,
          TO_CHAR(SYSDATE,'YYMMDD') AS DT_FAB,
          A.NUMOP,
          TO_CHAR(CASE WHEN B.DESCRICAO6 = 'MESES' THEN ADD_MONTHS(SYSDATE,B.PRAZOVAL)
                       ELSE SYSDATE + B.PRAZOVAL END,'YYMMDD') AS DT_VAL,
          TO_CHAR(CASE WHEN B.DESCRICAO6 = 'MESES' THEN ADD_MONTHS(SYSDATE,B.PRAZOVAL)
                       ELSE SYSDATE + B.PRAZOVAL END) AS DATA_VALIDADE,
          A.DTFECHA,
          A.QTPRODUZIR,
          B.DESCRICAO,
          B.EMBALAGEM,
          B.QTTOTPAL,
          B.LASTROPAL,
          B.ALTURAPAL,
          B.PRAZOVAL,
          B.CODAUXILIAR,
          CASE WHEN B.DESCRICAO7 = 'JULIANO' THEN
                 (TO_CHAR(MOD(TO_NUMBER(TO_CHAR(SYSDATE,'YYYY')),10)) || TO_CHAR(SYSDATE,'DDD'))
               WHEN B.DESCRICAO7 = 'TAMPICO' THEN
                 '318-' || (SELECT TO_CHAR(MOD(TO_NUMBER(TO_CHAR(SYSDATE,'YYYY')),10)) || TO_CHAR(SYSDATE,'DDD') FROM DUAL)
               ELSE
                 TO_CHAR(NVL(A.NUMLOTE,0) + :offset)
          END AS NUMLOTE
        FROM PCOPC A
        INNER JOIN PCPRODUT B ON A.CODPRODMASTER = B.CODPROD
        WHERE A.CODFILIAL = '1'
          AND A.NUMOP = :numOp
     )`,
    { numOp, offset }
  );
  return result.rows[0] || null;
}

module.exports = {
  search,
  getHeader,
  getLabelData,
  getForValidation,
  getItems,
  getInsumosComEstoque,
  getMovements,
  insertPeprogop,
  insertPcopc,
  insertPcobsop,
  insertFormulaItem,
  deleteFormulaItems,
  updateMetodo,
  updatePosicaoCancelada,
  updateInicioProducao,
  updateReservaPendente,
  updateReservaLiberada,
  reservarLotePorItem
};

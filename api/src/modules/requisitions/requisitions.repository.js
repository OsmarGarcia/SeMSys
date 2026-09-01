'use strict';

/**
 * Acesso a dados da requisição/separação de materiais de uma OP — réplica das
 * consultas de `RequisitarInsumos` e `PesquisarItensOP` (frmSeparacaoMaterial.vb).
 */

/** Disponibilidade Winthor x WMS por insumo da OP (antes de escalar pela quantidade solicitada). */
async function getDisponibilidade(connection, { numOp, codFilialEstoque }) {
  const result = await connection.execute(
    `SELECT A.CODPROD AS CODPROD,
            C.DESCRICAO AS DESCRICAO,
            C.USAWMS AS USAWMS,
            NVL(ROUND(A.QTNECESSIDADE,3),0) AS QTNECESSIDADE,
            NVL(ROUND(A.QTREQUISITADO,3),0) AS QTREQUISITADO,
            NVL(ROUND(A.QTRESERVATUAL,3),0) AS QTRESERVATUAL,
            NVL(ROUND(B.QTESTGER - B.QTBLOQUEADA,3),0) AS QT_DISP_WINTHOR,
            NVL((SELECT ROUND(SUM(NVL(QT,0)) - SUM(NVL(QTPENDSAIDA,0)),3)
                   FROM PCESTENDERECO WHERE PCESTENDERECO.CODPROD = A.CODPROD),0) AS QT_DISP_WMS,
            NVL(B.CUSTOREAL,0) AS CUSTOREAL,
            NVL(B.CUSTOCONT,0) AS CUSTOCONT,
            NVL(B.CUSTOFIN,0) AS CUSTOFIN,
            NVL(B.VALORULTENT,0) AS VALORULTENT,
            NVL(B.CUSTOULTENT,0) AS CUSTOULTENT
       FROM PCOPI A, PCEST B, PCPRODUT C
      WHERE A.CODPROD = B.CODPROD
        AND A.CODPROD = C.CODPROD
        AND B.CODFILIAL = :codFilialEstoque
        AND A.NUMOP = :numOp`,
    { numOp, codFilialEstoque }
  );
  return result.rows;
}

/** Itens da OP a requisitar, com custos, ordenados para tratar primeiro os controlados por lote. */
async function getItensParaRequisitar(connection, { numOp, codFilialEstoque }) {
  const result = await connection.execute(
    `SELECT TO_CHAR(A.CODPROD) AS CODPROD,
            B.DESCRICAO AS DESCRICAO,
            ROUND(NVL(A.QTNECESSIDADE,0),(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS QTNECESSIDADE,
            ROUND(NVL(C.CUSTOREAL,0),(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOREAL,
            ROUND(NVL(C.CUSTOFIN,0),(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOFIN,
            ROUND(NVL(C.CUSTOCONT,0),(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOCONT,
            ROUND(NVL(C.VALORULTENT,0),(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS VALORULTENT,
            ROUND(NVL(C.CUSTOULTENT,0),(SELECT NVL(NUMCASASDECESTOQUE,1) FROM PCCONSUM)) AS CUSTOULTENT,
            NVL(B.ESTOQUEPORLOTE,'N') AS ESTOQUEPORLOTE
       FROM PCOPI A, PCPRODUT B, PCEST C
      WHERE A.CODPROD = B.CODPROD
        AND A.CODPROD = C.CODPROD
        AND C.CODFILIAL = :codFilialEstoque
        AND A.NUMOP = :numOp
      ORDER BY NVL(B.ESTOQUEPORLOTE,'N')`,
    { numOp, codFilialEstoque }
  );
  return result.rows;
}

/** Lotes já reservados para a OP (PCOPILOTE) com saldo ainda não requisitado, em ordem FEFO. */
async function getLotesParaRequisitar(connection, { numOp, codProd, codFilialEstoque }) {
  const result = await connection.execute(
    `SELECT TO_CHAR(A.CODPROD) AS CODPROD,
            B.DESCRICAO AS DESCRICAO,
            NVL(A.NUMLOTE,'1') AS NUMLOTE,
            ROUND(A.QT,3) AS QTNECESSIDADE,
            ROUND(A.QTREQUISITADO,3) AS QTREQUISITADO,
            NVL(ROUND(C.QTESTGER - C.QTBLOQUEADA - C.QTRESERV,3),0) AS QT_DISP_WINTHOR,
            NVL((SELECT ROUND(SUM(NVL(QT,0)) - SUM(NVL(QTPENDSAIDA,0)),3)
                   FROM PCESTENDERECO WHERE PCESTENDERECO.CODPROD = A.CODPROD),0) AS QT_DISP_WMS,
            ROUND(C.CUSTOREAL,3) AS CUSTOREAL,
            ROUND(C.CUSTOFIN,3) AS CUSTOFIN,
            ROUND(C.CUSTOCONT,3) AS CUSTOCONT,
            ROUND(C.VALORULTENT,3) AS VALORULTENT,
            ROUND(C.CUSTOULTENT,3) AS CUSTOULTENT,
            (SELECT NVL(DTVALIDADE, TO_DATE('01/01/1900','DD/MM/YYYY')) FROM PCLOTE
              WHERE PCLOTE.CODFILIAL = :codFilialEstoque AND CODPROD = A.CODPROD AND PCLOTE.NUMLOTE = A.NUMLOTE) AS DTVALIDADE
       FROM PCOPILOTE A, PCPRODUT B, PCEST C
      WHERE A.CODPROD = B.CODPROD
        AND A.CODPROD = C.CODPROD
        AND C.CODFILIAL = :codFilialEstoque
        AND A.NUMOP = :numOp
        AND NVL(B.ESTOQUEPORLOTE,'N') = 'S'
        AND A.CODPROD = :codProd
        AND QT > QTREQUISITADO
      ORDER BY (SELECT NVL(DTVALIDADE, TO_DATE('01/01/1900','DD/MM/YYYY')) FROM PCLOTE
                  WHERE PCLOTE.CODFILIAL = :codFilialEstoque AND CODPROD = A.CODPROD AND PCLOTE.NUMLOTE = A.NUMLOTE) ASC,
               QTREQUISITADO DESC`,
    { numOp, codProd, codFilialEstoque }
  );
  return result.rows;
}

async function insertPcmov(connection, values) {
  await connection.execute(
    `INSERT INTO PCMOV
       (DTMOV, CODPROD, CODOPER, QT, PUNIT, CUSTOREAL, CUSTOFIN, CUSTOCONT, VALORULTENT,
        CUSTOULTENT, CODFILIAL, STATUS, NUMLOTE, NUMOP, CODFUNCLANC, CODFUNCREQ,
        NUMTRANSVENDA, CODUSUR, NUMTRANSITEM, NUMPED, NUMSEQ, NUMCAR)
     VALUES
       (TRUNC(SYSDATE), :codProd, 'SP', :qt, :custoReal, :custoReal, :custoFin, :custoCont, :valorUltEnt,
        :custoUltEnt, :codFilial, 'AB', :numLote, :numOp, :usuario, :usuario,
        :numTransVenda, :usuario, :numTransItem, :numOp, :numSeq, :numOp)`,
    {
      codProd: values.codProd,
      qt: values.qt,
      custoReal: values.custoReal,
      custoFin: values.custoFin,
      custoCont: values.custoCont,
      valorUltEnt: values.valorUltEnt,
      custoUltEnt: values.custoUltEnt,
      codFilial: values.codFilial,
      numLote: values.numLote,
      numOp: values.numOp,
      usuario: values.usuario,
      numTransVenda: values.numTransVenda,
      numTransItem: values.numTransItem,
      numSeq: values.numSeq
    }
  );
}

async function insertPcmovcomple(connection, numTransItem) {
  await connection.execute(
    `INSERT INTO PCMOVCOMPLE (NUMTRANSITEM, DTREGISTRO, CODAGREGACAO) VALUES (:numTransItem, SYSDATE, '0')`,
    { numTransItem }
  );
}

async function updatePcopiloteRequisitado(connection, { numOp, numLote, codProd, qt }) {
  await connection.execute(
    `UPDATE PCOPILOTE SET QTREQUISITADO = NVL(QTREQUISITADO,0) + :qt
      WHERE NUMOP = :numOp AND NUMLOTE = :numLote AND FRACAOUMIDA = 'A' AND CODPROD = :codProd`,
    { qt, numOp, numLote, codProd }
  );
}

/**
 * Movimentos gerados pela transação de venda/requisição, já com a baixa
 * efetivada por `PKG_ESTOQUE.VENDAS_SAIDA`. Nota de fidelidade: o VB.NET
 * original também seleciona uma coluna DESCRICAO de PCMOV aqui, mas nunca a
 * lê depois — omitida por não existir de fato em PCMOV e não ser usada.
 */
async function getMovimentosPorTransacao(connection, numTransVenda) {
  const result = await connection.execute(
    `SELECT CODPROD, QT, NUMLOTE, CODFILIAL, NUMOP, NUMTRANSVENDA
       FROM PCMOV WHERE NUMTRANSVENDA = :numTransVenda`,
    { numTransVenda }
  );
  return result.rows;
}

async function updateGiroEstoque(connection, { codProd, codFilial, qt }) {
  await connection.execute(
    `UPDATE PCEST
        SET QTVENDMES = QTVENDMES + :qt,
            QTVENDDIA = QTVENDDIA + :qt,
            QTVENDSEMANA = QTVENDSEMANA + :qt,
            DTULTSAIDA = TRUNC(SYSDATE)
      WHERE CODPROD = :codProd AND CODFILIAL = :codFilial`,
    { qt, codProd, codFilial }
  );
}

async function updatePcopiPosRequisicao(connection, { codProd, numOp, qt }) {
  await connection.execute(
    `UPDATE PCOPI
        SET QTRESERVATUAL = NVL(QTRESERVATUAL,0) - :qt,
            QTREQUISITADO = NVL(QTREQUISITADO,0) + :qt
      WHERE CODPROD = :codProd AND NUMOP = :numOp`,
    { qt, codProd, numOp }
  );
}

module.exports = {
  getDisponibilidade,
  getItensParaRequisitar,
  getLotesParaRequisitar,
  insertPcmov,
  insertPcmovcomple,
  updatePcopiloteRequisitado,
  getMovimentosPorTransacao,
  updateGiroEstoque,
  updatePcopiPosRequisicao
};

'use strict';

/**
 * Relatório de produção total no Winthor — réplica de
 * `ClassProducaoTotalWinthor.ConsultarProducaoTotal`.
 *
 * Nota de fidelidade: a consulta original usa o alias `DTMOV` duas vezes
 * (uma para `M.DTMOV`, outra para `M.DTMOVLOG`) — inofensivo em VB.NET
 * porque o DataTable indexa por posição/nome e mantém as duas colunas
 * fisicamente distintas, mas fatal ao virar JSON (a segunda chave
 * sobrescreveria a primeira). Renomeada aqui para `DTMOVLOG`.
 */
async function getProducaoTotal(connection, { dtInicio, dtFim, filiais, departamentos }) {
  const result = await connection.execute(
    `SELECT
        TO_CHAR(M.DTMOV,'DD/MM/YYYY') AS DTMOV,
        TO_CHAR(M.CODPROD) AS CODPROD,
        TO_CHAR(S.CODSEC) AS CODSEC,
        S.DESCRICAO AS SECAO,
        TO_CHAR(D.CODEPTO) AS CODEPTO,
        D.DESCRICAO AS DEPTO,
        M.CODFILIAL AS CODFILIAL,
        P.DESCRICAO AS DESCRICAO,
        P.UNIDADE AS UNIDADE,
        P.EMBALAGEM AS EMBALAGEM,
        P.DESCRICAO1 AS NOMINAL,
        O.QTPRODUZIR AS QTPROGRAMADA,
        TO_CHAR(M.DTMOVLOG,'DD/MM/YYYY') AS DTMOVLOG,
        TO_CHAR(M.CODFUNCREQ) AS CODFUNCREQ,
        (SELECT PCEMPR.NOME FROM PCEMPR WHERE PCEMPR.MATRICULA = M.CODFUNCREQ) AS NOME,
        M.QT AS QT,
        P.QTUNITCX AS QTUNITCX,
        M.PUNIT AS PUNIT,
        O.NUMOP AS NUMOP,
        M.NUMLOTE AS NUMLOTE,
        M.CODOPER AS CODOPER,
        CASE
          WHEN M.CODOPER = 'SP' THEN 'ESTORNO DE APONTAMENTO'
          WHEN M.CODOPER = 'EP' THEN 'APONTAMENTO DE PRODUÇÃO'
          WHEN M.QT < 0 THEN 'CANCELAMENTO DE PRODUÇÃO'
        END AS OPERACAO
     FROM PCMOV M, PCPRODUT P, PCOPC O, PCSECAO S, PCDEPTO D
    WHERE P.CODPROD = M.CODPROD
      AND M.CODPROD = O.CODPRODMASTER
      AND M.CODFILIAL = O.CODFILIAL
      AND P.CODSEC = S.CODSEC
      AND P.CODEPTO = D.CODEPTO
      AND M.NUMOP = O.NUMOP
      AND M.CODFILIAL IN (${filiais.map((_, i) => `:filial${i}`).join(',')})
      AND M.DTMOV BETWEEN TO_DATE(:dtInicio,'DD/MM/YYYY') AND TO_DATE(:dtFim,'DD/MM/YYYY')
      AND D.CODEPTO IN (${departamentos.map((_, i) => `:depto${i}`).join(',')})
      AND M.CODOPER IN ('EP','SP')
    ORDER BY S.DESCRICAO, M.NUMLOTE, M.DTMOVLOG`,
    {
      dtInicio,
      dtFim,
      ...Object.fromEntries(filiais.map((f, i) => [`filial${i}`, f])),
      ...Object.fromEntries(departamentos.map((d, i) => [`depto${i}`, d]))
    }
  );
  return result.rows;
}

module.exports = { getProducaoTotal };

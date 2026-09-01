'use strict';

const { toBrDate } = require('../utils/dates');

/**
 * Geração de numeração sequencial (NUMOP, NUMLOTE, NUMTRANSVENDA, NUMSEQ,
 * NUMTRANSITEM, código de programa) — réplica das rotinas espalhadas em
 * `frmProgramarProducao.vb` (GerarProgramacao / ReprogramarOPWinthor) e
 * `frmSeparacaoMaterial.vb` (RequisitarInsumos).
 *
 * IMPORTANTE — concorrência: o VB.NET original lê o "próximo número" com um
 * SELECT e grava o incremento com um UPDATE separado, sem nenhum lock — seguro
 * apenas porque a aplicação desktop era operada por uma pessoa de cada vez.
 * Numa API isso é uma corrida (duas requisições concorrentes podem calcular o
 * mesmo número). Por isso toda função aqui deve ser chamada dentro de uma
 * transação que já tenha travado a linha de parâmetros relevante — ver
 * `lockCounters()` abaixo, chamada no início de cada caso de uso que gera
 * numeração (ver docs/oracle-integration-rest-api-plan.md, seção 6, item 2).
 */

/**
 * Trava a linha de parâmetros globais (PCCONSUM é uma tabela de instância
 * única) pelo tempo da transação corrente, serializando qualquer geração de
 * NUMOP/NUMLOTE/NUMTRANSVENDA concorrente.
 */
async function lockGlobalCounters(connection) {
  await connection.execute('SELECT 1 FROM PCCONSUM FOR UPDATE');
}

/**
 * Próximo número de Ordem de Produção.
 *
 * Regra original: o maior entre o próximo NUMOP livre em PCOPC e o próximo
 * NUMOP livre em PEPROGOP (as duas tabelas recebem o mesmo número).
 *
 * Nota de fidelidade: o VB.NET original grava o incremento com
 * `UPDATE PEPARAMETROS SET PROXNUMPROG = ...` e
 * `UPDATE PCCONSUM SET PROXNUMOP = ...` SEM cláusula WHERE — ou seja,
 * atualiza todas as linhas dessas tabelas (PEPARAMETROS tem uma linha por
 * filial). Mantido aqui por fidelidade ao comportamento observado; vale
 * confirmar com o time de dados/DBA se isso é intencional antes de operar
 * várias filiais simultaneamente em produção.
 */
async function nextNumOp(connection) {
  await lockGlobalCounters(connection);

  const result = await connection.execute(`
    SELECT CASE WHEN PROXNUMOP > PROXNUMPROG THEN PROXNUMOP
                WHEN PROXNUMOP <= PROXNUMPROG THEN PROXNUMPROG
           END AS PROXNUMPROG
    FROM (
      SELECT NVL((SELECT MAX(NUMOP) FROM PEPROGOP), 1) + 1 AS PROXNUMPROG,
             NVL((SELECT MAX(NUMOP) FROM PCOPC), 1) + 1 AS PROXNUMOP
      FROM DUAL
    )
  `);

  const numOp = String(result.rows[0].PROXNUMPROG);
  const next = Number(numOp) + 1;

  await connection.execute('UPDATE PEPARAMETROS SET PROXNUMPROG = :next', { next });
  await connection.execute('UPDATE PCCONSUM SET PROXNUMOP = :next', { next });

  return numOp;
}

/**
 * Próximo número de lote para um produto, respeitando o parâmetro
 * PEPARAMETROS.SEQUENCIALOTE ('P' = sequência por produto, outro valor =
 * sequência por filial via FNC_PROXNUMLOTE). Réplica de `GerarProgramacao`.
 *
 * @returns {{ numLote: string, tipoLote: string|null }} tipoLote só é
 *   preenchido no modo "por filial" — usado para detectar os tipos especiais
 *   JULIANO/TAMPICO (lote calculado por data, não por contador).
 */
async function nextNumLote(connection, { codProd, codFilial, dtPrevInicio }) {
  const paramResult = await connection.execute(
    `SELECT NVL(SEQUENCIALOTE,'P') AS SEQUENCIALOTE, NVL(TRAVARLOTE,'N') AS TRAVARLOTE
       FROM PEPARAMETROS WHERE CODFILIAL = :codFilial`,
    { codFilial }
  );
  const sequenciaLote = paramResult.rows[0]?.SEQUENCIALOTE ?? 'P';

  if (sequenciaLote === 'P') {
    await lockGlobalCounters(connection);
    const produtoResult = await connection.execute(
      `SELECT (PREFIXOLOTE || NVL(PROXNUMLOTE,1)) AS PROXNUMLOTE
         FROM PCPRODUT WHERE CODPROD = :codProd`,
      { codProd }
    );
    return { numLote: String(produtoResult.rows[0].PROXNUMLOTE), tipoLote: null };
  }

  const tipoLoteResult = await connection.execute(
    'SELECT DESCRICAO7 AS TIPO_LOTE FROM PCPRODUT WHERE CODPROD = :codProd',
    { codProd }
  );
  const tipoLote = tipoLoteResult.rows[0]?.TIPO_LOTE ?? null;

  await lockGlobalCounters(connection);
  const loteResult = await connection.execute(
    `SELECT FNC_PROXNUMLOTE(:codProd, TO_DATE(:dtPrevInicio,'DD/MM/YYYY')) AS PROXNUMLOTE FROM DUAL`,
    { codProd, dtPrevInicio: toBrDate(dtPrevInicio) }
  );
  const numLote = String(loteResult.rows[0].PROXNUMLOTE);

  // Lotes JULIANO/TAMPICO são calculados a partir da data (dia juliano do
  // ano), não consomem contador — replica a condição original.
  if (tipoLote !== 'JULIANO' && tipoLote !== 'TAMPICO') {
    const next = Number(numLote) + 1;
    await connection.execute('UPDATE PCCONSUM SET PROXNUMLOTE = :next', { next });
  }

  return { numLote, tipoLote };
}

/** Próximo número de transação de venda/movimentação (PCCONSUM.PROXNUMTRANSVENDA). */
async function nextNumTransVenda(connection) {
  await lockGlobalCounters(connection);

  const result = await connection.execute(
    'SELECT NVL(PROXNUMTRANSVENDA,1) AS PROXNUMTRANSVENDA FROM PCCONSUM'
  );
  const numTransVenda = Number(result.rows[0].PROXNUMTRANSVENDA);

  await connection.execute('UPDATE PCCONSUM SET PROXNUMTRANSVENDA = NVL(PROXNUMTRANSVENDA,1) + 1');

  return numTransVenda;
}

/** Próximo NUMSEQ de PCMOV para uma OP específica. */
async function nextNumSeqForOp(connection, numOp) {
  const result = await connection.execute(
    'SELECT MAX(NVL(NUMSEQ,1)) + 1 AS NUMSEQ FROM PCMOV WHERE NUMOP = :numOp',
    { numOp }
  );
  return result.rows[0].NUMSEQ ? Number(result.rows[0].NUMSEQ) : 1;
}

/** Próximo NUMTRANSITEM (sequence DFSEQ_PCMOVCOMPLE). */
async function nextNumTransItem(connection) {
  const result = await connection.execute(
    'SELECT DFSEQ_PCMOVCOMPLE.NEXTVAL AS NUMTRANSITEM FROM DUAL'
  );
  return Number(result.rows[0].NUMTRANSITEM);
}

/** Próximo código de programa de produção (tabela própria do SeMSys). */
async function nextCodPrograma(connection) {
  const result = await connection.execute(
    'SELECT DFSEQ_NOVO_SMPROGRAMAPRODUCAO.NEXTVAL AS PROXIMO FROM DUAL'
  );
  return String(result.rows[0].PROXIMO);
}

module.exports = {
  lockGlobalCounters,
  nextNumOp,
  nextNumLote,
  nextNumTransVenda,
  nextNumSeqForOp,
  nextNumTransItem,
  nextCodPrograma
};

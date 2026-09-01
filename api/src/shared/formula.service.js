'use strict';

const { round } = require('../utils/rounding');

/**
 * Explosão de fórmula (ficha técnica / BOM) de um produto acabado ou
 * semiacabado — réplica de `BuscarFormula`, presente (com pequenas variações
 * de arredondamento) em `frmManutencaoOP.vb` e `frmProgramarProducao.vb`.
 *
 * Cada linha retornada é um insumo direto da fórmula, com a quantidade já
 * multiplicada pela quantidade a produzir solicitada.
 *
 * Fidelidade ao original: o código-fonte tem 3 variações do mesmo cálculo,
 * não intercambiáveis bit-a-bit (arredondar a quantidade unitária dentro do
 * SQL muda o resultado depois de multiplicar pela quantidade desejada):
 *   - `frmManutencaoOP.BuscarFormula`      → unitDecimals=6, decimals=3
 *   - `frmProgramarProducao.BuscarFormula` → unitDecimals=3, decimals=3 (usado em GerarProgramacao)
 *   - `frmProgramarProducao.FormularAcabados` (MRP multinível) → unitDecimals=null (sem round no SQL), decimals=6
 * Cada serviço chamador deve escolher explicitamente a variante equivalente à
 * tela que está substituindo — os defaults abaixo replicam o caso mais comum
 * (MRP multinível).
 *
 * @param {number|null} [unitDecimals=null] Casas decimais aplicadas a
 *   `SUM(A.QT)` ainda dentro do SQL, antes de multiplicar pela quantidade.
 *   `null` = sem arredondamento nessa etapa (variante FormularAcabados).
 * @param {number} [decimals=6] Casas decimais do arredondamento final
 *   (depois de multiplicar pela quantidade a produzir).
 * @param {string} codFilialProducao Filial usada em `PCCOMPOSICAO.CODFILIAL`
 *   (a composição/ficha técnica é cadastrada por filial de produção).
 * @param {string} [codFilialEstoque] Filial usada em `PCEST.CODFILIAL` (saldo
 *   disponível do insumo). Nas variantes `BuscarFormula` (frmManutencaoOP e
 *   frmProgramarProducao) o VB.NET original usa a MESMA filial para as duas
 *   tabelas — por isso este parâmetro é opcional e cai para
 *   `codFilialProducao` quando omitido. Só `FormularAcabados`
 *   (explosão de MRP multinível) de fato usa duas filiais distintas.
 */
async function explodeFormula(connection, {
  codProdMaster,
  metodo,
  codFilial, // atalho: usa a mesma filial nas duas tabelas (variantes BuscarFormula)
  codFilialProducao = codFilial,
  codFilialEstoque = codFilial,
  qty,
  unitDecimals = null,
  decimals = 6
}) {
  const sumExpression =
    unitDecimals === null
      ? 'SUM(A.QT)'
      : `ROUND(SUM(A.QT), ${Number.parseInt(unitDecimals, 10)})`;

  const result = await connection.execute(
    `SELECT A.CODPROD AS CODPROD,
            B.DESCRICAO AS DESCRICAO,
            A.METODO AS METODO,
            ${sumExpression} AS QTUNITARIA,
            SUM(C.QTESTGER - C.QTBLOQUEADA - C.QTRESERV) AS ESTOQUEDISP,
            B.TIPOMERC AS TIPOMERC
       FROM PCCOMPOSICAO A, PCPRODUT B, PCEST C
      WHERE A.CODPROD = B.CODPROD
        AND A.CODPROD = C.CODPROD
        AND A.CODPRODMASTER = :codProdMaster
        AND A.METODO = :metodo
        AND C.CODFILIAL = :codFilialEstoque
        AND A.CODFILIAL = :codFilialProducao
      GROUP BY A.CODPROD, B.DESCRICAO, A.METODO, B.TIPOMERC`,
    { codProdMaster, metodo, codFilialEstoque, codFilialProducao }
  );

  return result.rows.map((row) => ({
    codProd: String(row.CODPROD).toUpperCase(),
    descricao: (row.DESCRICAO || '').toUpperCase(),
    metodo: row.METODO,
    qtNecessidade: round(Number(row.QTUNITARIA) * qty, decimals),
    estoqueDisponivel: Number(row.ESTOQUEDISP) || 0,
    // SA = semiacabado (produzido internamente); demais tipos são insumos comprados.
    tipoMerc: row.TIPOMERC,
    isSemiAcabado: row.TIPOMERC === 'SA'
  }));
}

/**
 * Métodos de fórmula distintos cadastrados para um produto acabado/semiacabado
 * numa filial — réplica de `DefinirMetodoMRP`/consulta usada em
 * `CapturarDadosProduto`.
 */
async function getMetodosDisponiveis(connection, { codProd, codFilial }) {
  const result = await connection.execute(
    `SELECT DISTINCT METODO FROM PCCOMPOSICAO
      WHERE CODPRODMASTER = :codProd AND CODFILIAL = :codFilial
      ORDER BY METODO`,
    { codProd, codFilial }
  );
  return result.rows.map((row) => row.METODO);
}

module.exports = { explodeFormula, getMetodosDisponiveis };

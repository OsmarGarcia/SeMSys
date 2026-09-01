'use strict';

const oracledb = require('oracledb');
const { BusinessError } = require('../utils/errors');

/**
 * Wrappers para as rotinas PL/SQL nativas do Winthor que já concentram regra
 * de negócio de estoque — a API não reimplementa essa lógica, apenas chama as
 * mesmas packages que o VB.NET chamava (`ModuloFuncoes.vb`,
 * `frmManutencaoOP.vb`, `frmSeparacaoMaterial.vb`).
 */

/** Estoque "disponível para venda" de um produto/filial (PKG_ESTOQUE.ESTOQUE_DISPONIVEL). */
async function getEstoqueDisponivel(connection, { codProd, codFilial }) {
  const result = await connection.execute(
    `BEGIN :ret := PKG_ESTOQUE.ESTOQUE_DISPONIVEL(:codProd, :codFilial, 'V'); END;`,
    {
      ret: { dir: oracledb.BIND_OUT, type: oracledb.NUMBER },
      codProd,
      codFilial
    }
  );
  return Number(result.outBinds.ret) || 0;
}

/**
 * Lotes com saldo disponível para um produto/filial, em ordem FEFO
 * (primeiro a vencer, primeiro a sair) — mesma consulta usada em `IniciarOP`
 * para escolher os lotes a reservar.
 */
async function getLotesDisponiveis(connection, { codProd, codFilial }) {
  const result = await connection.execute(
    `SELECT PCLOTE.CODPROD AS CODPROD,
            PCPRODUT.DESCRICAO AS DESCRICAO,
            ROUND(NVL(PCLOTE.QT,0) - NVL(PCLOTE.QTBLOQUEADA,0) - NVL(PCLOTE.QTRESERV,0)
                  - NVL(PCLOTE.QTTEMPINDUSTRIA,0), 6) AS QTDISPONIVEL,
            NVL(PCLOTE.QTTEMPINDUSTRIA,0) AS QTTEMPINDUSTRIA,
            PCLOTE.DTVALIDADE AS DTVALIDADE,
            PCLOTE.NUMLOTE AS NUMLOTE
       FROM PCLOTE, PCPRODUT
      WHERE PCLOTE.CODPROD = PCPRODUT.CODPROD
        AND PCLOTE.CODFILIAL = :codFilial
        AND PCLOTE.CODPROD = :codProd
        AND PCLOTE.DTEXCLUSAO IS NULL
        AND (NVL(PCLOTE.QT,0) - NVL(PCLOTE.QTBLOQUEADA,0) - NVL(PCLOTE.QTRESERV,0)) > 0
        AND (NVL(PCLOTE.QT,0) - NVL(PCLOTE.QTBLOQUEADA,0) - NVL(PCLOTE.QTRESERV,0))
            > NVL(PCLOTE.QTTEMPINDUSTRIA,0)
      ORDER BY DTVALIDADE ASC`,
    { codProd, codFilial }
  );
  return result.rows;
}

/**
 * Cria a reserva de estoque de um insumo para uma OP (PKG_ESTOQUE.RESERVA_INCLUIR,
 * operação 'II' = inclusão individual, mesmo parâmetro usado em `IniciarOP`).
 * Lança BusinessError se o retorno do Oracle não for 'OK'.
 */
async function reservaIncluir(connection, { numOp, codProd, numSeq = '1' }) {
  const result = await connection.execute(
    `DECLARE
       vRETORNO VARCHAR2(1);
     BEGIN
       vRETORNO := PKG_ESTOQUE.RESERVA_INCLUIR(
         :numOp,
         :codProd,
         :numSeq,
         SYS.DIUTIL.INT_TO_BOOL(0),
         'II',
         psMSG_RETORNO => :msgRetorno
       );
       :retorno := vRETORNO;
     END;`,
    {
      numOp: String(numOp),
      codProd: String(codProd),
      numSeq: String(numSeq),
      msgRetorno: { dir: oracledb.BIND_INOUT, type: oracledb.STRING, maxSize: 32767, val: '' },
      retorno: { dir: oracledb.BIND_OUT, type: oracledb.STRING, maxSize: 1 }
    }
  );

  const mensagem = result.outBinds.msgRetorno;
  if (mensagem !== 'OK') {
    throw new BusinessError(
      `Erro ao gerar reserva de estoque para o produto ${codProd}: ${mensagem}`,
      'STOCK_RESERVATION_FAILED',
      { codProd, numOp, mensagem }
    );
  }
  return result.outBinds.retorno;
}

/**
 * Efetiva a baixa de estoque de uma transação de movimentação já gravada em
 * PCMOV (PKG_ESTOQUE.VENDAS_SAIDA — mesmo motor genérico usado para vendas,
 * chamado em `RequisitarInsumos` para dar baixa nos insumos requisitados).
 */
async function vendasSaida(connection, { numTransVenda }) {
  const result = await connection.execute(
    `BEGIN :retorno := PKG_ESTOQUE.VENDAS_SAIDA(:numTransVenda, 'N', :msgRetorno); END;`,
    {
      numTransVenda,
      retorno: { dir: oracledb.BIND_OUT, type: oracledb.NUMBER },
      msgRetorno: { dir: oracledb.BIND_OUT, type: oracledb.STRING, maxSize: 1000 }
    }
  );

  const retorno = Number(result.outBinds.retorno);
  const mensagem = result.outBinds.msgRetorno || 'Sem mensagem de retorno';

  if (!retorno || retorno <= 0 || mensagem !== 'OK') {
    throw new BusinessError(
      `Erro na movimentação de estoque (PKG_ESTOQUE.VENDAS_SAIDA): ${mensagem}`,
      'STOCK_MOVEMENT_FAILED',
      { numTransVenda, mensagem }
    );
  }
  return retorno;
}

/**
 * Recalcula o saldo reservado de um produto/filial
 * (PKG_ANALISAR_ESTOQUE.PRC_RESERVADO) — réplica de `RecalcularReserva` em
 * `ModuloFuncoes.vb`. Os demais campos do record TP_ENTRADA (depósitos,
 * departamentos, fornecedores, seções, categorias, subcategorias, marcas) são
 * sempre zerados no código original — mantido aqui por fidelidade.
 */
async function recalcularReserva(connection, { codProd, codFilial }) {
  await connection.execute(
    `DECLARE
       vENTRADA PKG_ANALISAR_ESTOQUE.TP_ENTRADA;
     BEGIN
       vENTRADA.CODFILIAL              := :codFilial;
       vENTRADA.LISTA_DE_DEPOSITOS     := 0;
       vENTRADA.LISTA_DE_PRODUTOS      := :codProd;
       vENTRADA.LISTA_DE_DEPARTAMENTOS := 0;
       vENTRADA.LISTA_DE_FORNECEDORES  := 0;
       vENTRADA.LISTA_DE_SECOES        := 0;
       vENTRADA.LISTA_DE_CATEGORIAS    := 0;
       vENTRADA.LISTA_DE_SUBCATEGORIAS := 0;
       vENTRADA.LISTA_DE_MARCAS        := 0;
       PKG_ANALISAR_ESTOQUE.PRC_RESERVADO(vENTRADA);
     END;`,
    { codFilial, codProd }
  );
}

module.exports = {
  getEstoqueDisponivel,
  getLotesDisponiveis,
  reservaIncluir,
  vendasSaida,
  recalcularReserva
};

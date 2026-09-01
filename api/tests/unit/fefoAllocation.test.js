'use strict';

const { allocateLotsFEFO } = require('../../src/modules/productionOrders/productionOrders.service');
const { allocateNextLotBatch } = require('../../src/modules/requisitions/requisitions.service');

describe('allocateLotsFEFO (início de produção — IniciarOP)', () => {
  test('consome o primeiro lote inteiro e completa no segundo quando insuficiente', () => {
    const lotes = [
      { NUMLOTE: 'A', QTDISPONIVEL: 10 },
      { NUMLOTE: 'B', QTDISPONIVEL: 20 }
    ];

    const { alocacoes, restanteNaoAlocado } = allocateLotsFEFO(lotes, 15);

    expect(alocacoes).toEqual([
      { NUMLOTE: 'A', QTDISPONIVEL: 10, alocado: 10 },
      { NUMLOTE: 'B', QTDISPONIVEL: 20, alocado: 5 }
    ]);
    expect(restanteNaoAlocado).toBe(0);
  });

  test('não consome o próximo lote quando o primeiro já é suficiente', () => {
    const lotes = [
      { NUMLOTE: 'A', QTDISPONIVEL: 50 },
      { NUMLOTE: 'B', QTDISPONIVEL: 20 }
    ];

    const { alocacoes } = allocateLotsFEFO(lotes, 15);

    expect(alocacoes).toEqual([{ NUMLOTE: 'A', QTDISPONIVEL: 50, alocado: 15 }]);
  });

  test('sinaliza saldo não alocado quando os lotes se esgotam', () => {
    const lotes = [{ NUMLOTE: 'A', QTDISPONIVEL: 4 }];

    const { restanteNaoAlocado } = allocateLotsFEFO(lotes, 10);

    expect(restanteNaoAlocado).toBe(6);
  });
});

describe('allocateNextLotBatch (requisição — RequisitarInsumos, versão corrigida)', () => {
  test('requisita apenas o que falta, não o total original do item', () => {
    // Cenário que expõe o bug do VB.NET original: item precisa de 100 no
    // total, já requisitou 60 do lote A (faltam 40); lote B tem 50
    // disponíveis. A versão corrigida deve pedir só os 40 que faltam, não os
    // 50 inteiros do lote B.
    const lotes = [{ NUMLOTE: 'B', QTNECESSIDADE: 50, QTREQUISITADO: 0 }];

    const resultado = allocateNextLotBatch(lotes, 40);

    expect(resultado).toEqual({ numLote: 'B', qtRequisitar: 40 });
  });

  test('requisita o saldo do lote quando ele é menor que o restante', () => {
    const lotes = [{ NUMLOTE: 'A', QTNECESSIDADE: 60, QTREQUISITADO: 0 }];

    const resultado = allocateNextLotBatch(lotes, 100);

    expect(resultado).toEqual({ numLote: 'A', qtRequisitar: 60 });
  });

  test('retorna null quando não há mais lotes disponíveis', () => {
    expect(allocateNextLotBatch([], 10)).toBeNull();
  });
});

'use strict';

const { z } = require('zod');

const codProgramaParam = z.object({
  codPrograma: z.string().regex(/^\d+$/, 'codPrograma deve ser numérico.')
});

const itemParam = codProgramaParam.extend({
  itemId: z.string().regex(/^\d+$/, 'itemId deve ser numérico.')
});

const scheduleQuery = z.object({
  codFilialEstoque: z.string().optional()
});

const addItemBody = z
  .object({
    codProd: z.string().min(1),
    descricao: z.string().min(1),
    metodo: z.string().optional(),
    qtProduzir: z.number().nonnegative(),
    horaInicial: z.string().datetime(),
    linha: z.string().min(1),
    numOp: z.string().optional(),
    numLote: z.string().optional(),
    qtUnitCx: z.number().optional(),
    embalagem: z.string().optional(),
    tempoTotal: z.number().nonnegative().optional(),
    // Representação numérica bruta da eficiência, na MESMA escala usada pela
    // tela original (txtEficiencia.Text, ex.: "9700" para 97,00%) — obrigatório
    // quando `tempoTotal` não é enviado e codProd <> '99999'. Ver nota em
    // productionSchedules.service.js/addItem.
    eficienciaRaw: z.number().positive().optional()
  })
  .refine((data) => data.codProd === '99999' || data.tempoTotal !== undefined || data.eficienciaRaw !== undefined, {
    message: 'Informe tempoTotal ou eficienciaRaw para calcular o tempo necessário.',
    path: ['eficienciaRaw']
  });

const removeItemQuery = z.object({
  cancelInWinthor: z.coerce.boolean().default(false)
});

const explodeBody = z.object({
  methodOverrides: z.record(z.string(), z.string()).optional().default({}),
  codFilialEstoque: z.string().optional(),
  codFilialProducao: z.string().optional()
});

module.exports = {
  codProgramaParam,
  itemParam,
  scheduleQuery,
  addItemBody,
  removeItemQuery,
  explodeBody
};

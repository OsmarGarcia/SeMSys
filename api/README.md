# SeMSys API

API REST em Node.js/Express que substitui o acesso direto ao Oracle (Winthor)
feito hoje pela aplicação desktop VB.NET do SeMSys. Implementa o plano
descrito em [`../docs/oracle-integration-rest-api-plan.md`](../docs/oracle-integration-rest-api-plan.md):
mesmas tabelas, mesmas packages PL/SQL, mesmas regras de negócio — só que
expostas via HTTP em vez de embutidas em formulários WinForms.

> Cada arquivo de serviço (`*.service.js`) traz, em comentário, qual rotina
> VB.NET original ele substitui e quais decisões de fidelidade/correção foram
> tomadas. Vale ler antes de alterar regra de negócio.

## Requisitos

- Node.js 18+
- Acesso de rede ao Oracle do Winthor (a API usa o driver `oracledb` em modo
  **thin** — não precisa instalar Oracle Instant Client)

## Configuração

```bash
cp .env.example .env
# edite .env com a connect string, usuário e senha do Oracle
```

Variáveis principais (ver `.env.example` para a lista completa e comentada):

| Variável | Descrição |
|---|---|
| `ORACLE_CONNECT_STRING` | `host:porta/SID` do Winthor (ex.: `192.168.0.20:1521/WINT`) |
| `ORACLE_USER` / `ORACLE_PASSWORD` | Credenciais do schema Winthor |
| `DEFAULT_CODFILIAL_PRODUCAO` / `DEFAULT_CODFILIAL_ESTOQUE` | Equivalentes a `My.Settings.CodFilialProducao`/`CodFilialEstoque` no VB.NET |
| `JWT_SECRET` | Segredo para validar o token Bearer de autenticação |
| `ENABLE_DEV_TOKEN` | Habilita `POST /auth/dev-token` para gerar tokens em desenvolvimento — **nunca deixe `true` em produção** |

## Rodando

```bash
npm install
npm run dev     # com reload automático (nodemon)
# ou
npm start
```

A API sobe em `http://localhost:3000` (ou a porta definida em `PORT`).

- `GET /health` — verificação de vida (não toca o Oracle).
- `POST /auth/dev-token` — emite um token Bearer de teste (só com `ENABLE_DEV_TOKEN=true`).

Todas as demais rotas exigem `Authorization: Bearer <token>`. Em produção, o
token deve ser emitido por quem autentica o usuário contra o cadastro de
funcionários do Winthor (`PCEMPR`) — a API só valida a assinatura e usa
`matricula`/`nome`/`codFilial` do payload para preencher os campos de autoria
(`CODFUNCLANC`, `CODFUNCINICIO`, `CODFUNCREQ`) nas tabelas do Winthor.

## Testes

```bash
npm test
```

Os testes automatizados são **unitários e não dependem de conexão com o
Oracle** (cobrem arredondamento, validação de payload, alocação FEFO de
lotes e a montagem do Express). Não há testes de integração contra um Oracle
real neste repositório — antes de ir para produção, valide manualmente os
fluxos de escrita (criar OP, iniciar produção, requisitar insumos, explodir
MRP, gerar OPs a partir da programação) contra uma base de homologação, já
que dependem de packages PL/SQL (`PKG_ESTOQUE.*`, `PKG_ANALISAR_ESTOQUE.*`,
`Reprogramar_OP_Func`, `FNC_PROXNUMLOTE`) e sequences (`DFSEQ_PCMOVCOMPLE`,
`DFSEQ_NOVO_SMPROGRAMAPRODUCAO`) específicas desse ambiente.

## Estrutura

```
src/
  config/env.js            # leitura/validação de variáveis de ambiente
  db/pool.js                # pool Oracle (modo thin)
  db/transaction.js         # withTransaction / withConnection
  middleware/                # auth (JWT), validação (zod), erros
  utils/                      # erros tipados, arredondamento, datas
  shared/
    sequences.service.js     # NUMOP, NUMLOTE, NUMTRANSVENDA, NUMSEQ...
    formula.service.js       # explosão de fórmula/BOM (BuscarFormula)
    stock.service.js         # wrappers de PKG_ESTOQUE / PKG_ANALISAR_ESTOQUE
    parametros.service.js    # casas decimais de estoque (PCCONSUM)
  modules/
    productionOrders/        # OPs: consulta, criação, reprogramação, início, cancelamento
    requisitions/             # separação/requisição de materiais de uma OP
    productionSchedules/      # programa de produção por linha (SMPROGRAMAPRODUCAO) + MRP
    products/                  # cadastro, fórmula, estoque, lotes
    branches/, employees/      # cadastros auxiliares
    reports/                   # relatório de produção total
  app.js / server.js
```

## Decisões de fidelidade ao VB.NET original

A tradução linha a linha das telas VB.NET para serviços HTTP expôs alguns
pontos do código original que precisaram de uma decisão consciente — todos
documentados também no código-fonte, no ponto exato onde aparecem:

- **Numeração sequencial (NUMOP/NUMLOTE/NUMTRANSVENDA) agora é serializada**
  com `SELECT ... FOR UPDATE` na tabela de parâmetros dentro da transação
  (`shared/sequences.service.js`). O VB.NET original lia e incrementava sem
  lock — seguro só porque a aplicação desktop era operada por uma pessoa de
  cada vez; numa API isso seria uma condição de corrida real.
- **Bug de sobre-requisição em lotes corrigido.** Em `RequisitarInsumos`
  (frmSeparacaoMaterial.vb), quando um insumo precisa de mais de um lote, o
  código original compara a quantidade a requisitar de cada lote contra a
  necessidade TOTAL do item em vez do que ainda falta — o que pode
  requisitar mais do que o necessário a partir do segundo lote. A API
  corrige isso (`requisitions.service.js#allocateNextLotBatch`), com o
  comportamento errado documentado e coberto por teste
  (`tests/unit/fefoAllocation.test.js`).
- **`DividirOP` corrigida.** A consulta de origem retorna a coluna
  `QTNECESSIDADE`, mas o VB.NET original lê uma coluna `QT` que não existe
  nesse resultado — o recurso provavelmente nunca funcionou no original
  (sempre cairia no `Catch` e mostraria "Erro ao realizar divisão da OP").
  A API usa o nome de coluna correto.
- **Coluna `DESCRICAO` de `PCMOV`** aparece em duas consultas originais
  (`PesquisarApontamentos` e o SELECT pós-`VENDAS_SAIDA` de
  `RequisitarInsumos`) — mas `PCMOV` normalmente não tem essa coluna. Mantida
  (com alerta em comentário) onde o valor é de fato exibido ao usuário;
  removida onde nunca era lida, para não quebrar em runtime por uma coluna
  que talvez não exista no seu schema.
- **Alias duplicado `DTMOV`** na consulta de produção total
  (`ClassProducaoTotalWinthor`) foi renomeado para `DTMOVLOG` — inofensivo em
  DataTable, mas faria uma chave sobrescrever a outra em JSON.
- **Duas filiais distintas na explosão de fórmula.** `FormularAcabados` usa
  `filialProducao` para `PCCOMPOSICAO.CODFILIAL` e `filialEstoque` para
  `PCEST.CODFILIAL`; as demais variantes de `BuscarFormula` usam uma única
  filial para as duas. `shared/formula.service.js` aceita ambos os parâmetros
  (com fallback para um único `codFilial`) para não perder essa distinção.

Nenhuma dessas mudanças altera o *resultado esperado* das regras de negócio —
todas existem para o comportamento realmente funcionar de forma consistente
como API, ou para corrigir um defeito que já existia no original. Qualquer
outra ambiguidade encontrada (ex.: o formato de data esperado por
`Reprogramar_OP_Func`, ou a escala do campo de eficiência usado no cálculo de
horas de programação) está sinalizada em comentário no ponto onde aparece,
pedindo confirmação com quem mantém o PL/SQL antes de operar em produção.

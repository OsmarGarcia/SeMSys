# Plano de API REST — Integração Oracle (Winthor ERP) do SeMSys

> Documento gerado a partir da leitura completa dos arquivos VB.NET que interagem com o banco Oracle (Winthor).
> Objetivo: mapear todas as integrações atuais (SQL direto, packages PL/SQL, sequences) e propor uma API REST que
> reproduza fielmente as regras de negócio hoje embutidas na camada VB.NET (WinForms), para permitir que o
> processo de produção passe a ser operado via API em vez de acesso direto ao Oracle pela aplicação desktop.

## 1. Escopo analisado

Arquivos com acesso direto ao Oracle (`Oracle.ManagedDataAccess.Client`):

| Arquivo | Responsabilidade |
|---|---|
| `mdlConexaoOracle.vb` | Módulo de conexão (TNS direto ao SID `WINT`, usuário `sampaio`) e conexão auxiliar SQL Server (`SampaioBD`, usada só para dados de paradas/IoT — fora do escopo Oracle). |
| `ModuloFuncoes.vb` | `RecalcularReserva` (PKG_ANALISAR_ESTOQUE), `DefinirMetodoMRP` (métodos de fórmula), reordenação de grade de programação (sem I/O). |
| `frmProgramarProducao.vb` | Núcleo do módulo de **Programação de Produção**: geração de OP no Winthor, explosão de MRP/BOM, programação por linha, reprogramação, cancelamento, impressão. |
| `frmManutencaoOP.vb` | **Manutenção de OP**: consulta, recálculo de itens/fórmula, início de produção (reserva de estoque/lote), listagem de OPs aguardando início, reserva manual de estoque. |
| `frmSeparacaoMaterial.vb` | **Separação/Requisição de Materiais**: baixa de insumos da OP (movimentação de estoque), divisão de OP por volume, impressão de requisição. |
| `frmPesquisarOP.vb` | Busca de OPs em produção (lookup usado por outras telas). |
| `ClassOrdemProducao.vb` | Impressão de OP / geração de código de barras (COD128) e data de validade do lote. |
| `ClassParadasOP.vb`, `ClassResumoOP.vb` | Relatórios híbridos (Winthor + SQL Server) — a parte Oracle é só leitura do progresso da OP (`PCOPC`/`PCPRODUT`). |
| `ClassProducaoTotalWinthor.vb` | Relatório de produção total por período (somente leitura). |

Não incluí no plano abaixo as partes que só usam SQL Server (`BDADOS_REGISTROS`, `TBL_REGISTRO_PROCESSO`, paradas de linha) porque o pedido foi especificamente sobre a integração com Oracle — mas deixei uma nota na seção 8 sobre a fronteira entre os dois bancos.

## 2. Modelo de dados Oracle mapeado (Winthor)

### Tabelas principais
| Tabela | Papel no fluxo |
|---|---|
| `PCOPC` | Cabeçalho da Ordem de Produção (NUMOP, produto master, método, quantidade, posição L/P/F/C, datas). |
| `PCOPI` | Itens/insumos necessários da OP (não controlados por lote): necessidade, requisitado, reserva. |
| `PCOPILOTE` | Itens/insumos necessários da OP **controlados por lote**: mesma semântica de `PCOPI` + número de lote e data de validade. |
| `PCCOMPOSICAO` | Fórmula/BOM (ficha técnica) de um produto acabado/semiacabado por método e filial. |
| `PCCOMPOSICAOFRACAO` | Explosão da fórmula gravada por OP (fracionamento). |
| `PCPRODUT` | Cadastro de produto (descrição, embalagem, velocidades nominais, tipo de mercadoria, controle por lote, prefixo de lote, tipo de sequência de lote). |
| `PCEST` | Saldo de estoque por filial/produto (geral, bloqueado, reservado, custos, giro). |
| `PCLOTE` | Lotes físicos de estoque (saldo, bloqueio, reserva, validade). |
| `PCESTENDERECO` | Saldo de estoque por endereço no WMS. |
| `PCMOV` / `PCMOVCOMPLE` | Movimentações de estoque (requisição de produção `SP`, apontamento de produção `EP`). |
| `PCCONSUM` | Parâmetros globais (próximo NUMOP, próximo NUMTRANSVENDA, casas decimais de estoque, próximo lote por filial). |
| `PEPARAMETROS` | Parâmetros de produção (próximo número de programa, regra de sequência de lote — por produto `P` ou por filial). |
| `PEPROGOP` / `PEPROGITENS` | Cabeçalho/itens da "ordem de produção planejada" (espelha `PCOPC`/`PCOPI` no módulo de planejamento). |
| `PCOBSOP` | Observações/log textual da OP. |
| `SMPROGRAMAPRODUCAO` | Tabela própria do SeMSys — grade de programação por linha de produção (não é tabela nativa Winthor). |
| `PCEMPR`, `PCFILIAL` | Cadastros auxiliares (funcionário, filial). |
| `PCSECAO`, `PCDEPTO` | Cadastros auxiliares usados em relatório de produção total. |

### Rotinas PL/SQL chamadas (regra de negócio que **vive no banco**, não no VB.NET)
| Rotina | Uso |
|---|---|
| `PKG_ESTOQUE.ESTOQUE_DISPONIVEL(codprod, filial, 'V')` | Estoque disponível considerando reservas/bloqueios. |
| `PKG_ESTOQUE.RESERVA_INCLUIR(numop, codprod, numseq, pedido, operacao, msg)` | Cria reserva de estoque para o insumo ao iniciar a OP. |
| `PKG_ESTOQUE.VENDAS_SAIDA(numtransvenda, 'N', msg)` | Efetiva a baixa de estoque (motor genérico de saída usado também para vendas). |
| `PKG_ANALISAR_ESTOQUE.PRC_RESERVADO(TP_ENTRADA)` | Recalcula saldo reservado de um produto/filial. |
| `FNC_PROXNUMLOTE(codprod, data)` | Próximo número de lote quando a sequência é "por filial". |
| `Reprogramar_OP_Func(numop, novaQt, numlote, dtprevinicio)` | Reprograma quantidade/lote/data de início de uma OP existente; retorna `'SUCESSO'` ou mensagem de erro. |
| Sequences: `DFSEQ_PCMOVCOMPLE`, `DFSEQ_NOVO_SMPROGRAMAPRODUCAO` | Geração de identificadores. |

## 3. Fluxos de negócio e regras identificadas

### 3.1 Consulta de Ordens de Produção
- **Listar OPs em produção**: `PCOPC.POSICAO = 'P'` (usado como lookup em várias telas).
- **Listar OPs aguardando início**: `POSICAO = 'L' AND NVL(QTPRODUZIDA,0) <= NVL(QTPRODUZIR,0) AND CODFILIAL = :filialProducao AND DTPREVINICIO BETWEEN :inicio AND :fim`.
- **Cabeçalho da OP**: produto master, descrição, quantidade a produzir, método, posição, "kit" (`MODOPREPARO` de `PCCOMPOSICAO`, `'N/A'` se nulo).
- **Itens/insumos da OP**: união de dois conjuntos —
  - Não controlado por lote (`PCOPI`, quando `NVL(PCPRODUT.ESTOQUEPORLOTE,'N') = 'N'`);
  - Controlado por lote (`PCOPILOTE`, quando `= 'S'`).
  - Custos (`CUSTOREAL`, `CUSTOFIN`, `CUSTOCONT`, `VALORULTENT`, `CUSTOULTENT`) sempre arredondados para `PCCONSUM.NUMCASASDECESTOQUE` casas decimais.
- **Alerta de insuficiência de estoque** (grade de OPs aguardando início): item é destacado quando `QTNECESSIDADE > ESTOQUE_DISPONIVEL` e `TIPOMERC <> 'SA'` (semiacabados não bloqueiam, pois serão produzidos internamente).
- **Apontamentos/movimentos da OP**: histórico de `PCMOV` da OP, com nome do funcionário via `PCEMPR`.

### 3.2 Geração de Programação (criação de OP no Winthor)
Regras, em ordem:
1. Próximo `NUMOP` = `MAX(maior entre PEPROGOP.NUMOP e PCOPC.NUMOP) + 1`; grava de volta em `PEPARAMETROS.PROXNUMPROG` e `PCCONSUM.PROXNUMOP` (contador incremental, precisa de lock/serialização).
2. Determinação do **número de lote**, conforme `PEPARAMETROS.SEQUENCIALOTE`:
   - `'P'` (por produto): usa `PCPRODUT.PREFIXOLOTE || PROXNUMLOTE`.
   - Outro (por filial): usa `PCPRODUT.DESCRICAO7` (tipo de lote) e a função `FNC_PROXNUMLOTE(codprod, dtprevinicio)`; se o tipo **não** for `JULIANO`/`TAMPICO`, incrementa `PCCONSUM.PROXNUMLOTE`; para `JULIANO`/`TAMPICO` o lote é calculado por data (dia juliano do ano) e **não** consome contador.
3. Grava `PEPROGOP` (planejamento) e `PCOPC` (OP Winthor) com o mesmo `NUMOP`, posição inicial `'L'` (aguardando).
4. Grava `PCOBSOP` com observação padrão "ORDEM DE PRODUCAO GERADA COM SUCESSO".
5. Explode a fórmula (`BuscarFormula`) e grava, para cada insumo: `PEPROGITENS`, `PCOPI` (necessidade calculada, requisitado 0), `PCCOMPOSICAOFRACAO`.
6. Toda a operação é uma única transação — qualquer erro reverte tudo.
7. **Explosão de MRP multinível** (`GerarMRP`/`FormularAcabados`): para cada item da grade de produtos acabados, explode a fórmula; itens do tipo `TIPOMERC = 'SA'` (semiacabado) sem método definido disparam escolha manual de método (`DefinirMetodoMRP`) e são explodidos recursivamente até não restar semiacabado não processado. Itens que somam quantidade líquida zero após a explosão têm a OP associada cancelada automaticamente (`PCOPC.POSICAO='C'`).

### 3.3 Reprogramação de OP existente
- Só reprograma se a nova quantidade for diferente da atual **ou** o lote for do tipo `JULIANO`/`TAMPICO` (que precisa recalcular o lote mesmo com quantidade igual, por depender da data).
- Bloqueia reprogramação se a OP tiver quantidade zerada ou posição `'C'`/`'F'`.
- Para lote por data (`JULIANO`/`TAMPICO`), recalcula o lote via `FNC_PROXNUMLOTE` antes de aplicar.
- Aplica a mudança via função `Reprogramar_OP_Func(numop, novaQt, numlote, dtprevinicio)`; exige retorno `'SUCESSO'`.

### 3.4 Início de produção (liberar OP para produzir)
1. Para cada insumo da OP (`PCOPI`/`PCOPC`), calcula estoque disponível via `PKG_ESTOQUE.ESTOQUE_DISPONIVEL(codprod, filial, 'V')`.
2. Se `QTNECESSIDADE > ESTOQUE` e o insumo **não** é semiacabado (`TIPOMERC <> 'SA'`), a operação inteira é bloqueada e a lista de itens faltantes é retornada.
3. Se passou na validação, para cada insumo:
   - `UPDATE PCOPI SET QTRESERVALTERAR = QTNECESSIDADE` (não permite valor negativo) e `RESERVALIBERADA = 'N'`.
   - Se controlado por lote: seleciona lotes disponíveis em `PCLOTE` (saldo líquido = `QT - QTBLOQUEADA - QTRESERV - QTTEMPINDUSTRIA > 0`), ordenados por **FEFO** (data de validade crescente); aloca quantidade necessária cumulativamente entre lotes (`QTTEMPINDUSTRIA`), grava `PCOPILOTE` por lote alocado e zera `QTTEMPINDUSTRIA` do lote.
   - Chama `PKG_ESTOQUE.RESERVA_INCLUIR` (operação `'II'`) para reservar efetivamente o insumo; exige retorno de mensagem `'OK'`.
4. Ao final, com sucesso em todos os insumos: `UPDATE PCOPC SET POSICAO='P', DTINICIO=TRUNC(SYSDATE), CODFUNCINICIO=:usuario` e `UPDATE PCOPI SET BAIXAVIRTUAL='N'`.
5. Tudo em uma única transação Oracle.

### 3.5 Requisição/Separação de materiais (consumo de insumos)
1. Só permite requisitar se `PCOPC.POSICAO = 'P'` (OP iniciada).
2. Recalcula necessidade proporcional: `QTNECESSIDADE_original * qtSolicitada / QTPRODUZIR_original` (permite requisitar por fração/volume, ex.: separação por palete).
3. Valida disponibilidade em **dois estoques** para itens que usam WMS (`PCPRODUT.USAWMS='S'`): saldo Winthor (`PCEST.QTESTGER - QTBLOQUEADA`) **e** saldo WMS (`SUM(PCESTENDERECO.QT) - SUM(QTPENDSAIDA)`); bloqueia toda a operação listando os itens insuficientes.
4. Gera número de transação (`PCCONSUM.PROXNUMTRANSVENDA`, incrementado) e próximo `NUMSEQ` de `PCMOV` para a OP.
5. Para cada insumo pendente de requisitar (laço `REVALIDAR` até zerar tudo):
   - Se controlado por lote: seleciona lotes com saldo (`PCOPILOTE.QT > QTREQUISITADO`) em ordem **FEFO** (validade asc, depois maior já requisitado desc); requisita o menor entre necessidade e saldo do lote da vez, podendo consumir múltiplos lotes em iterações sucessivas.
   - Se não controlado por lote: requisita a quantidade total de uma vez, lote fixo `'1'`.
   - Insere `PCMOV` (operação `'SP'`, status `'AB'`, custos herdados de `PCEST`) e `PCMOVCOMPLE`; atualiza `PCOPILOTE.QTREQUISITADO` quando aplicável.
6. Chama `PKG_ESTOQUE.VENDAS_SAIDA(numtransvenda, 'N', msg)` para efetivar a baixa; exige retorno numérico `> 0` e mensagem `'OK'`.
7. Pós-baixa: atualiza indicadores de giro em `PCEST` (`QTVENDMES/DIA/SEMANA`, `DTULTSAIDA`) e reduz reserva/aumenta requisitado em `PCOPI` para cada linha de `PCMOV` gerada.
8. Toda a operação é uma transação única.
9. Função auxiliar de **divisão de OP** (`DividirOP`) apenas recalcula proporcionalmente as quantidades dos insumos para fins de impressão de múltiplas requisições parciais (não persiste no banco).

### 3.6 Recalcular itens da OP (troca de método/fórmula)
- Apaga `PCOPI` e `PCCOMPOSICAOFRACAO` da OP.
- Reexplode a fórmula (`BuscarFormula`) com o método atualizado e reinsere `PEPROGITENS`/`PCOPI`/`PCCOMPOSICAOFRACAO`.
- Atualiza `PCOPC.METODO`.
- Transação única.

### 3.7 Cancelamento de OP
- Cancelamento veio do programa de produção: sempre marca o item como `'CANCELADA'` em `SMPROGRAMAPRODUCAO`; se havia uma OP Winthor vinculada, pergunta e, se confirmado, `UPDATE PCOPC SET POSICAO='C'`.
- Cancelamento automático dentro do MRP quando a necessidade líquida do item recalculada fica zero.

### 3.8 Recalcular reserva de estoque (rotina de manutenção)
- Monta um record `PKG_ANALISAR_ESTOQUE.TP_ENTRADA` com filial + produto (demais filtros zerados) e chama `PKG_ANALISAR_ESTOQUE.PRC_RESERVADO`.

### 3.9 Consultas auxiliares (somente leitura)
- Dados de produto (descrição, embalagem, velocidade nominal por linha `M30` vs. demais, quantidade por caixa) — `PCPRODUT`.
- Métodos de fórmula disponíveis para um produto — `PCCOMPOSICAO` distinct.
- Filiais cadastradas — `PCFILIAL`.
- Relatório de produção total por período — `PCMOV`+`PCPRODUT`+`PCOPC`+`PCSECAO`+`PCDEPTO`, filtros fixos (`CODFILIAL IN (1,4)`, `CODEPTO IN ('30','40')`, `CODOPER IN ('EP','SP')`); classifica operação como apontamento, estorno ou cancelamento (`QT<0`).
- Impressão/etiqueta da OP: monta string COD128 e calcula validade a partir de `PCPRODUT.DESCRICAO6` (unidade do prazo: meses ou dias) e regras específicas de lote `JULIANO`/`TAMPICO`.

## 4. Modelo de recursos da API REST

```
/production-orders                          → PCOPC (Ordens de Produção)
/production-orders/{numop}
/production-orders/{numop}/items            → PCOPI + PCOPILOTE
/production-orders/{numop}/movements        → PCMOV (apontamentos/requisições)
/production-orders/{numop}/label            → dados de etiqueta/COD128
/production-orders/{numop}/start            → ação: iniciar produção
/production-orders/{numop}/requisitions     → ação: requisitar/baixar insumos
/production-orders/{numop}/reprogram        → ação: reprogramar qtd/lote/data
/production-orders/{numop}/cancel           → ação: cancelar OP
/production-orders/{numop}/recalculate-items → ação: refazer fórmula/itens

/production-schedules                       → SMPROGRAMAPRODUCAO (programa de produção por linha)
/production-schedules/{id}
/production-schedules/{id}/items            → produtos acabados da grade
/production-schedules/{id}/materials        → insumos/MRP explodido (semiacabados + matérias-primas)
/production-schedules/{id}/generate-orders  → ação: gerar OPs Winthor a partir do programa

/products/{codprod}                         → PCPRODUT (cadastro)
/products/{codprod}/methods                 → métodos de fórmula (PCCOMPOSICAO distinct)
/products/{codprod}/formula                 → explosão de fórmula (BuscarFormula)
/products/{codprod}/stock                   → estoque disponível (PKG_ESTOQUE.ESTOQUE_DISPONIVEL)
/products/{codprod}/stock/recalculate       → ação: recalcular reserva (PKG_ANALISAR_ESTOQUE)
/products/{codprod}/lots                    → PCLOTE (saldo por lote, FEFO)

/branches                                   → PCFILIAL
/employees/{matricula}                      → PCEMPR (uso interno/relatórios)

/reports/production-total                   → relatório PCMOV x PCOPC (produção total por período)
```

## 5. Lista de endpoints

### 5.1 Ordens de Produção (`/production-orders`)

| Método | Rota | Descrição | Regras de negócio aplicadas |
|---|---|---|---|
| `GET` | `/production-orders?position=P` | Lista OPs por posição (`L`,`P`,`F`,`C`) — usado hoje pelo lookup de pesquisa. | Filtro obrigatório por `CODFILIAL`; default `POSICAO='P'` reproduz `frmPesquisarOP`. |
| `GET` | `/production-orders?position=L&startDateFrom&startDateTo&branch=` | Lista OPs aguardando início dentro de uma janela de `DTPREVINICIO`. | Reproduz `PesquisarOPsIniciar`; resposta inclui flag `stockShortage: boolean` por item calculada com a mesma regra de `TIPOMERC<>'SA'`. |
| `GET` | `/production-orders/{numop}` | Cabeçalho da OP. | Inclui `kit` (`MODOPREPARO`) e posição textual (`L/P/F/C` já traduzido). |
| `POST` | `/production-orders` | Cria uma nova OP (equivalente a `GerarProgramacao`). Body: `codprod`, `metodo`, `qtProduzir`, `dtPrevInicio`, `codFuncLanc`. | Executa toda a sequência 3.2 (numeração, lote, `PEPROGOP`/`PCOPC`/`PCOBSOP`, explosão de fórmula) em transação única; retorna `numop`+`numlote` gerados. |
| `POST` | `/production-orders/{numop}/reprogram` | Body: `novaQtProduzir`, `numLote?`, `dtPrevInicio`. | Aplica 3.3 (idempotente quando nada muda e o lote não é por data); chama `Reprogramar_OP_Func`. |
| `POST` | `/production-orders/{numop}/start` | Sem body (usa usuário autenticado). | Aplica 3.4 integralmente; em caso de estoque insuficiente retorna `409` com lista de insumos faltantes (`codprod`,`necessario`,`disponivel`). |
| `POST` | `/production-orders/{numop}/recalculate-items` | Body: `metodo` (opcional, se trocou o método). | Aplica 3.6. |
| `POST` | `/production-orders/{numop}/cancel` | Body: `cancelInWinthor: boolean`. | Reproduz o fluxo de confirmação dupla do botão excluir (cancela no programa e, se solicitado, também `PCOPC.POSICAO='C'`). |
| `GET` | `/production-orders/{numop}/items` | Itens/insumos necessários (união `PCOPI`/`PCOPILOTE`). | Aplica arredondamento por `PCCONSUM.NUMCASASDECESTOQUE`; inclui custos. |
| `GET` | `/production-orders/{numop}/movements` | Histórico de `PCMOV` da OP (apontamentos/requisições), com nome do funcionário. | Ordenado por `NUMTRANSVENDA`. |
| `GET` | `/production-orders/{numop}/label` | Dados para impressão de etiqueta/COD128 (`ClassOrdemProducao.ImprimirOP`). | Reproduz cálculo de validade (mensal vs. dias) e lote especial `JULIANO`/`TAMPICO`. |

### 5.2 Requisição de materiais (`/production-orders/{numop}/requisitions`)

| Método | Rota | Descrição | Regras de negócio |
|---|---|---|---|
| `GET` | `/production-orders/{numop}/requisitions/preview?qty=` | Simula a requisição sem gravar: retorna itens, quantidades e alerta de estoque insuficiente (Winthor x WMS). | Reproduz a etapa de validação de 3.5 (itens 1–3) sem efetivar. |
| `POST` | `/production-orders/{numop}/requisitions` | Body: `qty` (quantidade a requisitar/separar). | Executa 3.5 completo (numeração, FEFO por lote, `PCMOV`/`PCMOVCOMPLE`, `PKG_ESTOQUE.VENDAS_SAIDA`, atualização de giro e `PCOPI`) em transação única; `409` se estoque insuficiente. |
| `GET` | `/production-orders/{numop}/requisitions/split?divisor=&qty=` | Retorna a divisão de quantidades por volume/palete para impressão (não grava nada). | Reproduz `DividirOP`. |

### 5.3 Programação de Produção (`/production-schedules`)

| Método | Rota | Descrição | Regras de negócio |
|---|---|---|---|
| `POST` | `/production-schedules` | Cria um novo programa (código sequencial `DFSEQ_NOVO_SMPROGRAMAPRODUCAO`). | — |
| `GET` | `/production-schedules/{id}` | Carrega cabeçalho + itens + materiais do programa. | Reproduz `btnCarregarPrograma`, incluindo atualização de estoque disponível e tipo de mercadoria por item de material. |
| `PUT` | `/production-schedules/{id}` | Substitui a grade completa (itens de produtos acabados por linha + horários). | Reaplica o algoritmo de sequenciamento por linha (recalcula hora início/fim por linha a partir da lista ordenada por `datainicial`) — equivalente a `ReprogramarOPs`. |
| `POST` | `/production-schedules/{id}/items` | Adiciona um item de produto acabado à grade de uma linha. Body: `codprod`, `metodo`, `qtProduzir`, `linha`, `dtInicio`, `numOp?`, `numLote?`. | Valida OP existente e quantidade (regra de 3.2/3.3: se a OP informada já existe e a quantidade/data mudou, reprograma automaticamente no Winthor antes de inserir na grade); calcula horas necessárias = `qt / velocidadeNominal / eficiência`, mais paradas de turno/intervalo. |
| `DELETE` | `/production-schedules/{id}/items/{itemId}` | Remove item da grade. | Se o item tiver OP associada, cancela no programa e pergunta/objeto de confirmação para cancelar no Winthor (3.7); reordena a linha após remoção. |
| `POST` | `/production-schedules/{id}/materials/explode` | Recalcula o MRP (explosão de BOM multinível) para todos os produtos acabados da grade. | Reproduz `GerarMRP`/`FormularAcabados`: soma necessidades por insumo, explode semiacabados recursivamente, exige escolha de método quando ambíguo (retorna `422` pedindo `metodo` quando um semiacabado não tem método definido), cancela automaticamente OPs cuja necessidade líquida zerar. |
| `POST` | `/production-schedules/{id}/generate-orders` | Gera as OPs reais no Winthor para os itens da grade que ainda não têm `numop`. | Chama `POST /production-orders` internamente para cada item pendente (produtos acabados e depois semiacabados), na mesma ordem do código (`btnProgramar2_Click` → `btnProgramarSA_Click`). |
| `GET` | `/production-schedules/{id}/print` | Dados formatados para o relatório do programa (inclui semiacabados). | Reproduz `ImprimirPrograma`/`ImprimirSemiAcabado`. |

### 5.4 Produtos / Fórmula / Estoque (`/products`)

| Método | Rota | Descrição | Regras de negócio |
|---|---|---|---|
| `GET` | `/products/{codprod}` | Cadastro básico (descrição, embalagem, tipo de mercadoria, controle por lote, prefixo de lote). | — |
| `GET` | `/products/{codprod}/methods?branch=` | Métodos de fórmula distintos cadastrados. | — |
| `GET` | `/products/{codprod}/formula?method=&branch=&qty=` | Explosão de uma fórmula para uma quantidade (`BuscarFormula`). | Quantidade de cada insumo = `SUM(qtd unitária) * qty`, arredondada a 3–6 casas conforme o caller; inclui estoque disponível líquido (`QTESTGER-QTBLOQUEADA-QTRESERV`). |
| `GET` | `/products/{codprod}/stock?branch=` | Estoque disponível "de venda" (`PKG_ESTOQUE.ESTOQUE_DISPONIVEL`). | Usado nas validações de início de OP/requisição. |
| `POST` | `/products/{codprod}/stock/recalculate` | Body: `branch`. | Reproduz `RecalcularReserva` (chamada a `PKG_ANALISAR_ESTOQUE.PRC_RESERVADO`). |
| `GET` | `/products/{codprod}/lots?branch=` | Lotes com saldo disponível, ordenados por validade (FEFO). | Mesmo filtro de exclusão de lote excluído/zerado usado em `IniciarOP`. |

### 5.5 Cadastros auxiliares

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/branches` | Lista `PCFILIAL` (código). |
| `GET` | `/employees/{matricula}` | Nome do funcionário (uso interno em relatórios/apontamentos). |

### 5.6 Relatórios

| Método | Rota | Descrição | Regras de negócio |
|---|---|---|---|
| `GET` | `/reports/production-total?startDate=&endDate=` | Produção total no período. | Filtros fixos: filiais `1,4`; departamentos `30,40`; operações `EP`/`SP`; classifica linha como Apontamento/Estorno/Cancelamento conforme `CODOPER`/sinal de `QT`. |

## 6. Regras transversais que a API precisa preservar

1. **Transações atômicas por caso de uso.** Cada ação (`start`, `requisitions`, criação de OP, recálculo de itens) precisa ser uma única transação Oracle no backend — hoje isso já é assim no VB.NET (uso extensivo de `OracleTransaction` com rollback em qualquer exceção); a API deve manter essa atomicidade e nunca deixar uma etapa parcial persistida.
2. **Numeração sequencial sensível a concorrência.** `NUMOP`, `NUMLOTE`, `NUMTRANSVENDA` e `NUMSEQ` são hoje calculados por `SELECT MAX(...)+1` seguido de `UPDATE` — sujeito a *race condition* sob concorrência real. Ao migrar para API (potencialmente multiusuário/multithread), recomenda-se serializar essas seções (lock explícito de linha ou `SELECT ... FOR UPDATE` na tabela de parâmetros) dentro da mesma transação, mantendo a regra de negócio (fonte do número), mas eliminando a condição de corrida que hoje só é "tolerada" pelo uso desktop sequencial.
3. **Validação de posição da OP antes de qualquer ação de escrita**: `L` para iniciar, `P` para requisitar, nunca `C`/`F` para qualquer alteração. A API deve validar isso no início de cada endpoint de ação e responder `409 Conflict` com a posição atual.
4. **FEFO (First-Expire-First-Out)** é regra de negócio obrigatória tanto para reserva (início de OP) quanto para requisição de insumos controlados por lote — deve ser reimplementada exatamente com a mesma ordenação (`DTVALIDADE ASC`, e na requisição também `QTREQUISITADO DESC` como desempate).
5. **Semiacabados (`TIPOMERC='SA'`) não bloqueiam por estoque** nas validações de início/MRP — são tratados como "serão produzidos", não como insumo comprado.
6. **Arredondamento de estoque configurável** via `PCCONSUM.NUMCASASDECESTOQUE` deve ser respeitado em qualquer endpoint que devolva quantidades/custos de insumo.
7. **Erros de packages PL/SQL são regra de negócio, não infraestrutura**: `PKG_ESTOQUE.VENDAS_SAIDA` e `PKG_ESTOQUE.RESERVA_INCLUIR` retornam mensagens de negócio (`'OK'`/outra coisa) que devem virar `422`/`409` com o texto original do Oracle, não um erro genérico 500.
8. **Identidade do usuário**: hoje é `My.Settings.UsuarioWinthor` (config local). Na API isso deve vir do token de autenticação e ser usado em `CODFUNCLANC`/`CODFUNCINICIO`/`CODFUNCREQ` — nunca aceito como campo livre do payload, para não permitir falsificação de autoria.
9. **Idempotência das ações de escrita**: como a tela original é operada por clique único mas o usuário pode reenviar, endpoints como `/start`, `/requisitions` e `/production-orders` (criação) devem aceitar uma chave de idempotência ou, no mínimo, detectar que a OP já está no estado alvo e responder sem duplicar efeito (ex.: já iniciada → 200 idempotente; já requisitada com aquela mesma quantidade → tratar conforme regra 3.5).

## 7. Contrato de erro sugerido

```json
{
  "error": "INSUFFICIENT_STOCK",
  "message": "Insumos sem estoque disponível para iniciar o processo.",
  "details": [
    { "codprod": "12345", "descricao": "AÇÚCAR REFINADO", "necessario": 120.500, "disponivel": 80.000 }
  ]
}
```
Mapeia diretamente as mensagens de `MessageBox.Show` hoje usadas nas validações (item 3.4, 3.5), preservando os mesmos critérios de bloqueio.

## 8. Fronteira com o SQL Server (fora do escopo Oracle, mas relevante)

`ClassParadasOP.vb` e `ClassResumoOP.vb` combinam dados do Oracle (`PCOPC`/`PCPRODUT`: quantidade programada, produzida, posição) com dados de um SQL Server separado (`BDADOS_REGISTROS`, `TBL_REGISTRO_PROCESSO` — paradas de linha e sensores de produção, provavelmente IoT). Se a intenção for expor um endpoint de "resumo/paradas da OP" via API, ele precisará agregar as duas fontes (endpoint composto, ex. `GET /production-orders/{numop}/summary`), mas isso é uma integração adicional que não faz parte da pergunta original sobre Oracle — mencionado aqui apenas para não perder o relacionamento entre os dois sistemas ao planejar a API completa.

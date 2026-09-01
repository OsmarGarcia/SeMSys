'use strict';

// Variáveis mínimas para src/config/env.js não falhar ao ser importado nos
// testes — nenhum teste unitário aqui abre conexão real com o Oracle.
process.env.NODE_ENV = 'test';
process.env.ORACLE_USER = process.env.ORACLE_USER || 'test_user';
process.env.ORACLE_PASSWORD = process.env.ORACLE_PASSWORD || 'test_pass';
process.env.ORACLE_CONNECT_STRING = process.env.ORACLE_CONNECT_STRING || 'localhost:1521/TESTDB';
process.env.JWT_SECRET = process.env.JWT_SECRET || 'test-secret-do-not-use-in-production';
process.env.ENABLE_DEV_TOKEN = process.env.ENABLE_DEV_TOKEN || 'true';
process.env.DEFAULT_CODFILIAL_PRODUCAO = process.env.DEFAULT_CODFILIAL_PRODUCAO || '1';
process.env.DEFAULT_CODFILIAL_ESTOQUE = process.env.DEFAULT_CODFILIAL_ESTOQUE || '1';
process.env.LOG_LEVEL = 'silent';

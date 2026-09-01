'use strict';

const request = require('supertest');
const createApp = require('../../src/app');

describe('GET /health', () => {
  test('responde 200 sem precisar de conexão com o Oracle', async () => {
    const app = createApp();
    const response = await request(app).get('/health');

    expect(response.status).toBe(200);
    expect(response.body.status).toBe('ok');
  });
});

describe('autenticação', () => {
  test('rota protegida sem token retorna 401', async () => {
    const app = createApp();
    const response = await request(app).get('/production-orders?position=P');

    expect(response.status).toBe(401);
    expect(response.body.error).toBe('UNAUTHORIZED');
  });

  test('emite e aceita um dev-token (ENABLE_DEV_TOKEN=true)', async () => {
    const app = createApp();

    const tokenResponse = await request(app)
      .post('/auth/dev-token')
      .send({ matricula: '1234', nome: 'TESTE', codFilial: '1' });

    expect(tokenResponse.status).toBe(200);
    expect(tokenResponse.body.data.token).toBeTruthy();
  });
});

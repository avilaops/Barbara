import request from 'supertest';
import app from '../server.js';

describe('Healthcheck', () => {
    it('retorna status ok', async () => {
        const res = await request(app).get('/health');
        expect(res.statusCode).toBe(200);
        expect(res.body.status).toBe('ok');
    });
});

// Como o Mongo não está configurado nos testes ainda, testamos somente rotas simples.

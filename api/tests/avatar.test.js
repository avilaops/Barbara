import request from 'supertest';
import app from '../server.js';
import {
    getAvatarJobById,
    markJobCompleted
} from '../services/avatarJobs.js';
import { processAvatarJob } from '../services/avatarProcessor.js';

describe('Avatar API', () => {
    it('cria um job de avatar e retorna requestId', async () => {
        const payload = {
            userId: 'user-123',
            frontImageUrl: 'https://example.com/front.jpg',
            sideImageUrl: 'https://example.com/side.jpg'
        };

        const res = await request(app).post('/avatar/generate').send(payload);
        expect(res.statusCode).toBe(202);
        expect(res.body).toHaveProperty('requestId');
        expect(res.body).toHaveProperty('status', 'queued');

        const job = await getAvatarJobById(res.body.requestId);
        expect(job).not.toBeNull();
        expect(job.status).toBe('queued');
    });

    it('processa job via mock provider', async () => {
        const res = await request(app).post('/avatar/generate').send({
            userId: 'user-456',
            frontImageUrl: 'https://example.com/front.jpg',
            sideImageUrl: 'https://example.com/side.jpg',
            provider: 'mock'
        });

        const jobId = res.body.requestId;
        const jobBefore = await getAvatarJobById(jobId);
        const result = await processAvatarJob(jobBefore);
        await markJobCompleted(jobId, result);
        const jobAfter = await getAvatarJobById(jobId);

        expect(jobAfter.status).toBe('ready');
        expect(jobAfter.glbUrl).toContain(jobId);
        expect(jobAfter.metadata).toHaveProperty('provider', 'mock');
    });

    it('lista jobs por usuário', async () => {
        const payload = {
            userId: 'user-list',
            frontImageUrl: 'https://example.com/front.jpg',
            sideImageUrl: 'https://example.com/side.jpg'
        };
        await request(app).post('/avatar/generate').send(payload);

        const res = await request(app).get('/avatar').query({ userId: payload.userId });
        expect(res.statusCode).toBe(200);
        expect(res.body.items.length).toBeGreaterThanOrEqual(1);
        expect(res.body).toHaveProperty('total');
    });
});

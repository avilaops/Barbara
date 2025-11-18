import express from 'express';
import Joi from 'joi';
import {
    createAvatarJob,
    getAvatarJobById,
    listJobsByUser
} from '../services/avatarJobs.js';
import { enqueueAvatarJob as enqueueRabbitMQ } from '../services/rabbitmq.js';
import { enqueueAvatarJob as enqueueAzureQueue } from '../services/azureQueue.js';

const router = express.Router();

const avatarRequestSchema = Joi.object({
    userId: Joi.string().required(),
    frontImageUrl: Joi.string().uri().required(),
    sideImageUrl: Joi.string().uri().required(),
    provider: Joi.string().valid('mock', 'ready-player-me', 'tryon-diffusion').optional()
});

const avatarListSchema = Joi.object({
    userId: Joi.string().required(),
    limit: Joi.number().integer().min(1).max(100).default(20),
    page: Joi.number().integer().min(1).default(1)
});

router.post('/generate', async (req, res, next) => {
    try {
        const { error, value } = avatarRequestSchema.validate(req.body, { abortEarly: false });
        if (error) return res.status(400).json({ errors: error.details });

        const provider = value.provider || process.env.AVATAR_PROVIDER || 'mock';
        const job = await createAvatarJob({
            userId: value.userId,
            frontImageUrl: value.frontImageUrl,
            sideImageUrl: value.sideImageUrl,
            provider
        });

        const queueMode = process.env.AVATAR_QUEUE_MODE || 'local';
        if (queueMode === 'rabbitmq') {
            await enqueueRabbitMQ(job.id).catch((err) => {
                req.log?.error({ err }, 'Falha ao enfileirar no RabbitMQ');
            });
        } else if (queueMode === 'azure-queue') {
            await enqueueAzureQueue(job.id).catch((err) => {
                req.log?.error({ err }, 'Falha ao enfileirar no Azure Queue');
            });
        }

        res.status(202).json({
            requestId: job.id,
            status: job.status,
            provider: job.provider
        });
    } catch (err) {
        next(err);
    }
});

router.get('/:id', async (req, res, next) => {
    try {
        const job = await getAvatarJobById(req.params.id);
        if (!job) return res.status(404).json({ error: 'Avatar não encontrado' });
        res.json(job);
    } catch (err) {
        next(err);
    }
});

router.get('/', async (req, res, next) => {
    try {
        const { error, value } = avatarListSchema.validate(req.query, { abortEarly: false });
        if (error) return res.status(400).json({ errors: error.details });

        const result = await listJobsByUser(value.userId, {
            limit: value.limit,
            page: value.page
        });

        res.json(result);
    } catch (err) {
        next(err);
    }
});

export default router;

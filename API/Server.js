import dotenv from 'dotenv';
import express from 'express';
import cors from 'cors';
import helmet from 'helmet';
import rateLimit from 'express-rate-limit';
import pino from 'pino';
import pinoHttp from 'pino-http';
import * as Sentry from '@sentry/node';
import { connectDatabase } from './config/database.js';
import catalogRouter from './routes/catalog.js';
import avatarRouter from './routes/avatar.js';
import { startAvatarJobWorker } from './jobs/avatarWorker.js';
import { startAvatarConsumer as startRabbitMQConsumer } from './services/rabbitmq.js';
import { startAzureQueueWorker } from './services/azureQueue.js';
import { processJobById } from './jobs/jobProcessor.js';

dotenv.config();

const app = express();

const logger = pino({
    level: process.env.LOG_LEVEL || 'info',
    transport: process.env.NODE_ENV !== 'production'
        ? {
            target: 'pino-pretty',
            options: {
                translateTime: 'SYS:standard',
                colorize: true
            }
        }
        : undefined
});

const sentryDsn = process.env.SENTRY_DSN;
if (sentryDsn) {
    Sentry.init({
        dsn: sentryDsn,
        environment: process.env.SENTRY_ENVIRONMENT || process.env.NODE_ENV || 'production',
        tracesSampleRate: 0.1
    });
    app.use(Sentry.Handlers.requestHandler());
}

const originList = (process.env.ALLOWED_ORIGINS || '*')
    .split(',')
    .map((origin) => origin.trim())
    .filter(Boolean);
const allowAllOrigins = originList.includes('*');

const corsOptions = {
    origin: (origin, callback) => {
        if (!origin || allowAllOrigins || originList.includes(origin)) {
            return callback(null, true);
        }
        return callback(new Error('CORS origin not allowed'));
    },
    credentials: true
};

// Middlewares
app.use(cors(corsOptions));
app.use(helmet({
    crossOriginResourcePolicy: { policy: 'cross-origin' },
    crossOriginOpenerPolicy: { policy: 'same-origin-allow-popups' }
}));
app.use(express.json({ limit: '5mb' }));
app.use(pinoHttp({ logger }));

const avatarLimiter = rateLimit({
    windowMs: 60 * 1000,
    max: Number(process.env.AVATAR_RATE_LIMIT || 10),
    standardHeaders: true,
    legacyHeaders: false
});

// Healthcheck
app.get('/health', (req, res) => {
    res.json({ status: 'ok', timestamp: new Date().toISOString() });
});

// Routes
app.use('/catalog', catalogRouter);
app.use('/avatar', avatarLimiter, avatarRouter);

// 404 fallback
app.use((req, res) => {
    res.status(404).json({ error: 'Not found' });
});

if (sentryDsn) {
    app.use(Sentry.Handlers.errorHandler());
}

// Global error handler
app.use((err, req, res, next) => {
    if (sentryDsn) {
        Sentry.captureException(err, { tags: { scope: 'express' } });
    }
    logger.error({ err }, 'Erro interno');
    res.status(err.status || 500).json({ error: err.message || 'Erro interno' });
});

// Start server only if not in test
if (process.env.NODE_ENV !== 'test') {
    const port = process.env.PORT || 3000;
    connectDatabase()
        .then(() => {
            if (!process.env.DISABLE_AVATAR_WORKER) {
                const queueMode = process.env.AVATAR_QUEUE_MODE || 'local';

                switch (queueMode) {
                    case 'rabbitmq':
                        startRabbitMQConsumer(processJobById, logger)
                            .then(() => logger.info('Worker RabbitMQ iniciado'))
                            .catch((err) => {
                                logger.error({ err }, 'Falha ao iniciar RabbitMQ, usando worker local');
                                startAvatarJobWorker(logger);
                            });
                        break;
                    case 'azure-queue':
                        startAzureQueueWorker(processJobById, logger)
                            .then(() => logger.info('Worker Azure Queue iniciado'))
                            .catch((err) => {
                                logger.error({ err }, 'Falha ao iniciar Azure Queue, usando worker local');
                                startAvatarJobWorker(logger);
                            });
                        break;
                    default:
                        startAvatarJobWorker(logger);
                        logger.info('Worker local iniciado');
                }
            }
            app.listen(port, () => logger.info(`API Bárbara ouvindo na porta ${port}`));
        })
        .catch((err) => {
            logger.error({ err }, 'Falha ao conectar MongoDB');
            logger.warn('⚠️  Iniciando servidor SEM MongoDB (modo desenvolvimento)');
            app.listen(port, () => logger.info(`API Bárbara ouvindo na porta ${port} (sem MongoDB)`));
        });
}

export default app;

import dotenv from 'dotenv';
import express from 'express';
import cors from 'cors';
import morgan from 'morgan';
import { connectDatabase } from './config/database.js';
import catalogRouter from './routes/catalog.js';
import avatarRouter from './routes/avatar.js';

dotenv.config();

const app = express();

// Middlewares
app.use(cors());
app.use(express.json({ limit: '1mb' }));
app.use(morgan('dev'));

// Healthcheck
app.get('/health', (req, res) => {
    res.json({ status: 'ok', timestamp: new Date().toISOString() });
});

// Routes
app.use('/catalog', catalogRouter);
app.use('/avatar', avatarRouter);

// 404 fallback
app.use((req, res) => {
    res.status(404).json({ error: 'Not found' });
});

// Global error handler
app.use((err, req, res, next) => {
    console.error('Erro interno:', err); // eslint-disable-line no-console
    res.status(err.status || 500).json({ error: err.message || 'Erro interno' });
});

// Start server only if not in test
if (process.env.NODE_ENV !== 'test') {
    const port = process.env.PORT || 3000;
    connectDatabase()
        .then(() => {
            app.listen(port, () => console.log(`API Bárbara ouvindo na porta ${port}`)); // eslint-disable-line no-console
        })
        .catch((err) => {
            console.error('Falha ao conectar MongoDB:', err); // eslint-disable-line no-console
            process.exit(1);
        });
}

export default app;

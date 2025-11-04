import express from 'express';
import Joi from 'joi';

const router = express.Router();

const avatarRequestSchema = Joi.object({
    userId: Joi.string().required(),
    frontImageUrl: Joi.string().uri().required(),
    sideImageUrl: Joi.string().uri().required()
});

// Placeholder store (em memória)
const avatars = new Map();

router.post('/generate', async (req, res) => {
    const { error, value } = avatarRequestSchema.validate(req.body, { abortEarly: false });
    if (error) return res.status(400).json({ errors: error.details });

    // Simulação de geração assíncrona
    const id = `${value.userId}-${Date.now()}`;
    const avatar = {
        id,
        userId: value.userId,
        status: 'processing',
        createdAt: new Date().toISOString()
    };
    avatars.set(id, avatar);

    // Em produção: disparar job (Fila / Worker / API Ready Player Me / Diffusion)
    setTimeout(() => {
        const done = avatars.get(id);
        if (done) {
            done.status = 'ready';
            done.glbUrl = `https://storage.example.com/avatars/${id}.glb`;
            avatars.set(id, done);
        }
    }, 1500);

    res.status(202).json({ requestId: id, status: 'queued' });
});

router.get('/:id', (req, res) => {
    const avatar = avatars.get(req.params.id);
    if (!avatar) return res.status(404).json({ error: 'Avatar não encontrado' });
    res.json(avatar);
});

export default router;

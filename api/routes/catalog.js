import express from 'express';
import Joi from 'joi';
import Product from '../models/Product.js';

const router = express.Router();

const productSchema = Joi.object({
    name: Joi.string().min(2).required(),
    sku: Joi.string().alphanum().min(3).required(),
    description: Joi.string().allow(''),
    category: Joi.string().allow(''),
    size: Joi.string().allow(''),
    color: Joi.string().allow(''),
    price: Joi.number().min(0).required(),
    images: Joi.array().items(Joi.string().uri()).default([]),
    model3dUrl: Joi.string().uri().allow(''),
    stock: Joi.number().integer().min(0).default(1),
    active: Joi.boolean().default(true)
});

// Create
router.post('/', async (req, res, next) => {
    try {
        const { error, value } = productSchema.validate(req.body, { abortEarly: false });
        if (error) return res.status(400).json({ errors: error.details });
        const product = await Product.create(value);
        res.status(201).json(product);
    } catch (err) {
        if (err.code === 11000) {
            return res.status(409).json({ error: 'SKU já existente' });
        }
        next(err);
    }
});

// List with optional search
router.get('/', async (req, res, next) => {
    try {
        const { q, category, limit = 20, page = 1 } = req.query;
        const query = {};
        if (category) query.category = category;
        if (q) query.$text = { $search: q };
        const skip = (Number(page) - 1) * Number(limit);
        const [items, total] = await Promise.all([
            Product.find(query).skip(skip).limit(Number(limit)).sort({ createdAt: -1 }),
            Product.countDocuments(query)
        ]);
        res.json({ items, total, page: Number(page), pages: Math.ceil(total / Number(limit)) });
    } catch (err) {
        next(err);
    }
});

// Get by id
router.get('/:id', async (req, res, next) => {
    try {
        const product = await Product.findById(req.params.id);
        if (!product) return res.status(404).json({ error: 'Produto não encontrado' });
        res.json(product);
    } catch (err) {
        next(err);
    }
});

// Update
router.put('/:id', async (req, res, next) => {
    try {
        const { error, value } = productSchema.validate(req.body, { abortEarly: false });
        if (error) return res.status(400).json({ errors: error.details });
        const product = await Product.findByIdAndUpdate(req.params.id, value, { new: true });
        if (!product) return res.status(404).json({ error: 'Produto não encontrado' });
        res.json(product);
    } catch (err) {
        next(err);
    }
});

// Delete
router.delete('/:id', async (req, res, next) => {
    try {
        const product = await Product.findByIdAndDelete(req.params.id);
        if (!product) return res.status(404).json({ error: 'Produto não encontrado' });
        res.json({ success: true });
    } catch (err) {
        next(err);
    }
});

export default router;

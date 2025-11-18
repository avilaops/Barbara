import request from 'supertest';
import app from '../server.js';
import Product from '../models/Product.js';

describe('Catálogo', () => {
    it('cria um produto válido', async () => {
        const payload = {
            name: 'Vestido Aurora',
            sku: 'VEST123',
            description: 'Vestido com tecidos leves',
            category: 'Vestidos',
            size: 'M',
            color: '#C29AFF',
            price: 189.9,
            images: ['https://example.com/aurora.png']
        };

        const response = await request(app).post('/catalog').send(payload);
        expect(response.statusCode).toBe(201);
        expect(response.body).toHaveProperty('sku', payload.sku);

        const saved = await Product.findOne({ sku: payload.sku });
        expect(saved).not.toBeNull();
    });

    it('impede criação com SKU duplicado', async () => {
        await Product.create({
            name: 'Saia Breeze',
            sku: 'SAIA123',
            price: 99.9
        });

        const res = await request(app)
            .post('/catalog')
            .send({ name: 'Saia Breeze II', sku: 'SAIA123', price: 109.9 });

        expect(res.statusCode).toBe(409);
        expect(res.body).toHaveProperty('error', 'SKU já existente');
    });

    it('lista produtos com paginação', async () => {
        await Product.create([
            { name: 'Camisa', sku: 'CAM001', price: 79.9 },
            { name: 'Calça', sku: 'CAL001', price: 129.5 }
        ]);

        const res = await request(app).get('/catalog?limit=1&page=1');
        expect(res.statusCode).toBe(200);
        expect(res.body).toHaveProperty('items');
        expect(res.body.items.length).toBe(1);
        expect(res.body).toMatchObject({ total: 2, page: 1, pages: 2 });
    });
});

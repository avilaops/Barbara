import mongoose from 'mongoose';

const MAX_RETRIES = 5;
let retries = 0;

export async function connectDatabase() {
    const uri = process.env.MONGODB_URI;
    if (!uri) {
        throw new Error('MONGODB_URI não definido no ambiente');
    }

    try {
        await mongoose.connect(uri, {
            serverSelectionTimeoutMS: 8000,
            maxPoolSize: 10
        });
        console.log('MongoDB conectado'); // eslint-disable-line no-console
        return mongoose.connection;
    } catch (err) {
        if (err?.code === 429 || /Rate/i.test(err.message)) {
            const delay = 1000 * Math.pow(2, retries);
            if (retries < MAX_RETRIES) {
                retries += 1;
                console.warn(`Retry conexão MongoDB (${retries}) aguardando ${delay}ms`); // eslint-disable-line no-console
                await new Promise(r => setTimeout(r, delay));
                return connectDatabase();
            }
        }
        throw err;
    }
}

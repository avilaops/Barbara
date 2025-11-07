import { MongoMemoryServer } from 'mongodb-memory-server';
import mongoose from 'mongoose';
import { connectDatabase } from '../config/database.js';

let mongoServer;

beforeAll(async () => {
    mongoServer = await MongoMemoryServer.create();
    process.env.MONGODB_URI = mongoServer.getUri();
    process.env.NODE_ENV = 'test';
    process.env.DISABLE_AVATAR_WORKER = 'true';
    await connectDatabase();
});

afterEach(async () => {
    if (mongoose.connection.readyState === 1) {
        await mongoose.connection.db.dropDatabase();
    }
});

afterAll(async () => {
    await mongoose.connection.close();
    if (mongoServer) {
        await mongoServer.stop();
    }
});

import amqp from 'amqplib';

let connection = null;
let channel = null;

const QUEUE_NAME = process.env.AVATAR_QUEUE_NAME || 'barbara-avatar-jobs';
const RABBITMQ_URL = process.env.RABBITMQ_URL || 'amqp://localhost';

export async function connectRabbitMQ() {
    if (connection && channel) {
        return { connection, channel };
    }

    try {
        connection = await amqp.connect(RABBITMQ_URL);
        channel = await connection.createChannel();
        await channel.assertQueue(QUEUE_NAME, { durable: true });

        connection.on('error', (err) => {
            console.error('RabbitMQ connection error:', err);
            connection = null;
            channel = null;
        });

        connection.on('close', () => {
            console.warn('RabbitMQ connection closed');
            connection = null;
            channel = null;
        });

        return { connection, channel };
    } catch (err) {
        console.error('Failed to connect to RabbitMQ:', err);
        throw err;
    }
}

export async function enqueueAvatarJob(jobId) {
    const { channel: ch } = await connectRabbitMQ();
    const message = JSON.stringify({ jobId, enqueuedAt: new Date().toISOString() });
    ch.sendToQueue(QUEUE_NAME, Buffer.from(message), { persistent: true });
}

export async function startAvatarConsumer(processor, logger) {
    const { channel: ch } = await connectRabbitMQ();

    ch.prefetch(1);

    ch.consume(QUEUE_NAME, async (msg) => {
        if (!msg) return;

        try {
            const { jobId } = JSON.parse(msg.content.toString());
            logger?.info({ jobId }, 'Processando job da fila RabbitMQ');
            await processor(jobId, logger);
            ch.ack(msg);
        } catch (err) {
            logger?.error({ err }, 'Erro ao processar mensagem RabbitMQ');
            ch.nack(msg, false, false);
        }
    });

    logger?.info('RabbitMQ consumer iniciado');
}

export async function closeRabbitMQ() {
    if (channel) await channel.close();
    if (connection) await connection.close();
    connection = null;
    channel = null;
}

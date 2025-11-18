import { QueueServiceClient } from '@azure/storage-queue';

const QUEUE_NAME = process.env.AZURE_QUEUE_NAME || 'barbara-avatar-jobs';
let queueClient = null;

function getQueueClient() {
    if (queueClient) return queueClient;

    const connectionString = process.env.AZURE_STORAGE_CONNECTION_STRING;
    if (!connectionString) {
        throw new Error('AZURE_STORAGE_CONNECTION_STRING não está definida');
    }

    const queueServiceClient = QueueServiceClient.fromConnectionString(connectionString);
    queueClient = queueServiceClient.getQueueClient(QUEUE_NAME);
    return queueClient;
}

export async function ensureQueue() {
    const client = getQueueClient();
    await client.createIfNotExists();
}

export async function enqueueAvatarJob(jobId) {
    const client = getQueueClient();
    const message = JSON.stringify({ jobId, enqueuedAt: new Date().toISOString() });
    const encodedMessage = Buffer.from(message).toString('base64');
    await client.sendMessage(encodedMessage);
}

export async function dequeueAvatarJob() {
    const client = getQueueClient();
    const response = await client.receiveMessages({ numberOfMessages: 1, visibilityTimeout: 30 });

    if (response.receivedMessageItems.length === 0) {
        return null;
    }

    const message = response.receivedMessageItems[0];
    const decodedText = Buffer.from(message.messageText, 'base64').toString('utf-8');
    const data = JSON.parse(decodedText);

    return {
        jobId: data.jobId,
        messageId: message.messageId,
        popReceipt: message.popReceipt
    };
}

export async function deleteMessage(messageId, popReceipt) {
    const client = getQueueClient();
    await client.deleteMessage(messageId, popReceipt);
}

export async function startAzureQueueWorker(processor, logger) {
    await ensureQueue();

    const pollInterval = Number(process.env.AZURE_QUEUE_POLL_MS || 5000);

    setInterval(async () => {
        try {
            const item = await dequeueAvatarJob();
            if (!item) return;

            logger?.info({ jobId: item.jobId }, 'Processando job da fila Azure Queue');

            try {
                await processor(item.jobId, logger);
                await deleteMessage(item.messageId, item.popReceipt);
            } catch (err) {
                logger?.error({ err, jobId: item.jobId }, 'Erro ao processar job Azure Queue');
            }
        } catch (err) {
            logger?.error({ err }, 'Erro no polling Azure Queue');
        }
    }, pollInterval);

    logger?.info({ pollInterval }, 'Azure Queue worker iniciado');
}

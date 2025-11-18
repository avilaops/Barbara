import {
    pullNextQueuedJob,
    markJobCompleted,
    markJobFailed
} from '../services/avatarJobs.js';
import { processAvatarJob } from '../services/avatarProcessor.js';

let timerHandle = null;
let isProcessing = false;

export function startAvatarJobWorker(logger) {
    if (timerHandle) {
        logger?.info('Avatar worker já está em execução.');
        return;
    }

    const pollInterval = Number(process.env.AVATAR_WORKER_POLL_MS || 3000);
    timerHandle = setInterval(async () => {
        if (isProcessing) {
            return;
        }
        isProcessing = true;

        try {
            const job = await pullNextQueuedJob();
            if (!job) {
                return;
            }

            logger?.info({ jobId: job.id, provider: job.provider }, 'Processando job de avatar');

            try {
                const result = await processAvatarJob(job, logger);
                await markJobCompleted(job.id, result);
                logger?.info({ jobId: job.id }, 'Job de avatar concluído');
            } catch (err) {
                await markJobFailed(job.id, err);
                logger?.error({ err, jobId: job.id }, 'Job de avatar falhou');
            }
        } catch (err) {
            logger?.error({ err }, 'Loop do avatar worker falhou');
        } finally {
            isProcessing = false;
        }
    }, pollInterval);

    logger?.info({ pollInterval }, 'Avatar worker iniciado');
}

export function stopAvatarJobWorker(logger) {
    if (timerHandle) {
        clearInterval(timerHandle);
        timerHandle = null;
        logger?.info('Avatar worker parado');
    }
}

export function isAvatarWorkerRunning() {
    return Boolean(timerHandle);
}

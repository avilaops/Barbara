import {
    getAvatarJobById,
    markJobCompleted,
    markJobFailed
} from '../services/avatarJobs.js';
import { processAvatarJob } from '../services/avatarProcessor.js';

export async function processJobById(jobId, logger) {
    const job = await getAvatarJobById(jobId);
    if (!job) {
        logger?.warn({ jobId }, 'Job não encontrado');
        return;
    }

    if (job.status !== 'queued' && job.status !== 'processing') {
        logger?.info({ jobId, status: job.status }, 'Job já foi processado');
        return;
    }

    logger?.info({ jobId, provider: job.provider }, 'Processando job de avatar');

    try {
        const result = await processAvatarJob(job, logger);
        await markJobCompleted(job.id, result);
        logger?.info({ jobId }, 'Job de avatar concluído');
    } catch (err) {
        await markJobFailed(job.id, err);
        logger?.error({ err, jobId }, 'Job de avatar falhou');
        throw err;
    }
}

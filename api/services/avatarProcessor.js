import { generateMockAvatar } from './providers/mock.js';
import { generateReadyPlayerMe } from './providers/readyPlayerMe.js';
import { generateTryOnDiffusion } from './providers/tryOnDiffusion.js';

function resolveProvider(job) {
    return (job.provider || process.env.AVATAR_PROVIDER || 'mock').toLowerCase();
}

export async function processAvatarJob(job, logger) {
    const provider = resolveProvider(job);

    try {
        switch (provider) {
            case 'ready-player-me':
                return await generateReadyPlayerMe(job);
            case 'tryon-diffusion':
                return await generateTryOnDiffusion(job);
            default:
                return await generateMockAvatar(job);
        }
    } catch (err) {
        logger?.error({ err, jobId: job.id }, `Provider ${provider} falhou, usando mock provider`);
        const fallback = await generateMockAvatar(job);
        fallback.metadata = {
            ...(fallback.metadata || {}),
            fallbackReason: err.message,
            requestedProvider: provider
        };
        return fallback;
    }
}

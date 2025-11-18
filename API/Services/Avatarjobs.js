import AvatarJob from '../models/AvatarJob.js';

export async function createAvatarJob(payload) {
    const job = await AvatarJob.create(payload);
    return job.toObject();
}

export async function getAvatarJobById(id) {
    const job = await AvatarJob.findById(id);
    return job ? job.toObject() : null;
}

export async function pullNextQueuedJob() {
    const job = await AvatarJob.findOneAndUpdate(
        { status: 'queued' },
        { status: 'processing', startedAt: new Date() },
        { sort: { createdAt: 1 }, new: true }
    );
    return job ? job.toObject() : null;
}

export async function markJobCompleted(id, data) {
    await AvatarJob.findByIdAndUpdate(id, {
        status: 'ready',
        finishedAt: new Date(),
        glbUrl: data?.glbUrl,
        metadata: data?.metadata,
        error: undefined
    });
}

export async function markJobFailed(id, error) {
    await AvatarJob.findByIdAndUpdate(id, {
        status: 'failed',
        finishedAt: new Date(),
        error: typeof error === 'string' ? error : error?.message
    });
}

export async function listJobsByUser(userId, { limit = 20, page = 1 } = {}) {
    const skip = (Number(page) - 1) * Number(limit);
    const [docs, total] = await Promise.all([
        AvatarJob.find({ userId })
            .sort({ createdAt: -1 })
            .skip(skip)
            .limit(Number(limit)),
        AvatarJob.countDocuments({ userId })
    ]);

    const items = docs.map((doc) => doc.toObject());

    return {
        items,
        total,
        page: Number(page),
        pages: Math.ceil(total / Number(limit)) || 1
    };
}

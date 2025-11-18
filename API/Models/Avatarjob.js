import mongoose from 'mongoose';

const AvatarJobSchema = new mongoose.Schema(
    {
        userId: { type: String, required: true, index: true },
        frontImageUrl: { type: String, required: true },
        sideImageUrl: { type: String, required: true },
        provider: { type: String, enum: ['mock', 'ready-player-me', 'tryon-diffusion'], default: 'mock' },
        status: {
            type: String,
            enum: ['queued', 'processing', 'ready', 'failed'],
            default: 'queued',
            index: true
        },
        glbUrl: { type: String },
        error: { type: String },
        metadata: { type: mongoose.Schema.Types.Mixed },
        startedAt: { type: Date },
        finishedAt: { type: Date }
    },
    { timestamps: true }
);

AvatarJobSchema.index({ createdAt: 1, status: 1 });

function transform(doc, ret) {
    ret.id = ret._id.toString();
    delete ret._id;
    delete ret.__v;
    return ret;
}

AvatarJobSchema.set('toJSON', { virtuals: true, transform });
AvatarJobSchema.set('toObject', { virtuals: true, transform });

export default mongoose.models.AvatarJob || mongoose.model('AvatarJob', AvatarJobSchema);

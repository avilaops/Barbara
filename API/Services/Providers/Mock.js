export async function generateMockAvatar(job) {
    const baseUrl = process.env.ASSETS_BASE_URL || 'https://storage.example.com/avatars';
    return {
        glbUrl: `${baseUrl}/${job._id || job.id}.glb`,
        metadata: {
            provider: 'mock',
            message: 'Avatar gerado via mock provider. Configure READY_PLAYER_ME ou TRYON_DIFFUSION para geração real.'
        }
    };
}

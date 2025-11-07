import axios from 'axios';

export async function generateTryOnDiffusion(job) {
    const {
        TRYON_DIFFUSION_ENDPOINT: endpoint,
        TRYON_DIFFUSION_TOKEN: token
    } = process.env;

    if (!endpoint || !token) {
        throw new Error('Configuração TryOn Diffusion incompleta. Defina TRYON_DIFFUSION_ENDPOINT e TRYON_DIFFUSION_TOKEN.');
    }

    try {
        const response = await axios.post(
            endpoint,
            {
                inputs: {
                    front_image_url: job.frontImageUrl,
                    side_image_url: job.sideImageUrl,
                    user_id: job.userId
                }
            },
            {
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                timeout: 120000
            }
        );

        const data = response.data || {};
        const glbUrl = data?.output?.glb_url || data?.glbUrl || data?.resultUrl;
        if (!glbUrl) {
            throw new Error('Resposta TryOn Diffusion não contém URL do avatar.');
        }

        return {
            glbUrl,
            metadata: {
                provider: 'tryon-diffusion',
                requestId: data?.id || data?.requestId || null
            }
        };
    } catch (err) {
        const detail = err.response?.data?.error || err.response?.data?.message || err.message;
        throw new Error(`TryOn Diffusion falhou: ${detail}`);
    }
}

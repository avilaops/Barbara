import axios from 'axios';

export async function generateReadyPlayerMe(job) {
    const {
        READY_PLAYER_ME_API_KEY: apiKey,
        READY_PLAYER_ME_APP_ID: appId,
        READY_PLAYER_ME_BASE_URL: baseUrl = 'https://api.readyplayer.me/v2/avatars'
    } = process.env;

    if (!apiKey || !appId) {
        throw new Error('Credenciais do Ready Player Me não configuradas. Defina READY_PLAYER_ME_APP_ID e READY_PLAYER_ME_API_KEY.');
    }

    try {
        const payload = {
            appId,
            images: [job.frontImageUrl, job.sideImageUrl].filter(Boolean),
            meta: {
                userId: job.userId
            }
        };

        const response = await axios.post(`${baseUrl}/generate`, payload, {
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${apiKey}`
            },
            timeout: 60000
        });

        const data = response.data || {};
        const glbUrl = data?.avatarUrl || data?.glb || data?.data?.attributes?.urls?.glb;
        if (!glbUrl) {
            throw new Error('Resposta Ready Player Me não contém URL do avatar.');
        }

        return {
            glbUrl,
            metadata: {
                provider: 'ready-player-me',
                requestId: data?.requestId || data?.id || null
            }
        };
    } catch (err) {
        const detail = err.response?.data?.error || err.response?.data?.message || err.message;
        throw new Error(`Ready Player Me falhou: ${detail}`);
    }
}

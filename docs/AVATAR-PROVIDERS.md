# 🔧 Configuração dos Provedores de Avatar

## Ready Player Me

Ready Player Me é um serviço de geração de avatares 3D personalizados.

### Obter Credenciais

1. Acesse [readyplayer.me/developers](https://readyplayer.me/developers)
2. Crie uma conta ou faça login
3. Crie um novo aplicativo
4. Copie o **App ID** e a **API Key**

### Configurar no .env

```properties
AVATAR_PROVIDER=ready-player-me
READY_PLAYER_ME_APP_ID=seu_app_id_aqui
READY_PLAYER_ME_API_KEY=sua_api_key_aqui
READY_PLAYER_ME_BASE_URL=https://api.readyplayer.me/v2/avatars
```

### Formato da Resposta

A API do Ready Player Me retorna:

```json
{
  "id": "avatar_id",
  "avatarUrl": "https://models.readyplayer.me/avatar_id.glb",
  "status": "completed"
}
```

### Ajustes Necessários

O provider em `api/services/providers/readyPlayerMe.js` espera que a resposta contenha:
- `avatarUrl` ou `glb` ou `data.attributes.urls.glb`

Se o formato real da API for diferente, ajuste a lógica de extração do `glbUrl`.

---

## TryOn Diffusion / Hugging Face

### Opção 1: Hugging Face Inference API

1. Acesse [huggingface.co](https://huggingface.co)
2. Gere um token de acesso em **Settings → Access Tokens**
3. Escolha um modelo de try-on (ex: `yisol/IDM-VTON`)

### Configurar no .env

```properties
AVATAR_PROVIDER=tryon-diffusion
TRYON_DIFFUSION_ENDPOINT=https://api-inference.huggingface.co/models/yisol/IDM-VTON
TRYON_DIFFUSION_TOKEN=seu_hf_token_aqui
```

### Opção 2: Replicate

1. Acesse [replicate.com](https://replicate.com)
2. Crie uma conta e obtenha uma API key
3. Use um modelo de virtual try-on disponível

```properties
AVATAR_PROVIDER=tryon-diffusion
TRYON_DIFFUSION_ENDPOINT=https://api.replicate.com/v1/predictions
TRYON_DIFFUSION_TOKEN=seu_replicate_token_aqui
```

### Formato da Resposta

O provider em `api/services/providers/tryOnDiffusion.js` espera:
- `output.glb_url` ou `glbUrl` ou `resultUrl`

Ajuste conforme o formato real do serviço escolhido.

---

## Testando os Provedores

### 1. Atualizar .env

Escolha um provider e configure as credenciais correspondentes.

### 2. Reiniciar o servidor

```bash
cd api
npm run dev
```

### 3. Criar um job de teste

```bash
curl -X POST http://localhost:3000/avatar/generate \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "test-user",
    "frontImageUrl": "https://example.com/front.jpg",
    "sideImageUrl": "https://example.com/side.jpg",
    "provider": "ready-player-me"
  }'
```

### 4. Verificar o status

```bash
curl http://localhost:3000/avatar/{requestId}
```

Se o provider falhar, o sistema usará automaticamente o mock provider como fallback.

---

## Debugging

### Logs

Os logs do worker mostram tentativas e falhas:

```
INFO: Processando job de avatar jobId=xxx provider=ready-player-me
ERROR: Provider ready-player-me falhou, usando mock provider
```

### Testar manualmente

Você pode testar o processamento de um job específico:

```javascript
import { processAvatarJob } from './services/avatarProcessor.js';
import { getAvatarJobById } from './services/avatarJobs.js';

const job = await getAvatarJobById('job_id');
const result = await processAvatarJob(job, console);
console.log(result);
```

---

## Próximos Passos

1. ✅ Configure credenciais reais
2. ⏳ Ajuste extração de `glbUrl` conforme formato da API
3. ⏳ Teste com imagens reais
4. ⏳ Configure storage permanente (Azure Blob/S3) para os GLBs gerados

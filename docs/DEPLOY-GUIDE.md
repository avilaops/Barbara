# 🚀 Guia de Deploy Completo - Projeto Bárbara

## ✅ Pré-requisitos Implementados

Todas as funcionalidades principais foram implementadas e testadas:

### Backend
- ✅ API REST com Express + MongoDB
- ✅ Sistema de jobs de avatar persistido
- ✅ Provedores configuráveis (mock, Ready Player Me, TryOnDiffusion)
- ✅ Worker interno + suporte para RabbitMQ e Azure Queue
- ✅ Rate limiting e segurança (Helmet, CORS configurável)
- ✅ Logging estruturado (Pino) + Sentry opcional
- ✅ Testes automatizados (Jest + MongoDB in-memory)

### Frontend Unity
- ✅ Scripts de comunicação com API
- ✅ Loader de GLB em runtime
- ✅ UI manager para navegação
- ✅ Aplicação de roupas em avatar

### DevOps
- ✅ Docker + Docker Compose
- ✅ Ngrok para exposição pública
- ✅ GitHub Actions (CI para API + build Unity WebGL)

---

## 📋 Checklist de Deploy

### 1. Configurar Banco de Dados MongoDB

**Opção A: MongoDB Atlas (Recomendado)**

1. Acesse [mongodb.com/cloud/atlas](https://www.mongodb.com/cloud/atlas)
2. Crie um cluster gratuito (M0)
3. Configure acesso de rede:
   - IP Whitelist: `0.0.0.0/0` (todos) ou IPs específicos
4. Crie um usuário do banco
5. Copie a connection string

**Configurar no .env:**
```properties
MONGODB_URI=mongodb+srv://usuario:senha@cluster.mongodb.net/barbara?retryWrites=true&w=majority
```

### 2. Configurar Provedores de Avatar

Escolha um ou mais provedores:

#### Opção A: Mock (Teste)
```properties
AVATAR_PROVIDER=mock
ASSETS_BASE_URL=https://storage.example.com/avatars
```

#### Opção B: Ready Player Me
1. Cadastre-se em [readyplayer.me/developers](https://readyplayer.me/developers)
2. Crie um app e copie credenciais
3. Configure:
```properties
AVATAR_PROVIDER=ready-player-me
READY_PLAYER_ME_APP_ID=seu_app_id
READY_PLAYER_ME_API_KEY=sua_api_key
```

#### Opção C: TryOn Diffusion (Hugging Face)
1. Obtenha token em [huggingface.co/settings/tokens](https://huggingface.co/settings/tokens)
2. Configure:
```properties
AVATAR_PROVIDER=tryon-diffusion
TRYON_DIFFUSION_ENDPOINT=https://api-inference.huggingface.co/models/yisol/IDM-VTON
TRYON_DIFFUSION_TOKEN=seu_hf_token
```

**Documentação completa:** `docs/AVATAR-PROVIDERS.md`

### 3. Escolher Modo de Fila

#### Desenvolvimento/MVP: Local
```properties
AVATAR_QUEUE_MODE=local
```

#### Produção - RabbitMQ
```bash
# Docker local
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

```properties
AVATAR_QUEUE_MODE=rabbitmq
RABBITMQ_URL=amqp://localhost
```

#### Produção - Azure Queue
```properties
AVATAR_QUEUE_MODE=azure-queue
AZURE_STORAGE_CONNECTION_STRING=DefaultEndpointsProtocol=https;AccountName=...
```

**Documentação completa:** `docs/QUEUE-SETUP.md`

### 4. Configurar Observabilidade (Opcional)

#### Sentry
1. Crie projeto em [sentry.io](https://sentry.io)
2. Copie o DSN
```properties
SENTRY_DSN=https://xxx@xxx.ingest.sentry.io/xxx
SENTRY_ENVIRONMENT=production
```

### 5. Deploy do Backend

#### Opção A: Docker (Recomendado)

**Local com Ngrok:**
```powershell
.\docker.ps1 up
.\docker.ps1 ngrok-url
```

**Produção (Azure Container Instances):**
```bash
# Build imagem
docker build -t barbara-api ./api

# Tag para ACR
docker tag barbara-api shancrysacrdevhp7owg.azurecr.io/barbara-api:latest

# Push
docker push shancrysacrdevhp7owg.azurecr.io/barbara-api:latest

# Deploy
az container create \
  --resource-group barbara-rg \
  --name barbara-api \
  --image shancrysacrdevhp7owg.azurecr.io/barbara-api:latest \
  --registry-username shancrysacrdevhp7owg \
  --registry-password <ACR_PASSWORD> \
  --dns-name-label barbara-api \
  --ports 3000 \
  --environment-variables \
    MONGODB_URI=$MONGODB_URI \
    AVATAR_PROVIDER=mock \
    NODE_ENV=production
```

#### Opção B: Azure App Service

```bash
# Criar App Service
az webapp up \
  --name barbara-api \
  --resource-group barbara-rg \
  --runtime "NODE:20-lts" \
  --sku B1

# Configurar variáveis
az webapp config appsettings set \
  --name barbara-api \
  --resource-group barbara-rg \
  --settings \
    MONGODB_URI=$MONGODB_URI \
    AVATAR_PROVIDER=mock
```

### 6. Deploy do Frontend Unity

#### Build WebGL Local
1. Abra Unity Editor
2. File → Build Settings → WebGL → Build
3. Escolha pasta de saída

#### Deploy Azure Static Web Apps

```bash
# Via GitHub Actions (já configurado em .github/workflows/unity-webgl.yml)
# Requer secrets:
# - UNITY_LICENSE
# - UNITY_EMAIL  
# - UNITY_PASSWORD

# Ou manual:
az staticwebapp create \
  --name barbara-frontend \
  --resource-group barbara-rg \
  --source ./build/WebGL \
  --location "East US 2"
```

### 7. Configurar Unity GLB Loader

1. Abra Unity Editor
2. Window → Package Manager
3. Add package from git URL: `https://github.com/Siccity/GLTFUtility.git`
4. O script `Assets/Editor/DefineSymbolsSetup.cs` configura automaticamente o símbolo `GLTF_UTILITY`

**Documentação completa:** `docs/UNITY-GLB-SETUP.md`

### 8. Configurar CORS e URLs

**Backend (.env):**
```properties
ALLOWED_ORIGINS=https://seu-frontend.azurestaticapps.net,http://localhost:8080
```

**Unity (APIClient.cs):**
```csharp
[SerializeField] private string baseUrl = "https://barbara-api.azurecontainerapps.io";
```

### 9. Testar Integração Completa

#### 1. Testar API Backend
```bash
curl https://barbara-api.azurecontainerapps.io/health
```

#### 2. Criar Job de Avatar
```bash
curl -X POST https://barbara-api.azurecontainerapps.io/avatar/generate \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "test-user",
    "frontImageUrl": "https://example.com/front.jpg",
    "sideImageUrl": "https://example.com/side.jpg"
  }'
```

#### 3. Verificar Status
```bash
curl https://barbara-api.azurecontainerapps.io/avatar/{requestId}
```

#### 4. Testar Unity WebGL
- Abra o frontend no navegador
- Teste fluxo completo: catálogo → try-on → visualização

---

## 🔒 Segurança em Produção

### Checklist Final

- [ ] Trocar todas as senhas/tokens do `.env`
- [ ] Remover `ALLOWED_ORIGINS=*` (definir domínios específicos)
- [ ] Configurar HTTPS (Azure fornece automaticamente)
- [ ] Ativar rate limiting personalizado se necessário
- [ ] Configurar backup automático do MongoDB
- [ ] Configurar alertas no Sentry/Azure Monitor
- [ ] Revisar permissões de Storage/ACR
- [ ] Habilitar autenticação JWT (próximo passo)

---

## 📊 Monitoramento

### Logs
```bash
# Container local
docker logs barbara-api -f

# Azure
az container logs --name barbara-api --resource-group barbara-rg --follow
```

### Métricas
- RabbitMQ: `http://localhost:15672` (guest/guest)
- Azure Monitor: Portal Azure → Resource → Metrics
- Sentry: Dashboard de erros

### Health Check
```bash
curl https://barbara-api.azurecontainerapps.io/health
```

---

## 🚨 Troubleshooting

### API não responde
1. Verificar logs: `docker logs barbara-api`
2. Verificar MongoDB connection string
3. Testar localmente: `npm run dev`

### Worker não processa jobs
1. Verificar modo de fila: `AVATAR_QUEUE_MODE`
2. Para RabbitMQ: verificar `RABBITMQ_URL`
3. Logs devem mostrar: "Worker [modo] iniciado"

### Unity não carrega GLB
1. Verificar Console do navegador (F12)
2. Verificar CORS do storage
3. Testar URL do GLB diretamente no navegador
4. Verificar `GLTF_UTILITY` está definido

### Build WebGL falha no GitHub Actions
1. Verificar secrets do Unity estão configurados
2. Verificar licença Unity é válida
3. Testar build local primeiro

---

## 🎯 Próximos Passos Recomendados

1. **Autenticação**: Implementar JWT para proteger rotas
2. **Webhooks**: Receber callbacks dos provedores de IA
3. **CDN**: Configurar Azure CDN para assets estáticos
4. **Analytics**: Integrar Google Analytics 4
5. **Backup**: Configurar backup automático MongoDB
6. **CI/CD**: Expandir pipelines com testes E2E
7. **Escalabilidade**: Migrar para Kubernetes (AKS) se necessário

---

## 📚 Documentação Adicional

- `docs/AVATAR-PROVIDERS.md` - Configuração de provedores de IA
- `docs/QUEUE-SETUP.md` - Setup de filas (RabbitMQ/Azure Queue)
- `docs/UNITY-GLB-SETUP.md` - Configuração do Unity GLB loader
- `docs/DOCKER.md` - Guia Docker completo
- `core/README.md` - Documentação Unity
- `README.md` - Visão geral do projeto

---

**🎉 Parabéns! O projeto Bárbara está pronto para deploy em produção!**

# 🚀 Deploy Bárbara - Guia Completo

## 📋 Pré-requisitos

- [ ] Conta Azure ativa
- [ ] Azure CLI instalado
- [ ] Git configurado
- [ ] Secrets configurados no GitHub
- [ ] Unity License (para build WebGL)

---

## 🔐 1. Configurar Secrets no GitHub

Vá em: **Settings → Secrets and variables → Actions → New repository secret**

### Backend (Azure Web App)
```
AZURE_WEBAPP_PUBLISH_PROFILE
```
- Obter em: Azure Portal → App Service → Get Publish Profile

### Frontend (Azure Static Web Apps)
```
AZURE_STATIC_WEB_APPS_API_TOKEN
```
- Obter em: Azure Portal → Static Web Apps → Manage deployment token

### Unity Build
```
UNITY_LICENSE
UNITY_EMAIL
UNITY_PASSWORD
```
- Obter license: Unity → Account → License Management

### Variáveis de Ambiente (Azure Portal)
```
MONGODB_URI
HUGGINGFACE_API_KEY
AZURE_STORAGE_CONNECTION_STRING
AZURE_QUEUE_NAME
JWT_SECRET
REPLICATE_API_TOKEN (opcional)
```

---

## 🌐 2. Deploy do Backend (Azure App Service)

### Via Azure Portal (Recomendado)

1. **Criar App Service:**
```bash
az webapp create \
  --name barbara-api \
  --resource-group barbara-rg \
  --plan barbara-plan \
  --runtime "NODE:20-lts"
```

2. **Configurar Variáveis:**
```bash
az webapp config appsettings set \
  --name barbara-api \
  --resource-group barbara-rg \
  --settings \
    NODE_ENV=production \
    MONGODB_URI="<sua-connection-string>" \
    HUGGINGFACE_API_KEY="<sua-key>"
```

3. **Deploy via Git:**
```bash
cd api
git remote add azure https://barbara-api.scm.azurewebsites.net:443/barbara-api.git
git push azure main:master
```

### Via GitHub Actions (Automático)

1. **Commit e Push:**
```bash
git add .
git commit -m "feat: configure azure deploy"
git push origin main
```

2. **Verificar Actions:**
- GitHub → Actions → Ver workflow rodando

3. **Verificar Deploy:**
- https://barbara-api.azurewebsites.net/health

---

## 🎮 3. Deploy do Frontend Unity (Azure Static Web Apps)

### Build Local (Teste)

1. **Abrir Unity:**
```
File → Build Settings → WebGL → Build
```

2. **Testar Local:**
```bash
cd build/WebGL/barbara-webgl
python -m http.server 8000
# Abrir: http://localhost:8000
```

### Deploy Automático (GitHub Actions)

O workflow já está configurado! Ao fazer push:

1. **Unity build roda no CI/CD**
2. **Deploy automático para Azure Static Web Apps**
3. **URL gerada**: https://barbara.azurestaticapps.net

---

## 🐳 4. Deploy com Docker (Alternativa)

### Build e Push para Azure Container Registry

```bash
# Login no Azure
az acr login --name barbararegistry

# Build e push da API
cd api
docker build -t barbararegistry.azurecr.io/barbara-api:latest .
docker push barbararegistry.azurecr.io/barbara-api:latest

# Deploy no Azure Container Instances
az container create \
  --name barbara-api-container \
  --resource-group barbara-rg \
  --image barbararegistry.azurecr.io/barbara-api:latest \
  --dns-name-label barbara-api \
  --ports 3000 \
  --environment-variables \
    NODE_ENV=production \
    MONGODB_URI="<connection-string>"
```

---

## 🚀 5. Deploy Rápido (Railway/Vercel)

### Railway (Recomendado para Backend)

1. **Instalar Railway CLI:**
```bash
npm i -g @railway/cli
railway login
```

2. **Deploy:**
```bash
cd api
railway init
railway up
```

3. **Configurar Variáveis:**
```bash
railway variables set MONGODB_URI="<connection-string>"
railway variables set HUGGINGFACE_API_KEY="<key>"
```

4. **Obter URL:**
```bash
railway domain
# Exemplo: https://barbara-api-production.up.railway.app
```

### Vercel (Alternativa)

1. **Instalar Vercel CLI:**
```bash
npm i -g vercel
vercel login
```

2. **Deploy:**
```bash
cd api
vercel --prod
```

---

## 📊 6. Monitoramento

### Azure Application Insights

```bash
# Habilitar monitoring
az monitor app-insights component create \
  --app barbara-api-insights \
  --location eastus \
  --resource-group barbara-rg
```

### Logs em Tempo Real

```bash
# Azure
az webapp log tail --name barbara-api --resource-group barbara-rg

# Railway
railway logs

# Docker local
docker-compose logs -f api
```

---

## ✅ 7. Checklist de Deploy

### Backend
- [ ] App Service criado no Azure
- [ ] Variáveis de ambiente configuradas
- [ ] MongoDB Atlas acessível (whitelist IP do Azure)
- [ ] Health check respondendo: `/health`
- [ ] CORS configurado para frontend domain
- [ ] Rate limiting ativo
- [ ] HTTPS habilitado

### Frontend
- [ ] Unity build WebGL testado localmente
- [ ] Static Web App criado
- [ ] API backend URL configurada no Unity
- [ ] Compression habilitada
- [ ] CDN configurado (opcional)
- [ ] Custom domain configurado (opcional)

### CI/CD
- [ ] GitHub Actions workflow configurado
- [ ] Secrets adicionados no GitHub
- [ ] Testes passando
- [ ] Deploy automático funcionando

### Segurança
- [ ] Secrets em variáveis de ambiente (não no código)
- [ ] HTTPS em todos endpoints
- [ ] Rate limiting configurado
- [ ] CORS restrito ao domínio do frontend
- [ ] Helmet.js ativo
- [ ] MongoDB Atlas IP whitelist configurado

---

## 🌍 8. URLs Finais

Após deploy completo:

```
✅ API Backend:  https://barbara-api.azurewebsites.net
✅ Health Check: https://barbara-api.azurewebsites.net/health
✅ Frontend:     https://barbara.azurestaticapps.net
✅ Admin Panel:  https://barbara.azurestaticapps.net/admin (futuro)
```

---

## 🐛 Troubleshooting

### Erro: "Cannot find module"
```bash
# Rebuild node_modules
cd api
rm -rf node_modules package-lock.json
npm install
```

### Erro: "MongoDB connection failed"
```bash
# Verificar IP whitelist no Atlas
# Adicionar 0.0.0.0/0 (todos IPs) ou IP específico do Azure
```

### Erro: "Unity build failed"
```bash
# Verificar Unity version
# Verificar se todos assets estão commitados
# Verificar se Unity License está válida
```

### Erro: "CORS blocked"
```bash
# Atualizar CORS no backend (api/server.js)
# Adicionar domínio do frontend
```

---

## 💰 Custos Estimados (Azure)

### Free Tier (Desenvolvimento)
```
App Service (F1):         $0/mês
Static Web Apps:          $0/mês (até 100GB bandwidth)
MongoDB Atlas (M0):       $0/mês
Total:                    $0/mês ✅
```

### Production Tier
```
App Service (B1):         ~$13/mês
Static Web Apps (Free):   $0/mês
MongoDB Atlas (M10):      ~$57/mês
Azure Queue Storage:      ~$0.10/mês
Total:                    ~$70/mês
```

---

## 🚦 Status do Deploy

### Desenvolvimento
```
✅ Backend local rodando (Docker)
✅ MongoDB Atlas conectado
✅ TryOn Diffusion configurado
✅ Frontend Unity pronto
⏳ Deploy pendente
```

### Produção
```
⏳ Azure App Service (aguardando)
⏳ Azure Static Web Apps (aguardando)
⏳ CI/CD configurado (aguardando secrets)
⏳ Custom domain (futuro)
```

---

## 📞 Próximos Passos

1. **Escolher plataforma de deploy:**
   - [ ] Azure (completo, recomendado)
   - [ ] Railway (rápido, simples)
   - [ ] Vercel (frontend only)

2. **Configurar secrets no GitHub**
3. **Fazer primeiro deploy (push to main)**
4. **Testar endpoints em produção**
5. **Configurar custom domain (opcional)**

---

**🎉 Pronto para deploy!**

*Guia criado em: 6 de novembro de 2025*
*Versão: 1.0.0*

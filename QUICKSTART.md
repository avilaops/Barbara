# 🚀 Guia Rápido - Iniciando o Projeto Bárbara

## ⚠️ Pré-requisito: Docker Desktop

### 1. Iniciar Docker Desktop

**O erro atual** indica que o Docker Desktop não está rodando.

**Solução:**
1. Abra o **Docker Desktop** no Windows
2. Aguarde até o ícone na bandeja do sistema ficar verde ✅
3. Teste se está funcionando:
   ```powershell
   docker ps
   ```

---

## 🚀 Iniciando o Projeto

### Opção 1: Docker Compose (Recomendado)

```powershell
# 1. Subir containers
docker-compose up -d

# 2. Ver status
docker-compose ps

# 3. Ver logs
docker-compose logs -f

# 4. Ver URL do Ngrok
Start-Process "http://localhost:4040"

# 5. Parar
docker-compose down
```

### Opção 2: Sem Docker (Desenvolvimento Local)

```powershell
# 1. Instalar dependências
cd api
npm install

# 2. Iniciar API
npm run dev

# 3. Em outro terminal, testar
.\test-api.ps1
```

---

## 🧪 Testando a API

### Método 1: Script PowerShell (Recomendado)

```powershell
.\test-api.ps1
```

Este script irá:
- ✅ Testar health check
- ✅ Criar job de avatar
- ✅ Verificar status
- ✅ Listar jobs
- ✅ Testar rate limiting
- ✅ Testar catálogo

### Método 2: Comandos Individuais

**Health Check:**
```powershell
Invoke-RestMethod -Uri "http://localhost:3000/health" -Method Get | ConvertTo-Json
```

**Criar Avatar Job:**
```powershell
$body = @{
    userId = "teste-usuario"
    frontImageUrl = "https://example.com/front.jpg"
    sideImageUrl = "https://example.com/side.jpg"
} | ConvertTo-Json

$response = Invoke-RestMethod `
    -Uri "http://localhost:3000/avatar/generate" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body

Write-Host "Request ID: $($response.requestId)"
```

**Verificar Status (substitua {requestId}):**
```powershell
$requestId = "seu-request-id-aqui"
Invoke-RestMethod -Uri "http://localhost:3000/avatar/$requestId" | ConvertTo-Json
```

**Listar Jobs do Usuário:**
```powershell
Invoke-RestMethod -Uri "http://localhost:3000/avatar?userId=teste-usuario" | ConvertTo-Json
```

**Criar Produto no Catálogo:**
```powershell
$product = @{
    sku = "SHIRT-001"
    name = "Camiseta Básica"
    description = "Camiseta 100% algodão"
    price = 49.90
    category = "roupas"
    imageUrl = "https://example.com/shirt.jpg"
    model3dUrl = "https://example.com/shirt.glb"
} | ConvertTo-Json

Invoke-RestMethod `
    -Uri "http://localhost:3000/catalog" `
    -Method Post `
    -ContentType "application/json" `
    -Body $product | ConvertTo-Json
```

**Listar Produtos:**
```powershell
Invoke-RestMethod -Uri "http://localhost:3000/catalog?limit=10" | ConvertTo-Json
```

---

## 📊 Monitoramento

### Ver Logs do Docker
```powershell
# Todos os logs
docker-compose logs -f

# Apenas API
docker-compose logs -f api

# Últimas 100 linhas
docker-compose logs --tail=100 api
```

### Interface Web do Ngrok
```powershell
Start-Process "http://localhost:4040"
```

### Azure Queue Monitor
```powershell
# Via Portal Azure
Start-Process "https://portal.azure.com"

# Ir para: barbarastoragequeue → Queues → barbara-avatar-jobs
```

---

## 🔧 Troubleshooting

### Docker não inicia

**Problema:** `open //./pipe/dockerDesktopLinuxEngine: The system cannot find the file`

**Solução:**
1. Abra Docker Desktop
2. Aguarde inicialização completa
3. Teste: `docker ps`

### API não responde

```powershell
# Verificar se container está rodando
docker ps

# Se não estiver, subir
docker-compose up -d

# Ver logs de erro
docker-compose logs api
```

### Azure Queue não processa

```powershell
# Verificar connection string
Get-Content .env | Select-String "AZURE_STORAGE"

# Verificar modo da fila
Get-Content .env | Select-String "AVATAR_QUEUE_MODE"
```

### TryOn Diffusion timeout

- Modelo pode estar "frio" (primeira execução)
- Aguarde até 2-3 minutos
- Verifique logs: `docker-compose logs -f api`

---

## 📚 Próximos Passos

1. ✅ **Iniciar Docker Desktop**
2. ✅ **Subir containers:** `docker-compose up -d`
3. ✅ **Testar API:** `.\test-api.ps1`
4. ⏳ **Configurar Sentry:** https://sentry.io/signup/
5. ⏳ **Abrir Unity e testar GLB loader**
6. ⏳ **Deploy em produção**

---

## 🆘 Ajuda

- **Documentação Completa:** `CONFIGURACOES-APLICADAS.md`
- **Guia de Deploy:** `docs/DEPLOY-GUIDE.md`
- **Provedores de Avatar:** `docs/AVATAR-PROVIDERS.md`
- **Configuração de Filas:** `docs/QUEUE-SETUP.md`
- **Unity GLB Setup:** `docs/UNITY-GLB-SETUP.md`

---

**🎉 Tudo configurado e pronto para usar!**

*Última atualização: 6 de novembro de 2025*

# 🐳 Docker + Ngrok - Projeto Bárbara

Este documento explica como rodar o projeto Bárbara usando Docker com exposição pública via ngrok.

## 📋 Pré-requisitos

- Docker Desktop instalado
- Docker Compose instalado
- Token ngrok configurado no `.env`

## 🚀 Início Rápido

### Usando o script PowerShell (Recomendado)

```powershell
# Iniciar todos os serviços
.\docker.ps1 up

# Ver logs em tempo real
.\docker.ps1 logs

# Obter URL pública do ngrok
.\docker.ps1 ngrok-url

# Parar serviços
.\docker.ps1 down
```

### Usando Docker Compose diretamente

```bash
# Construir e iniciar
docker-compose up -d

# Ver logs
docker-compose logs -f

# Parar
docker-compose down
```

## 🌐 Acessando a API

### Localmente

- **API**: <http://localhost:3000>
- **Health**: <http://localhost:3000/health>
- **Ngrok Dashboard**: <http://localhost:4040>

### Publicamente

Após iniciar os containers, obtenha a URL pública:

```powershell
.\docker.ps1 ngrok-url
```

Ou acesse manualmente: <http://localhost:4040/status>

## 🏗️ Estrutura dos Containers

### barbara-api

- Porta: 3000
- Ambiente: Production
- Healthcheck: Ativo
- Volume: ./api/logs

### barbara-ngrok

- Porta Web UI: 4040
- Túnel: Expõe porta 3000 da API
- Configuração: ngrok.yml

## 📊 Comandos Úteis

```powershell
# Status dos containers
.\docker.ps1 status

# Rebuild completo
.\docker.ps1 build

# Reiniciar serviços
.\docker.ps1 restart

# Limpar tudo (cuidado!)
.\docker.ps1 clean
```

## 🔧 Configuração

### Variáveis de Ambiente (.env)

```properties
ALLOWED_ORIGINS=http://localhost:8080,http://localhost:3000
MONGODB_URI=sua_conexao_mongodb
AVATAR_PROVIDER=mock
READY_PLAYER_ME_APP_ID=
READY_PLAYER_ME_API_KEY=
READY_PLAYER_ME_BASE_URL=https://api.readyplayer.me/v2/avatars
TRYON_DIFFUSION_ENDPOINT=
TRYON_DIFFUSION_TOKEN=
ASSETS_BASE_URL=https://storage.example.com/avatars
NGROK_TOKEN=seu_token_aqui
OPENAI_API_KEY=sua_chave_openai
SENTRY_DSN=
SENTRY_ENVIRONMENT=production
# ... outras variáveis
```

### Ngrok Custom Domain (Opcional)

Edite `ngrok.yml`:

```yaml
tunnels:
  barbara-api:
    hostname: "seu-dominio.ngrok.io"
```

## 🐛 Troubleshooting

### API não responde

```powershell
docker-compose logs api
```

### Ngrok não conecta

```powershell
docker-compose restart ngrok
docker-compose logs ngrok
```

### Resetar tudo

```powershell
.\docker.ps1 clean
.\docker.ps1 build
.\docker.ps1 up
```

## 📱 Testando a API Pública

Após obter a URL do ngrok:

```bash
# Healthcheck
curl https://seu-url.ngrok.io/health

# Listar catálogo
curl https://seu-url.ngrok.io/catalog
```

## 🔒 Segurança

⚠️ **Importante**: A URL do ngrok é pública! Para produção:

1. Configure autenticação JWT
2. Use domínio ngrok reservado
3. Ative rate limiting
4. Configure CORS adequadamente
5. Use HTTPS apenas

## 💡 Dicas

- Use `.\docker.ps1 ngrok-url` para obter a URL sempre que os containers reiniciarem
- A URL do ngrok muda a cada restart (exceto se usar domínio reservado)
- Acesse <http://localhost:4040> para ver estatísticas de requests em tempo real
- Logs persistem em `./api/logs`

## 🆘 Suporte

Para mais ajuda, consulte:

- [Docker Docs](https://docs.docker.com/)
- [Ngrok Docs](https://ngrok.com/docs)
- [README principal](../README.md)

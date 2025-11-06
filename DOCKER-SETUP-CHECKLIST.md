# ✅ Checklist de Configuração Docker + Ngrok

## Pré-requisitos

- [ ] Docker Desktop instalado e rodando
- [ ] Token ngrok configurado
- [ ] Arquivo `.env` configurado com todas as variáveis

## Arquivos Criados

- [x] `api/Dockerfile` - Imagem Docker da API
- [x] `docker-compose.yml` - Orquestração de containers
- [x] `ngrok.yml` - Configuração do ngrok
- [x] `docker.ps1` - Script PowerShell de gerenciamento
- [x] `docker.bat` - Script batch com menu interativo
- [x] `.dockerignore` - Arquivos a ignorar no build
- [x] `.env.example` - Exemplo de configuração

## Passo a Passo

### 1. Verificar Docker

```powershell
docker --version
docker-compose --version
```

### 2. Verificar .env

```powershell
cat .env | Select-String "NGROK_TOKEN"
cat .env | Select-String "MONGODB_URI"
```

### 3. Iniciar Containers

```powershell
.\docker.ps1 up
```

Ou use o menu:

```cmd
docker.bat
```

### 4. Verificar Status

```powershell
.\docker.ps1 status
```

Esperado:

```
barbara-api    Up (healthy)
barbara-ngrok  Up
```

### 5. Obter URL Pública

```powershell
.\docker.ps1 ngrok-url
```

### 6. Testar API Local

```powershell
curl http://localhost:3000/health
```

### 7. Testar API Pública

```powershell
# Substitua pela URL obtida no passo 5
curl https://sua-url.ngrok.io/health
```

### 8. Ver Dashboard Ngrok

Abra no navegador: <http://localhost:4040>

## Comandos Úteis

```powershell
# Ver logs em tempo real
.\docker.ps1 logs

# Reiniciar containers
.\docker.ps1 restart

# Parar tudo
.\docker.ps1 down

# Rebuild completo
.\docker.ps1 build
.\docker.ps1 up
```

## Troubleshooting

### Erro: "Cannot connect to Docker daemon"

- Inicie o Docker Desktop
- Aguarde aparecer o ícone verde

### Erro: "Port already in use"

```powershell
# Parar processos Node.js
Get-Process -Name node | Stop-Process -Force

# Parar containers
.\docker.ps1 down
```

### Ngrok não conecta

```powershell
# Verificar token
cat .env | Select-String "NGROK_TOKEN"

# Reiniciar apenas ngrok
docker-compose restart ngrok
docker-compose logs ngrok
```

### MongoDB não conecta

- Verifique IP whitelist no MongoDB Atlas
- Confirme MONGODB_URI no .env
- Teste conexão: `docker-compose logs api | Select-String "MongoDB"`

## Próximos Passos

- [ ] Testar endpoints do catálogo
- [ ] Configurar Unity para usar API pública
- [ ] Adicionar autenticação JWT
- [ ] Configurar CI/CD
- [ ] Deploy para produção

## Recursos

- [Docker Docs](https://docs.docker.com/)
- [Ngrok Docs](https://ngrok.com/docs)
- [DOCKER.md](./docs/DOCKER.md) - Documentação completa
- [DOCKER-QUICKSTART.md](./DOCKER-QUICKSTART.md) - Guia rápido

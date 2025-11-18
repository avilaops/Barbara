# 🚀 Guia Rápido - Docker + Ngrok

## Iniciar o Projeto

```powershell
# 1. Iniciar containers
.\docker.ps1 up

# 2. Aguardar 10 segundos

# 3. Obter URL pública
.\docker.ps1 ngrok-url
```

## Resultado Esperado

```
✨ URL Pública da API Bárbara:
   https://abc123.ngrok.io

📋 Interface Web do ngrok:
   http://localhost:4040
```

## Testar API

```bash
# Substitua pela sua URL do ngrok
curl https://abc123.ngrok.io/health
```

## Comandos Principais

| Comando | Descrição |
|---------|-----------|
| `.\docker.ps1 up` | Iniciar tudo |
| `.\docker.ps1 ngrok-url` | Ver URL pública |
| `.\docker.ps1 logs` | Ver logs |
| `.\docker.ps1 down` | Parar tudo |
| `.\docker.ps1 status` | Ver status |

## Acessos

- 🌐 **API Local**: <http://localhost:3000>
- 🔗 **Ngrok Dashboard**: <http://localhost:4040>
- 🌍 **API Pública**: Execute `.\docker.ps1 ngrok-url`

---

Para mais detalhes, veja [DOCKER.md](./DOCKER.md)

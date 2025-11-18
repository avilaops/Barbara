# Bárbara - AI-Powered Virtual Fitting Room 👗

Sistema de prova virtual de roupas com avatares 3D personalizados e inteligência artificial.

## 🚀 Stack Tecnológica

### Backend
- **.NET 9.0** - Framework principal
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM
- **Clean Architecture** - Estrutura em camadas

### Databases
- **SQL Server 2022 Express** - Dados relacionais (gratuito)
- **MongoDB 7.0** - Avatares e assets
- **Redis 7** - Cache e sessões

### Storage & Infrastructure
- **MinIO** - Object storage S3-compatible
- **Traefik v3** - Reverse proxy + SSL automático
- **Docker Compose** - Orquestração de containers

### AI & ML
- **OpenAI GPT-4** - Assistente virtual
- **Ready Player Me** - Geração de avatares
- **TryOn Diffusion** - Virtual try-on
- **Hugging Face** - Modelos de ML

### Monitoring
- **Prometheus** - Métricas
- **Grafana** - Dashboards
- **Loki** - Logs centralizados
- **Promtail** - Collector de logs

### DevOps
- **GitHub Actions** - CI/CD
- **GitHub Container Registry** - Docker images
- **OneDrive** - Backups automatizados

## 📦 Estrutura do Projeto

```
Barbara/
├── Src/
│   ├── Barbara.Domain/        # Entidades e interfaces
│   ├── Barbara.Application/   # Casos de uso e DTOs
│   ├── Barbara.Infrastructure/# Implementações (DB, APIs)
│   ├── Barbara.API/           # Controllers e endpoints
│   └── Barbara.Web/           # Frontend Blazor
├── docker-compose.avila-full.yml  # Stack completa
├── Dockerfile.api             # Container API
├── Dockerfile.web             # Container Web
├── monitoring/                # Configs Prometheus/Grafana/Loki
└── scripts/                   # Automação e backups

```

## 🛠️ Quick Start

### Pré-requisitos
- Docker & Docker Compose
- .NET 9.0 SDK (para desenvolvimento local)
- Git

### 1. Clone o repositório
```bash
git clone https://github.com/avilaops/Barbara.git
cd Barbara
```

### 2. Configure as variáveis de ambiente
```bash
cp .env.example .env.production
# Edite .env.production com suas credenciais
```

### 3. Inicie a stack completa
```bash
docker-compose -f docker-compose.avila-full.yml up -d
```

### 4. Acesse os serviços

| Serviço | URL | Descrição |
|---------|-----|-----------|
| **Bárbara Web** | https://barbara.avila.inc | Interface principal |
| **API** | https://barbara.avila.inc/api | REST API |
| **Auth** | https://auth.avila.inc | Autenticação centralizada |
| **Grafana** | https://grafana.barbara.avila.inc | Dashboards |
| **Prometheus** | https://metrics.barbara.avila.inc | Métricas |
| **MinIO** | https://storage.barbara.avila.inc | Object storage |
| **Traefik** | https://traefik.barbara.avila.inc | Proxy dashboard |

**Credenciais padrão:**
- Grafana: `admin` / (definido em `.env.production`)
- MinIO: `minioadmin` / (definido em `.env.production`)

## 🔧 Desenvolvimento Local

### Rodar apenas o backend (API)
```bash
cd Src/Barbara.API
dotnet run
```

### Rodar apenas o frontend (Blazor)
```bash
cd Src/Barbara.Web
dotnet run
```

### Rodar testes
```bash
dotnet test
```

## 🐳 Docker

### Build manual das imagens
```bash
# API
docker build -t ghcr.io/avilaops/barbara-api:latest -f Dockerfile.api .

# Web
docker build -t ghcr.io/avilaops/barbara-web:latest -f Dockerfile.web .
```

### Push para GitHub Container Registry
```bash
docker push ghcr.io/avilaops/barbara-api:latest
docker push ghcr.io/avilaops/barbara-web:latest
```

## 🔐 Configuração do Auth Service

Bárbara usa o serviço centralizado `auth.avila.inc` (FastAPI) para autenticação.

**Endpoints:**
- `POST /auth/register` - Registrar usuário
- `POST /auth/login` - Login (envia MFA por email)
- `POST /auth/mfa/verify` - Verificar código MFA e receber JWT

## 📊 Monitoring

### Prometheus Metrics
Acesse `https://metrics.barbara.avila.inc` para ver:
- Requisições HTTP
- Latência de APIs
- Uso de CPU/Memória
- Erros e exceções

### Grafana Dashboards
Acesse `https://grafana.barbara.avila.inc` para:
- Visualizar métricas em tempo real
- Configurar alertas
- Analisar logs com Loki

### Logs Centralizados
Todos os logs são coletados pelo Promtail e enviados para Loki:
```bash
# Ver logs do container API
docker logs barbara-api -f

# Ver logs no Grafana
# Explore > Loki > {container="barbara-api"}
```

## 💾 Backups

Backup automático para OneDrive (30 dias de retenção):

```bash
bash scripts/backup-to-onedrive.sh
```

**O que é feito backup:**
- SQL Server database
- MongoDB collections
- Redis snapshots
- MinIO buckets
- Configurações

## 🚀 Deploy para Produção

### GitHub Actions
O projeto possui workflows automatizados:

1. **CI** - Testa e builda a cada push
2. **Deploy** - Cria e publica Docker images (manual)

Para fazer deploy:
```bash
# Via GitHub Actions
gh workflow run deploy.yml

# Ou manual
docker-compose -f docker-compose.avila-full.yml pull
docker-compose -f docker-compose.avila-full.yml up -d
```

## 🌐 DNS Configuration (Cloudflare)

Configure os seguintes registros A/CNAME apontando para seu servidor:

```
barbara.avila.inc           → SEU_IP_SERVIDOR
auth.avila.inc              → SEU_IP_SERVIDOR
grafana.barbara.avila.inc   → SEU_IP_SERVIDOR
metrics.barbara.avila.inc   → SEU_IP_SERVIDOR
storage.barbara.avila.inc   → SEU_IP_SERVIDOR
traefik.barbara.avila.inc   → SEU_IP_SERVIDOR
```

O Traefik gerará certificados SSL automaticamente via Let's Encrypt.

## 📈 Arquitetura

```
┌─────────────────────────────────────────────────────────┐
│                     Cloudflare DNS                      │
│              barbara.avila.inc (Proxy ON)               │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
         ┌───────────────────────┐
         │   Traefik (Port 443)  │
         │   SSL/TLS Auto Cert   │
         └───────────┬───────────┘
                     │
        ┌────────────┼────────────┐
        │            │            │
        ▼            ▼            ▼
   ┌────────┐  ┌─────────┐  ┌────────┐
   │  Web   │  │   API   │  │  Auth  │
   │ Blazor │  │  .NET   │  │FastAPI │
   └────────┘  └────┬────┘  └────────┘
                    │
        ┌───────────┼───────────┐
        │           │           │
        ▼           ▼           ▼
   ┌──────────┐ ┌─────────┐ ┌───────┐
   │SQL Server│ │ MongoDB │ │ Redis │
   └──────────┘ └─────────┘ └───────┘
```

## 🤝 Contribuindo

1. Fork o repositório
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📝 License

MIT License - veja o arquivo [LICENSE](LICENSE) para detalhes.

## 👥 Equipe

**Avila Inc** - [avila.inc](https://avila.inc)
- **Desenvolvedor Principal:** Nicolas Rosa
- **Email:** dev@avila.inc

## 🔗 Links Úteis

- [Documentação completa](./Documentation/)
- [API Reference](https://barbara.avila.inc/swagger)
- [Roadmap](https://github.com/avilaops/Barbara/issues)
- [Changelog](https://github.com/avilaops/Barbara/releases)

---

**Bárbara** - Transformando a experiência de compra de roupas online com IA 🚀✨

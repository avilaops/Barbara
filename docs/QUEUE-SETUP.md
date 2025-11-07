# 🚀 Configuração de Filas Externas

O sistema de avatar suporta três modos de fila:

1. **Local** (padrão): worker em memória na mesma instância
2. **RabbitMQ**: fila distribuída para múltiplas instâncias
3. **Azure Queue Storage**: fila gerenciada no Azure

## Modo Local (Padrão)

```properties
AVATAR_QUEUE_MODE=local
```

- ✅ Sem dependências externas
- ✅ Ideal para desenvolvimento e baixo volume
- ❌ Não escala horizontalmente
- ❌ Jobs são perdidos se o processo cai

## RabbitMQ

### Instalação Local

```bash
# Docker
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management

# Ou via Chocolatey (Windows)
choco install rabbitmq
```

### Configuração

```properties
AVATAR_QUEUE_MODE=rabbitmq
RABBITMQ_URL=amqp://localhost
AVATAR_QUEUE_NAME=barbara-avatar-jobs
```

### Deploy em Produção

**CloudAMQP (Managed RabbitMQ):**
```properties
RABBITMQ_URL=amqps://usuario:senha@host.cloudamqp.com/vhost
```

**Azure Container Instances:**
```bash
az container create \
  --resource-group barbara-rg \
  --name rabbitmq \
  --image rabbitmq:3-management \
  --ports 5672 15672 \
  --environment-variables \
    RABBITMQ_DEFAULT_USER=admin \
    RABBITMQ_DEFAULT_PASS=senha_segura
```

### Vantagens

- ✅ Escalabilidade horizontal (múltiplos workers)
- ✅ Persistência de mensagens
- ✅ Dead letter queues para retry
- ✅ Dashboard de monitoramento (porta 15672)

## Azure Queue Storage

### Configuração

1. Crie uma Storage Account no Azure Portal
2. Copie a connection string em **Access Keys**

```properties
AVATAR_QUEUE_MODE=azure-queue
AZURE_STORAGE_CONNECTION_STRING=DefaultEndpointsProtocol=https;AccountName=...
AZURE_QUEUE_NAME=barbara-avatar-jobs
AZURE_QUEUE_POLL_MS=5000
```

### Vantagens

- ✅ Totalmente gerenciado pelo Azure
- ✅ Escalabilidade automática
- ✅ Integração nativa com serviços Azure
- ✅ Pay-per-use (muito barato)

## Escolhendo o Modo Certo

| Cenário | Recomendação |
|---------|-------------|
| Desenvolvimento local | **Local** |
| MVP com 1 servidor | **Local** |
| Produção com múltiplas instâncias | **RabbitMQ** ou **Azure Queue** |
| Já usa Azure | **Azure Queue** |
| Precisa de retry complexo | **RabbitMQ** |

## Testando

### Criar job

```bash
curl -X POST http://localhost:3000/avatar/generate \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "test",
    "frontImageUrl": "https://example.com/front.jpg",
    "sideImageUrl": "https://example.com/side.jpg"
  }'
```

### Verificar processamento

```bash
# Local: logs do servidor
# RabbitMQ: http://localhost:15672 (guest/guest)
# Azure: Azure Portal → Storage Account → Queues
```

## Troubleshooting

### RabbitMQ não conecta

```bash
# Verificar se está rodando
docker ps | grep rabbitmq

# Ver logs
docker logs rabbitmq
```

### Azure Queue não funciona

1. Verificar connection string
2. Confirmar que a fila existe (é criada automaticamente)
3. Verificar permissões da Storage Account

## Próximos Passos

1. ✅ Escolha o modo de fila apropriado
2. ✅ Configure variáveis de ambiente
3. ⏳ Teste com jobs reais
4. ⏳ Configure monitoramento (Dashboard RabbitMQ ou Azure Monitor)
5. ⏳ Implemente retry policies customizadas

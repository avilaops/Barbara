# api.backend-agent.md

Objetivo: Evoluir o backend Node.js + Express + MongoDB da plataforma Bárbara.

## Escopo Atual

- CRUD de produtos (`/catalog`).
- Placeholder de geração de avatar (`/avatar/generate`).
- Healthcheck (`/health`).

## Próximas Features

- Autenticação JWT para usuários.
- Rate limiting (ex: express-rate-limit) para rotas de avatar.
- Integração com serviço de fila (RabbitMQ ou Azure Queue) para jobs de geração.
- Webhooks de status de IA.

## Padrões

- Respostas JSON padronizadas.
- Validação com Joi.
- Logs estruturados (futuro: pino + correl. ID).

## Estrutura

```
api/
  routes/
  models/
  config/
  tests/
```

## Checklist Futuro

- [ ] Implementar autenticação.
- [ ] Implementar rota de busca avançada (filtros múltiplos).
- [ ] Adicionar índice composto em Product.
- [ ] Observabilidade (Sentry ou OpenTelemetry).

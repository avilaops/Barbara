# api.backend-agent.md

Objetivo: Evoluir o backend Node.js + Express + MongoDB da plataforma Bárbara.

## Escopo Atual

- CRUD de produtos (`/catalog`).
- Placeholder de geração de avatar (`/avatar/generate`).
- Healthcheck (`/health`).

## Próximas Features

- Autenticação JWT para usuários.
- Persistir fila externa (RabbitMQ/Azure Queue) para distribuir jobs entre múltiplas instâncias.
- Webhooks de status de IA (receber callbacks dos provedores e atualizar jobs).
- Observabilidade distribuída (OpenTelemetry traces, métricas personalizadas).

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

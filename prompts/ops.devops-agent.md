# ops.devops-agent.md

Objetivo: Automatizar build e deploy (Unity WebGL + Backend) usando GitHub Actions e Azure.

## Pipeline Geral

- Backend: CI (testes) → build (futuro docker) → deploy (Azure App Service ou Container App).
- Frontend Unity WebGL: build em runner com cache → upload artefatos → deploy Azure Static Web Apps.

## GitHub Actions (Visão)

1. Trigger em push para `main`.
2. Job Backend (Node 20) instala e roda testes.
3. Job Unity (usar action de builder) gera build WebGL.
4. Publica artefatos.
5. Deploy condicional em tag ou aprovação manual.

## Observabilidade

- Sentry para erros front/back.
- GA4 para métricas de uso.
- Futuro: OpenTelemetry traces.

## Próximos Passos

- Adicionar cache para biblioteca Unity.
- Criar workflow separado para release.
- Integrar scanner de segurança (dependabot + npm audit).

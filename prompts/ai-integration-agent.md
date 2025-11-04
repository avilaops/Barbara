# ai-integration-agent.md

Objetivo: Coordenar integração entre Unity, serviços de IA (Ready Player Me, Diffusion) e backend.

## Pipeline Ideal

1. Upload imagens (frontal/lateral) para storage seguro.
2. Criação de job de geração de avatar.
3. Chamada API Ready Player Me ou modelo local (Diffusion fine-tuned).
4. Pós-processamento (limpeza mesh, normal maps, otimização).
5. Publicação do `.glb`/texturas em Blob Storage.
6. Atualização de estado via webhook no backend.

## Modelos Sugeridos

- TryOnDiffusion
- FashionVTON
- OutfitAnyone

## Considerações

- Limitar tamanho de imagens.
- Utilizar fila para evitar sobrecarga.
- Versionar modelos (tag semântica ex: v-avatar-1.0.0).

## Próximos Passos

- Definir contrato JSON para job de geração.
- Mapear tempos médios de resposta e SLAs.

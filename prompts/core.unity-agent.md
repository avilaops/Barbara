# core.unity-agent.md

Objetivo: Auxiliar na criação e evolução do projeto Unity (URP / WebGL) para o provador virtual Bárbara.

## Tarefas Principais

- Inicializar projeto Unity 2022+ com URP.
- Configurar cena principal `MainScene` com iluminação básica e plano neutro.
- Integrar carregamento de modelos `.glb` vindos do backend (catálogo).
- Implementar `AvatarManager` para substituir avatar base pelo personalizado.
- Implementar `TryOnController` para aplicação de roupas (troca de mesh/material).
- Implementar `CatalogLoader` para consumir endpoint `/catalog`.
- Implementar `UIManager` para menus (Avatar | Catálogo | Loja).

## Diretrizes Técnicas

- Usar Addressables para carregamento assíncrono de assets.
- Padronizar scripts em C# com namespace `Barbara.Core`.
- Evitar GameObjects órfãos; organizar hierarquia em `_SceneRoot`.
- Preparar build WebGL (compressão Brotli, DataCaching ativado).

## Próximos Passos

1. Criar estrutura inicial de cena.
2. Implementar carregamento de avatar placeholder.
3. Integrar chamada REST ao backend.
4. Ajustar pipeline de build para GitHub Actions.

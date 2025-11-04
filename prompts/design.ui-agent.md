# design.ui-agent.md

Objetivo: Definir e manter diretrizes visuais e UX da plataforma Bárbara.

## Princípios

- Minimalismo funcional.
- Feedback rápido (hover, foco, estados de carregamento).
- Paleta: Lilás #C29AFF, branco, preto suave #111111.
- Tipografia: Poppins (titles), Inter (body).

## Componentes-Chave

- Sidebar: navegação entre Avatar | Catálogo | Loja.
- Botão primário translúcido com blur leve.
- Cards de produto com preview 2D + indicador 3D.
- Overlay de try-on com estado de processamento.

## Acessibilidade

- Contraste mínimo WCAG AA.
- Foco visível em todos os elementos interativos.
- Textos alternativos em imagens de produto.

## Microinterações

- Fade-in avatar carregado.
- Skeleton loading para catálogo.
- Progress spinner para geração de avatar.

## Próximos Passos

- Criar design tokens exportáveis.
- Mapear estados de erro e empty states.

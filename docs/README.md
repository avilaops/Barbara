# 👗 Bárbara — Plataforma de Moda Virtual com IA

Bárbara é uma **plataforma de provador virtual inteligente** que une moda, inteligência artificial e interação 3D.  
Seu propósito é permitir que qualquer pessoa visualize roupas reais em um **avatar digital personalizado**, conectando o catálogo do **Bazar Boa Sorte** a uma experiência imersiva e interativa.

---

## 📚 Documentação do Frontend Premium

### 🎯 Guias Completos Disponíveis:

1. **[FRONTEND-PREMIUM.md](./FRONTEND-PREMIUM.md)** - Visão geral completa do sistema UI
   - 7 componentes principais criados
   - Design system (cores, tipografia, espaçamentos)
   - Arquitetura e integrações
   - Performance e otimizações
   - Próximos passos e roadmap

2. **[FRONTEND-SETUP-GUIDE.md](./FRONTEND-SETUP-GUIDE.md)** - Implementação passo-a-passo
   - Checklist completo em 5 fases (2-3 horas)
   - Como criar todos os prefabs Unity
   - Configuração completa da Scene
   - Integração com backend
   - Testing e troubleshooting

3. **[FRONTEND-CODE-EXAMPLES.md](./FRONTEND-CODE-EXAMPLES.md)** - Exemplos práticos
   - Como usar cada componente
   - Fluxos completos (busca, compra, avatar)
   - Customizações visuais
   - Responsividade

**👉 Comece pelo FRONTEND-PREMIUM.md para entender a arquitetura!**

---

## 🧩 Identidade do Projeto

**Nome:** Bárbara  
**Função:** Plataforma virtual de moda e experimentação 3D  
**Slogan:** "Vista-se com inteligência."  
**Missão:** Conectar tecnologia, moda e personalização em uma só experiência.

---

## ⚙️ Arquitetura Técnica do Sistema Bárbara

| Camada | Tecnologia | Descrição |
|--------|-------------|------------|
| **Frontend 3D** | Unity 2022+ (URP / WebGL) | Interface principal com renderização 3D e física de roupas. |
| **Backend** | Node.js + Express + MongoDB Atlas | Gerencia catálogo, usuários e integração com APIs externas. |
| **IA** | TryOnDiffusion / Ready Player Me / Hugging Face | Geração de avatar e simulação de vestimenta. |
| **Armazenamento** | Firebase Storage / Azure Blob | Hospedagem de modelos `.glb` e texturas. |
| **DevOps** | GitHub Actions + Azure Static Web Apps | Build e deploy automatizado do sistema. |
| **Design/UI** | Figma + Fontsource + Font Awesome | Identidade visual, ícones e tipografia padronizada. |

---

## 🔄 Fluxo do Usuário

1. O usuário envia **duas fotos** (frontal e lateral).  
2. A IA gera um **avatar 3D personalizado**.  
3. O **catálogo do Bazar Boa Sorte** é carregado do backend.  
4. O usuário **seleciona roupas** e a IA aplica o “fit” automático.  
5. O usuário pode **visualizar, tirar captura ou comprar diretamente**.

---

## 🧠 Inteligência Artificial Integrável

| Tecnologia | Função |
|-------------|--------|
| **Ready Player Me** | Gera avatar 3D personalizado a partir de selfie. |
| **TryOnDiffusion (Replicate / Hugging Face)** | Aplica roupas do catálogo na imagem do avatar. |
| **Banuba SDK** | Tracking facial e corporal em tempo real. |
| **AI Foundry** | Orquestra e monitora pipelines de IA (recorte → vestir → exportar). |

**Hugging Face Hub:** armazena e executa modelos open-source como *FashionVTON* e *OutfitAnyone*.  
**AI Foundry:** garante versionamento e escalabilidade automática.

---

## 🧩 Estrutura de Pastas

Barbara/
│
├── core/ # Aplicação Unity (frontend 3D)
│ ├── Scenes/MainScene.unity
│ ├── Scripts/
│ │ ├── AvatarManager.cs
│ │ ├── CatalogLoader.cs
│ │ ├── TryOnController.cs
│ │ └── UIManager.cs
│ └── Assets/Models/
│ └── Catalog/
│
├── api/ # Servidor Node.js + Express
│ ├── routes/
│ │ ├── catalog.js
│ │ └── avatar.js
│ ├── models/Product.js
│ └── config/database.js
│
├── docs/ # Documentação técnica
└── prompts/ # Instruções das IAs do VSCode

---

## 🤖 Agentes de Desenvolvimento (VSCode AI Prompts)

| Arquivo | Função |
|----------|--------|
| `core.unity-agent.md` | Criação do projeto Unity e scripts principais. |
| `api.backend-agent.md` | Estrutura e endpoints do backend. |
| `ai-integration-agent.md` | Conexão entre Unity, IA e backend. |
| `ops.devops-agent.md` | CI/CD e deploy automatizado (Azure). |
| `design.ui-agent.md` | Diretrizes de design e interface. |

---

## 🎨 Design e Identidade Visual

- **Tema:** Fashion Tech + Metaverso  
- **Paleta:** Lilás `#C29AFF`, branco e preto suave `#111111`  
- **Fontes:** Poppins / Inter  
- **Estilo:** Minimalista, tridimensional leve, botões translúcidos  

**Elementos principais:**

- Menu lateral: Avatar | Catálogo | Loja  
- Botão principal: “Vestir no Avatar”  
- Área central: viewport 3D com fundo holográfico sutil  

---

## 💳 Integrações Externas

| Categoria | Aplicações | Função |
|------------|-------------|--------|
| **IA** | Hugging Face, Replicate, Ready Player Me | Geração e personalização de avatar. |
| **Infraestrutura** | Azure, Firebase, MongoDB Atlas | Hospedagem e banco de dados. |
| **E-commerce** | WhatsApp Business API, Shopify Lite | Conversão e venda direta. |
| **Design/UI** | Figma, Google Fonts, Font Awesome | Interface e branding. |
| **Observabilidade** | Google Analytics 4, Sentry | Métricas e monitoramento. |

---

## 📈 Objetivo Final

> **Bárbara** é mais que um provador virtual — é a fusão entre moda, tecnologia e personalização.  
> Cada componente (Unity, IA, API e design) foi projetado para tornar o ato de vestir **interativo, imersivo e inteligente.**

---

## 🧩 Próximos Passos

1. Criar repositório `barbara-ai` e importar este README.  
2. Gerar `core.unity-agent.md` e `api.backend-agent.md` no VSCode.  
3. Integrar IA (Ready Player Me + TryOnDiffusion).  
4. Publicar build inicial WebGL no Azure Static Web Apps.  

---

**© 2025 — Projeto Bárbara · Bazar Boa Sorte · Ávila Inc.**

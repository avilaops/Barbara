# Unity Project - Bárbara Frontend 3D

Este é o projeto Unity para o frontend 3D da plataforma Bárbara.

## 📋 Requisitos

- Unity 2022.3 LTS ou superior
- URP (Universal Render Pipeline)
- Suporte WebGL habilitado

## 🗂️ Estrutura

```plaintext
core/
├── Assets/
│   ├── Scenes/
│   │   └── MainScene.unity      # Cena principal
│   ├── Scripts/
│   │   ├── APIClient.cs         # Cliente HTTP para backend
│   │   ├── AvatarManager.cs     # Gerenciamento de avatar
│   │   ├── CatalogLoader.cs     # Carregamento de catálogo
│   │   ├── ProductCard.cs       # UI card de produto
│   │   ├── ProductData.cs       # Estrutura de dados
│   │   ├── TryOnController.cs   # Controle de vestimenta
│   │   └── UIManager.cs         # Gerenciador de UI
│   ├── Models/                  # Modelos 3D
│   ├── Prefabs/                 # Prefabs reutilizáveis
│   └── ...
├── Packages/
│   └── manifest.json            # Dependências do projeto
└── ProjectSettings/             # Configurações do projeto
```

## 🚀 Como Executar

### No Editor Unity

1. Abra o Unity Hub
2. Adicione o projeto apontando para a pasta `core/`
3. Abra o projeto (Unity 2022.3 LTS)
4. Abra a cena `Assets/Scenes/MainScene.unity`
5. Configure a URL do backend em `APIClient` (padrão: `http://localhost:3000`)
6. Pressione Play

### Build WebGL

#### Via Unity Editor

1. File → Build Settings
2. Selecione **WebGL** como plataforma
3. Clique em **Switch Platform**
4. Clique em **Build** e escolha pasta de saída
5. Sirva os arquivos com servidor HTTP (ex: `python -m http.server 8080`)

#### Via GitHub Actions

O workflow `.github/workflows/unity-webgl.yml` automatiza o build:

- Trigger: push para `main` com mudanças em `core/`
- Cache da biblioteca Unity para builds mais rápidos
- Artefato gerado disponível para download por 14 dias

**Segredos necessários no GitHub:**

- `UNITY_LICENSE` (arquivo .ulf de licença Unity)
- `UNITY_EMAIL` (email da conta Unity)
- `UNITY_PASSWORD` (senha da conta Unity)

## 🔌 Integração com Backend

O `APIClient.cs` comunica-se com a API Node.js:

- `GET /catalog` - Lista produtos
- `POST /avatar/generate` - Solicita geração de avatar
- `GET /avatar/:id` - Verifica status do avatar

Certifique-se de que o backend está rodando antes de testar no Unity.

## 📦 Dependências

Definidas em `Packages/manifest.json`:

- `com.unity.render-pipelines.universal` (URP)
- `com.unity.textmeshpro`
- `com.unity.ugui`
- `com.unity.modules.unitywebrequest`

**Opcional (para carregamento GLB):**

- GLTFUtility ou Siccity.GLTFUtility via Package Manager

## 🎨 Namespace

Todos os scripts estão no namespace `Barbara.Core`.

## 🧪 Testes

Atualmente não há testes automatizados Unity. Futuramente:

- Unity Test Framework (UTF)
- Play Mode tests para fluxos de UI
- Edit Mode tests para lógica de dados

## 🐛 Troubleshooting

### Erro de CORS ao chamar API

Configure o backend para aceitar requisições do domínio WebGL:

```javascript
app.use(cors({ origin: '*' })); // Em desenvolvimento
```

### Avatar não carrega

Verifique:

1. Backend está rodando
2. URL do `APIClient` está correta
3. Console do navegador (F12) para erros de rede

### Build WebGL muito grande

- Habilite compressão Brotli em Player Settings
- Remova assets não utilizados
- Use Addressables para carregamento dinâmico

## 📚 Próximos Passos

- [ ] Implementar carregamento real de GLB (GLTFUtility)
- [ ] Adicionar animações de transição
- [ ] Implementar sistema de cache de produtos
- [ ] Adicionar feedback visual de loading
- [ ] Integrar analytics (GA4)
- [ ] Otimizar shaders para WebGL

## 📄 Licença

© 2025 Projeto Bárbara · Bazar Boa Sorte · Ávila Inc.

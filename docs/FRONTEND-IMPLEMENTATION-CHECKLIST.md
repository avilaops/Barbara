# ✅ Checklist de Implementação - Frontend Bárbara

## 🎯 Use este arquivo para acompanhar seu progresso!

---

## 📋 FASE 1: FOUNDATION (Dias 1-7)

### Dia 1-2: Setup Inicial (4-6h)

#### Unity Project Setup
- [ ] Baixar e instalar Unity 2022.3 LTS
- [ ] Criar novo projeto 3D (URP)
- [ ] Configurar Unity para WebGL:
  - [ ] File → Build Settings → WebGL
  - [ ] Player Settings → Resolution: 1920x1080
  - [ ] Player Settings → WebGL Template
- [ ] Configurar URP:
  - [ ] Window → Rendering → Lighting
  - [ ] Create URP Asset
  - [ ] Assign em Graphics Settings

#### TextMesh Pro
- [ ] Window → TextMesh Pro → Import TMP Essential Resources
- [ ] Window → TextMesh Pro → Import TMP Examples & Extras (opcional)
- [ ] Verificar se TMP funciona criando um Text - TextMeshPro de teste

#### Estrutura de Pastas
- [ ] Criar: `Assets/Prefabs/UI/`
- [ ] Criar: `Assets/Sprites/UI/Icons/`
- [ ] Criar: `Assets/Sprites/UI/Backgrounds/`
- [ ] Criar: `Assets/Sprites/UI/Effects/`
- [ ] Criar: `Assets/Scripts/UI/` (se não existir)

#### Scripts C#
- [ ] Copiar `UIAnimator.cs` para `Assets/Scripts/UI/`
- [ ] Copiar `ToastNotification.cs` para `Assets/Scripts/UI/`
- [ ] Copiar `LoadingIndicator.cs` para `Assets/Scripts/UI/`
- [ ] Copiar `ProductCardEnhanced.cs` para `Assets/Scripts/UI/`
- [ ] Copiar `ModalSystem.cs` para `Assets/Scripts/UI/`
- [ ] Copiar `ProductFilterSystem.cs` para `Assets/Scripts/UI/`
- [ ] Copiar `UIManagerEnhanced.cs` para `Assets/Scripts/UI/`

#### Verificação de Compilação
- [ ] Abrir Unity Editor
- [ ] Verificar Console (Window → General → Console)
- [ ] Garantir 0 erros de compilação
- [ ] Garantir 0 warnings críticos

**✅ Checkpoint:** Projeto Unity configurado, TMP importado, scripts compilando sem erros

---

### Dia 3-4: Prefabs Básicos (6-8h)

#### Toast Prefab
- [ ] Criar Panel no Canvas → Rename: "Toast"
- [ ] Configurar RectTransform: 400x80, pivot (1,1)
- [ ] Configurar Background Image (sliced)
- [ ] Adicionar Shadow component
- [ ] Adicionar Icon (Image, 40x40, left-center)
- [ ] Adicionar Text (TMP, stretch com margins)
- [ ] Adicionar Canvas Group
- [ ] Adicionar UIAnimator (SlideFromTop, EaseOut, 0.3s)
- [ ] Salvar como Prefab em `Assets/Prefabs/UI/Toast.prefab`
- [ ] Deletar da Hierarchy
- [ ] Testar: Drag prefab para scene, verificar no Inspector

#### Loading Panel Prefab
- [ ] Criar Panel → Rename: "LoadingPanel"
- [ ] Fill screen (stretch all)
- [ ] Criar Backdrop (Image, fill, black 70% alpha)
- [ ] Criar Content container (VBox Layout, centered)
- [ ] Criar Spinner (Image, 80x80, rotatable)
- [ ] Criar Progress Bar (Slider, 300x20)
  - [ ] Configurar Fill (lilac color)
  - [ ] Adicionar Percentage Text
- [ ] Criar Dots Container (HBox Layout)
  - [ ] Adicionar 3 Dots (Image, 20x20 each)
- [ ] Criar Pulse Image (Image, 100x100)
- [ ] Criar Status Text (TMP, centered)
- [ ] Adicionar LoadingIndicator script
- [ ] Atribuir todas referências no Inspector
- [ ] Configurar Loading Messages array
- [ ] Salvar como Prefab em `Assets/Prefabs/UI/LoadingPanel.prefab`
- [ ] Testar: ShowGlobal() funciona?

#### Product Card Prefab
- [ ] Criar Panel → Rename: "ProductCard" (300x420)
- [ ] Adicionar ProductCardEnhanced script
- [ ] Adicionar Canvas Group
- [ ] Adicionar UIAnimator (ScaleIn, EaseOut, 0.3s)
- [ ] Criar Card Background (Image, fill, white, shadow)
- [ ] Criar Product Image (RawImage, top 250px)
- [ ] Criar Badge 3D (Image + Text, top-right corner)
- [ ] Criar Quick View Button (Button, center, hidden)
  - [ ] Adicionar Icon (eye sprite)
- [ ] Criar Favorite Button (Button, top-right)
  - [ ] Adicionar Heart Icon
- [ ] Criar Info Container (VBox Layout)
  - [ ] Product Name (TMP, 18px bold)
  - [ ] Category (TMP, 14px)
  - [ ] Price Container (HBox Layout)
    - [ ] Price Label (TMP, 22px bold, lilac)
    - [ ] Try On Button (Button, 100x40)
- [ ] Criar Glow Effect (Image, 1.1 scale, hidden)
- [ ] Criar Shimmer Particles (Particle System, stopped)
- [ ] Atribuir TODAS as referências no ProductCardEnhanced
- [ ] Configurar category colors array
- [ ] Salvar como Prefab em `Assets/Prefabs/UI/ProductCard.prefab`
- [ ] Testar: Hover funciona? Favorite toggle?

#### Modal Templates (3 prefabs)

**Product Modal:**
- [ ] Criar estrutura: Backdrop → Modal Window → Content
- [ ] Adicionar Close Button (X, top-right)
- [ ] Adicionar Product Image (large, top half)
- [ ] Adicionar Product Info (VBox Layout)
  - [ ] Title (TMP)
  - [ ] Description (TMP, scrollable)
  - [ ] Price (TMP, large, lilac)
  - [ ] Size Selector (Dropdown)
  - [ ] Color Selector (HBox of buttons)
- [ ] Adicionar Action Buttons (HBox)
  - [ ] Try On Button (lilac)
  - [ ] Buy Button (green)
- [ ] Salvar: `Assets/Prefabs/UI/ProductModal.prefab`

**Confirm Modal:**
- [ ] Criar estrutura: Backdrop → Modal Window → Content
- [ ] Adicionar Close Button
- [ ] Adicionar Icon (Image, warning/info/etc)
- [ ] Adicionar Title (TMP, 22px bold)
- [ ] Adicionar Message (TMP, 16px, wrap)
- [ ] Adicionar Button Container (HBox)
  - [ ] Cancel Button (gray)
  - [ ] Confirm Button (lilac)
- [ ] Salvar: `Assets/Prefabs/UI/ConfirmModal.prefab`

**Avatar Modal:**
- [ ] Criar estrutura: Backdrop → Modal Window → Content
- [ ] Adicionar Close Button
- [ ] Adicionar Title: "Criar Avatar"
- [ ] Adicionar Instructions (TMP)
- [ ] Adicionar User ID Input (TMP_InputField)
- [ ] Adicionar Photo Inputs (VBox)
  - [ ] Front Photo Container
    - [ ] Label
    - [ ] Preview Image
    - [ ] Select Button
  - [ ] Side Photo Container
    - [ ] Label
    - [ ] Preview Image
    - [ ] Select Button
- [ ] Adicionar Progress Bar (hidden initially)
- [ ] Adicionar Submit Button (large, lilac)
- [ ] Salvar: `Assets/Prefabs/UI/AvatarModal.prefab`

**✅ Checkpoint:** 4 prefabs criados e salvos, todos configurados corretamente

---

### Dia 5-6: Scene Setup (6-8h)

#### Canvas Principal
- [ ] Criar Canvas (Right Click → UI → Canvas)
- [ ] Configurar Canvas:
  - [ ] Render Mode: Screen Space - Overlay
  - [ ] Canvas Scaler: Scale With Screen Size
  - [ ] Reference Resolution: 1920x1080
  - [ ] Match: 0.5
- [ ] Adicionar Graphic Raycaster
- [ ] Rename: "UICanvas"

#### Background
- [ ] Criar Image (child of Canvas)
- [ ] Rename: "BackgroundImage"
- [ ] Fill screen (stretch all)
- [ ] Gradient background (lilac tones)

#### Transition Particles
- [ ] Criar Particle System (child of Canvas)
- [ ] Rename: "TransitionParticles"
- [ ] Configurar:
  - [ ] Duration: 1s
  - [ ] Looping: No
  - [ ] Start Color: Lilac gradient
  - [ ] Emission: Burst 30
  - [ ] Shape: Cone, upwards
  - [ ] Initially stopped

#### Sidebar
- [ ] Criar Empty GameObject → "Sidebar"
- [ ] Anchors: Left-Stretch
- [ ] Width: 80px
- [ ] Adicionar VBox Layout (spacing 20, padding top 100)
- [ ] Criar Logo (Image, top)
- [ ] Criar Navigation container → "Navigation"
- [ ] Criar 4 Navigation Buttons:
  - [ ] Avatar Button (icon + label)
  - [ ] Catalog Button (icon + label)
  - [ ] Shop Button (icon + label)
  - [ ] Cart Button (icon + label)
- [ ] Adicionar UIAnimator em cada botão (Bounce)
- [ ] Configure initial colors (gray)

#### Top Bar
- [ ] Criar Empty GameObject → "TopBar"
- [ ] Anchors: Top-Stretch
- [ ] Height: 70px
- [ ] Left offset: 80px (after sidebar)
- [ ] Adicionar HBox Layout (padding 15, spacing 15)
- [ ] Criar Search Bar:
  - [ ] TMP_InputField
  - [ ] Width: 400px
  - [ ] Placeholder: "Buscar produtos..."
  - [ ] Background: White 20% alpha
- [ ] Criar Spacer (Layout Element, flexible width)
- [ ] Criar Settings Button (icon 30x30, button 50x50)
- [ ] Criar Notifications Button (icon 30x30, button 50x50)
- [ ] Criar Profile Button (icon 30x30, button 50x50)
- [ ] Adicionar UIAnimator em cada button

#### Content Area - 4 Painéis

**Avatar Panel:**
- [ ] Criar Empty → "AvatarPanel"
- [ ] Anchors: Fill (with margins for sidebar/topbar)
- [ ] Criar Avatar Preview area (3D viewport placeholder)
- [ ] Criar Controls section:
  - [ ] Rotate buttons
  - [ ] Zoom slider
- [ ] Criar Create Avatar Button (large, centered)
- [ ] Criar Info Text (instructions)
- [ ] Initially active? Yes

**Catalog Panel:**
- [ ] Criar Empty → "CatalogPanel"
- [ ] Anchors: Fill
- [ ] Criar Filters Section → "FiltersSection"
  - [ ] Width: 300px, left side
  - [ ] Adicionar ProductFilterSystem script
  - [ ] Criar Search Input (TMP_InputField)
  - [ ] Criar Category Dropdown (TMP_Dropdown)
    - [ ] Options: Todas, Vestidos, Camisetas, Calças, Acessórios, Sapatos
  - [ ] Criar Price Section:
    - [ ] Min Price Slider (0-500)
    - [ ] Max Price Slider (0-500)
    - [ ] Price labels (TMP)
  - [ ] Criar Has3D Toggle
  - [ ] Criar InStock Toggle
  - [ ] Criar Sort Dropdown (TMP_Dropdown)
    - [ ] Options: Nome A-Z, Nome Z-A, Preço ↑, Preço ↓, Mais novos
  - [ ] Criar Clear Filters Button
  - [ ] Criar Results Count Text (TMP)
  - [ ] Atribuir TODAS referências no ProductFilterSystem
- [ ] Criar Product Grid → "ProductGrid"
  - [ ] Fill remaining space (right of filters)
  - [ ] Adicionar Scroll Rect
    - [ ] Viewport
    - [ ] Content
  - [ ] Adicionar Grid Layout Group ao Content:
    - [ ] Cell Size: 320x440
    - [ ] Spacing: 20x20
    - [ ] Constraint: Fixed Column Count = 3
    - [ ] Child Alignment: Upper Left
- [ ] Initially active? No

**Shop Panel:**
- [ ] Criar Empty → "ShopPanel"
- [ ] Anchors: Fill
- [ ] Criar Placeholder Text: "Loja em breve!"
- [ ] Initially active? No

**Cart Panel:**
- [ ] Criar Empty → "CartPanel"
- [ ] Anchors: Fill
- [ ] Criar Placeholder Text: "Carrinho em breve!"
- [ ] Initially active? No

#### Toast Container
- [ ] Criar Empty → "ToastContainer"
- [ ] Anchors: Top-Right
- [ ] Pivot: (1, 1)
- [ ] Position: (-20, -20)
- [ ] Adicionar VBox Layout (spacing 10, upper right)

#### Loading Overlay
- [ ] Drag LoadingPanel prefab para Canvas
- [ ] Rename: "LoadingOverlay"
- [ ] Configure como Global em script (ver código)

#### Modal Container
- [ ] Criar Empty → "ModalContainer"
- [ ] Fill screen
- [ ] Adicionar ModalSystem script
- [ ] Criar Modal Panel (full screen, hidden)
- [ ] Criar Backdrop (Image, black 80% alpha)
- [ ] Criar Modal Content (center container)
- [ ] Criar Close Button (X, top-right)
- [ ] Atribuir template prefabs:
  - [ ] Product Modal Template
  - [ ] Confirm Modal Template
  - [ ] Avatar Modal Template

#### UIManagerEnhanced Configuration
- [ ] Selecionar UICanvas
- [ ] Adicionar UIManagerEnhanced script
- [ ] Atribuir Panels array (4):
  - [ ] [0] = AvatarPanel
  - [ ] [1] = CatalogPanel
  - [ ] [2] = ShopPanel
  - [ ] [3] = CartPanel
- [ ] Atribuir Navigation Indicators (4):
  - [ ] [0] = AvatarButton
  - [ ] [1] = CatalogButton
  - [ ] [2] = ShopButton
  - [ ] [3] = CartButton
- [ ] Atribuir Background Gradient: BackgroundImage
- [ ] Atribuir Transition Particles: TransitionParticles
- [ ] Atribuir Top Bar Buttons:
  - [ ] Settings Button
  - [ ] Notifications Button
  - [ ] Profile Button
- [ ] Atribuir System References:
  - [ ] Modal System: ModalContainer
  - [ ] Filter System: FiltersSection
  - [ ] Loading Indicator: LoadingOverlay
  - [ ] Avatar Manager: (find in scene)
  - [ ] Catalog Loader: (find in scene)
  - [ ] Try On Controller: (find in scene)
- [ ] Configure Settings:
  - [ ] Default Panel: 1 (Catalog)
  - [ ] Panel Transition Duration: 0.3

**✅ Checkpoint:** Scene completa, hierarquia criada, todos componentes atribuídos

---

### Dia 7: Assets e Testes (4-6h)

#### Sprites de Ícones
- [ ] Baixar/criar ícones (32x32, PNG, transparente):
  - [ ] heart.png (outline)
  - [ ] heart_filled.png (solid)
  - [ ] eye.png (quick view)
  - [ ] settings.png (gear)
  - [ ] bell.png (notifications)
  - [ ] user.png (profile)
  - [ ] avatar.png (avatar tab)
  - [ ] catalog.png (catalog tab)
  - [ ] shop.png (shop tab)
  - [ ] cart.png (cart tab)
  - [ ] close.png (X button)
  - [ ] check.png (success)
  - [ ] error.png (error)
  - [ ] warning.png (warning)
  - [ ] info.png (info)
- [ ] Importar para `Assets/Sprites/UI/Icons/`
- [ ] Configurar Texture Type: Sprite (2D and UI)
- [ ] Atribuir ícones nos botões/cards

#### Fontes
- [ ] Baixar Poppins (Google Fonts)
- [ ] Baixar Inter (Google Fonts)
- [ ] Importar para `Assets/Fonts/`
- [ ] Criar Font Assets no TextMesh Pro:
  - [ ] Window → TextMesh Pro → Font Asset Creator
  - [ ] Create Poppins-Bold
  - [ ] Create Inter-Regular
  - [ ] Create Inter-SemiBold
- [ ] Atribuir fontes nos textos

#### Materiais
- [ ] Criar Material → "UI_Blur"
  - [ ] Shader: UI/Default (or custom blur)
- [ ] Aplicar em:
  - [ ] Card backgrounds (30% opacity)
  - [ ] Modal backgrounds (50% opacity)
  - [ ] Top bar (20% opacity)

#### Testes em Play Mode
- [ ] Pressionar Play
- [ ] Testar navegação:
  - [ ] Pressionar 1 → Avatar Panel aparece?
  - [ ] Pressionar 2 → Catalog Panel aparece?
  - [ ] Pressionar 3 → Shop Panel aparece?
  - [ ] Pressionar 4 → Cart Panel aparece?
  - [ ] Indicadores mudam de cor?
  - [ ] Animações acontecem?
- [ ] Testar Top Bar:
  - [ ] Settings button clicável?
  - [ ] Notifications button clicável?
  - [ ] Profile button clicável?
  - [ ] Toasts aparecem?
- [ ] Testar Filtros:
  - [ ] Digitar na busca → debounce funciona?
  - [ ] Mudar categoria → aplica filtro?
  - [ ] Mover sliders de preço → labels atualizam?
  - [ ] Toggles funcionam?
  - [ ] Clear Filters limpa tudo?
- [ ] Testar Loading:
  - [ ] LoadingIndicator.ShowGlobal() funciona?
  - [ ] Spinner gira?
  - [ ] HideGlobal() esconde?
- [ ] Testar Modais:
  - [ ] Modal abre?
  - [ ] Backdrop visível?
  - [ ] Close button fecha?
  - [ ] Animações suaves?

#### Fix Bugs
- [ ] Listar todos os bugs encontrados
- [ ] Priorizar (crítico, alto, médio, baixo)
- [ ] Corrigir bugs críticos
- [ ] Documentar bugs conhecidos

**✅ Checkpoint FASE 1 COMPLETA:** Sistema básico 100% funcional!

---

## 🎨 FASE 2: VISUAL POLISH (Dias 8-14)

### Dia 8-9: Glassmorphism (6-8h)

#### Criar Shader de Blur
- [ ] Pesquisar shader de UI blur para Unity
- [ ] Importar shader (or criar custom)
- [ ] Criar Material com shader
- [ ] Testar blur effect

#### Aplicar Glassmorphism
- [ ] Product Cards:
  - [ ] Background com blur
  - [ ] 30% opacity
  - [ ] Subtle border
- [ ] Modals:
  - [ ] Background com blur
  - [ ] 50% opacity
  - [ ] Rounded corners
- [ ] Top Bar:
  - [ ] Background com blur
  - [ ] 20% opacity
  - [ ] Bottom border
- [ ] Sidebar:
  - [ ] Background com blur (optional)
  - [ ] 15% opacity

#### Sombras
- [ ] Adicionar Shadow em Product Cards
  - [ ] Distance: (2, -2)
  - [ ] Color: Black 50% alpha
- [ ] Adicionar Shadow em Modals
  - [ ] Distance: (4, -4)
  - [ ] Color: Black 60% alpha
- [ ] Adicionar Shadow em Top Bar
  - [ ] Distance: (0, -2)
  - [ ] Color: Black 30% alpha

**✅ Checkpoint:** Visual moderno com depth

---

### Dia 10-11: Particle Effects (6-8h)

#### Transition Particles
- [ ] Selecionar TransitionParticles GameObject
- [ ] Configure Particle System:
  - [ ] Duration: 1s
  - [ ] Looping: No
  - [ ] Start Lifetime: 0.5-1.5
  - [ ] Start Speed: 2-5
  - [ ] Start Size: 0.1-0.3
  - [ ] Start Color: Lilac gradient (C29AFF → White)
  - [ ] Max Particles: 30
- [ ] Configure Emission:
  - [ ] Rate over Time: 0
  - [ ] Bursts: Add burst (time 0, count 20-30)
- [ ] Configure Shape:
  - [ ] Shape: Cone
  - [ ] Angle: 30
  - [ ] Radius: 2
  - [ ] Emit from: Base
- [ ] Configure Velocity over Lifetime:
  - [ ] Linear: (0, 5, 0) - upwards
- [ ] Configure Color over Lifetime:
  - [ ] Gradient: Full alpha → 0 alpha
- [ ] Configure Renderer:
  - [ ] Render Mode: Billboard
  - [ ] Material: Default-Particle (Additive)
- [ ] Testar: Play() no UIManagerEnhanced transitions

#### Shimmer Particles (per Card)
- [ ] No ProductCard prefab, selecionar ShimmerParticles
- [ ] Configure:
  - [ ] Duration: 2s
  - [ ] Looping: Yes
  - [ ] Start Lifetime: 1-2
  - [ ] Start Speed: 0.2
  - [ ] Start Size: 0.05-0.15
  - [ ] Start Color: White → Lilac gradient
  - [ ] Max Particles: 10
- [ ] Emission:
  - [ ] Rate over Time: 5
- [ ] Shape:
  - [ ] Shape: Box
  - [ ] Scale: (card width, card height, 0)
- [ ] Color over Lifetime:
  - [ ] Pulse gradient (fade in/out)
- [ ] Initially: Stop (play on hover)
- [ ] Testar: Hover em card → particles aparecem?

#### Background Particles (Optional)
- [ ] Criar Particle System no Canvas → "BackgroundParticles"
- [ ] Full screen
- [ ] Subtle floating elements
- [ ] Very slow movement
- [ ] Low emission (2-3 particles)
- [ ] Lilac tones, very transparent (5-10% alpha)

**✅ Checkpoint:** Micro-interações e feedback visual rico

---

### Dia 12-13: Animações (6-8h)

#### Configurar UIAnimator em Elementos
- [ ] Painéis (4):
  - [ ] Avatar Panel: FadeIn, EaseOut, 0.3s
  - [ ] Catalog Panel: FadeIn, EaseOut, 0.3s
  - [ ] Shop Panel: FadeIn, EaseOut, 0.3s
  - [ ] Cart Panel: FadeIn, EaseOut, 0.3s
- [ ] Product Cards:
  - [ ] ScaleIn, EaseOut, 0.3s
  - [ ] Stagger delay (0.05s increments per card)
- [ ] Modals:
  - [ ] SlideFromBottom, EaseOut, 0.3s
- [ ] Toasts:
  - [ ] SlideFromTop, EaseOut, 0.3s
- [ ] Buttons (hover):
  - [ ] Scale 1.1x, EaseOut, 0.2s (via script)

#### Ajustar Timings
- [ ] Testar todas as animações
- [ ] Ajustar durações se necessário
- [ ] Ajustar easing curves
- [ ] Garantir 60 FPS

#### Sequenciar Animações
- [ ] Welcome sequence:
  - [ ] Logo fade in
  - [ ] Sidebar slide in
  - [ ] Top bar slide in
  - [ ] Content fade in
  - [ ] Toasts aparecem
- [ ] Panel transitions:
  - [ ] Old panel fade out
  - [ ] Particles burst
  - [ ] New panel fade in
- [ ] Card grid:
  - [ ] Cards aparecem em sequência (stagger)

#### Bounce e Shake
- [ ] Botão Favorite: Bounce ao clicar
- [ ] Erro: Shake no input field
- [ ] Success: Bounce no ícone de check

**✅ Checkpoint:** Transições fluidas e profissionais

---

### Dia 14: Color & Typography (4h)

#### Aplicar Color Palette
- [ ] Verificar todas as cores:
  - [ ] Primary #C29AFF usado consistentemente
  - [ ] Success #33CC66 em confirmações
  - [ ] Error #E64D4D em erros
  - [ ] Warning #FFB347 em avisos
  - [ ] Info #4D9BE6 em informações
  - [ ] Text #333333 em textos principais
  - [ ] Subtle #999999 em textos secundários
- [ ] Ajustar contrastes:
  - [ ] Texto em background claro: ratio > 4.5:1
  - [ ] Texto em background escuro: ratio > 4.5:1
- [ ] Consistência de botões:
  - [ ] Primary actions: Lilac
  - [ ] Secondary: Gray
  - [ ] Destructive: Red
  - [ ] Success: Green

#### Configurar Tipografia
- [ ] Headings (Poppins Bold):
  - [ ] H1: 28px
  - [ ] H2: 22px
  - [ ] H3: 18px
- [ ] Body (Inter Regular):
  - [ ] Normal: 16px
  - [ ] Small: 14px
  - [ ] Tiny: 12px
- [ ] Buttons (Inter Semi-Bold):
  - [ ] Large: 16px
  - [ ] Normal: 14px
  - [ ] Small: 12px
- [ ] Line spacing:
  - [ ] Body: 1.5
  - [ ] Headings: 1.2
  - [ ] Buttons: 1.0
- [ ] Verificar legibilidade em todas resoluções

#### Testes Visuais
- [ ] Screenshot de cada painel
- [ ] Comparar com design system
- [ ] Ajustar inconsistências
- [ ] Validar com designer (se tiver)

**✅ Checkpoint FASE 2 COMPLETA:** Design system aplicado!

---

## 🔗 FASE 3: INTEGRATION (Dias 15-21)

### Dia 15-16: API Integration (8-10h)

#### Configurar APIClient
- [ ] Criar/editar `APIClient.cs`
- [ ] Configurar base URL do backend
- [ ] Implementar métodos:
  - [ ] GET
  - [ ] POST
  - [ ] PUT
  - [ ] DELETE
- [ ] Adicionar headers:
  - [ ] Content-Type: application/json
  - [ ] Authorization (se necessário)
- [ ] Implementar error handling
- [ ] Adicionar retry logic (3 tentativas)
- [ ] Timeout de 30 segundos

#### Integrar CatalogLoader
- [ ] Editar `CatalogLoader.cs`
- [ ] Adicionar referências:
  - [ ] ProductFilterSystem
  - [ ] Transform productGrid
  - [ ] GameObject productCardPrefab
- [ ] Implementar LoadCatalog():
  - [ ] Mostrar loading
  - [ ] Chamar API: GET /api/catalog
  - [ ] Parse JSON para List<ProductData>
  - [ ] Configurar filterSystem.SetProducts()
  - [ ] Subscribe to ProductGridUpdated event
  - [ ] DisplayProducts()
  - [ ] Esconder loading
  - [ ] Mostrar toast de sucesso
- [ ] Implementar error handling:
  - [ ] Try-catch
  - [ ] Toast de erro
  - [ ] Log no Console
  - [ ] Retry button (opcional)

#### Testar com Backend
- [ ] Garantir backend rodando (localhost:5000)
- [ ] Testar GET /api/catalog no Postman
- [ ] Rodar Unity Play Mode
- [ ] Verificar produtos carregando
- [ ] Verificar Console por erros
- [ ] Testar filtros com dados reais
- [ ] Testar busca com dados reais

#### CORS Configuration
- [ ] Configurar CORS no backend:
  - [ ] Permitir origem do Unity (localhost)
  - [ ] Permitir headers necessários
  - [ ] Permitir métodos: GET, POST, PUT, DELETE
- [ ] Testar novamente após configurar

**✅ Checkpoint:** Dados reais do backend aparecendo!

---

### Dia 17-18: Avatar System (8-10h)

#### Integrar AvatarManager
- [ ] Editar/criar `AvatarManager.cs`
- [ ] Implementar LoadAvatar(avatarId)
- [ ] Implementar CreateAvatar(userId, frontUrl, sideUrl)
- [ ] Integrar com API:
  - [ ] POST /api/avatar/create
  - [ ] GET /api/avatar/{id}
- [ ] Carregar modelo 3D na scene
- [ ] Implementar controles de câmera:
  - [ ] Rotate
  - [ ] Zoom
  - [ ] Pan (opcional)

#### Photo Upload
- [ ] Implementar OpenFilePicker():
  - [ ] WebGL: <input type="file">
  - [ ] Desktop: File dialog
  - [ ] Mobile: Camera/gallery
- [ ] Implementar UploadImage():
  - [ ] Convert Texture2D to byte[]
  - [ ] POST to backend
  - [ ] Retornar URL da imagem
- [ ] Preview de imagem selecionada

#### Avatar Creation Flow
- [ ] Conectar UIManagerEnhanced.ShowAvatarCreation()
- [ ] Abrir ModalSystem.ShowAvatarCreationModal()
- [ ] Ao submit:
  - [ ] Validar inputs
  - [ ] Mostrar loading com progresso
  - [ ] Upload front image (0-30%)
  - [ ] Upload side image (30-60%)
  - [ ] Criar avatar (60-80%)
  - [ ] Carregar avatar (80-100%)
  - [ ] Fechar modal
  - [ ] Mostrar painel de avatar
  - [ ] Toast de sucesso
- [ ] Error handling em cada etapa

#### Testar End-to-End
- [ ] Clicar "Criar Avatar"
- [ ] Preencher userId
- [ ] Selecionar foto frontal
- [ ] Selecionar foto lateral
- [ ] Clicar Submit
- [ ] Verificar progresso
- [ ] Avatar aparece?
- [ ] Controles funcionam?

**✅ Checkpoint:** Avatar creation funcional!

---

### Dia 19-20: Try-On Workflow (8-10h)

#### Integrar TryOnController
- [ ] Editar/criar `TryOnController.cs`
- [ ] Implementar ApplyClothing(ProductData):
  - [ ] Carregar modelo 3D da roupa
  - [ ] Aplicar no avatar
  - [ ] Ajustar posição/escala
  - [ ] Ajustar física (cloth simulation)
- [ ] Integrar com API:
  - [ ] POST /api/tryon
  - [ ] Body: { avatarId, productId }
  - [ ] Response: { success, imageUrl }
- [ ] Implementar RemoveClothing()
- [ ] Cache de modelos 3D (não recarregar)

#### Conectar ProductCard
- [ ] No ProductCardEnhanced.OnTryOnClick():
  - [ ] Mostrar loading
  - [ ] Chamar TryOnController.ApplyClothing()
  - [ ] Aguardar conclusão
  - [ ] Esconder loading
  - [ ] Toast de sucesso/erro
  - [ ] Mudar para painel de avatar
  - [ ] Animação de transição

#### Feedback Visual
- [ ] Loading spinner durante aplicação
- [ ] Progress bar (se API suportar)
- [ ] Toast "Roupa aplicada!"
- [ ] Highlight na roupa no avatar
- [ ] Particle effect ao aplicar (opcional)

#### Performance
- [ ] Object pooling de modelos 3D
- [ ] Async loading de texturas
- [ ] LOD (Level of Detail) nos modelos
- [ ] Garbage collection management
- [ ] Frame rate mantém 60 FPS?

#### Testar Fluxo Completo
- [ ] Buscar "vestido azul"
- [ ] Clicar em card
- [ ] Clicar "Experimentar"
- [ ] Loading aparece?
- [ ] Roupa aplica no avatar?
- [ ] Transição suave?
- [ ] FPS mantém?

**✅ Checkpoint:** Try-on workflow completo!

---

### Dia 21: E2E Testing (6-8h)

#### Fluxo 1: Novo Usuário
- [ ] Abrir aplicação (Play Mode)
- [ ] Welcome sequence aparece?
- [ ] Clicar "Criar Avatar"
- [ ] Upload fotos
- [ ] Avatar criado com sucesso?
- [ ] Painel de avatar mostra avatar?
- [ ] Controles de câmera funcionam?
- [ ] Ir para Catálogo (teclado: 2)
- [ ] Produtos carregam?
- [ ] Filtros funcionam?
- [ ] Buscar por "vestido"
- [ ] Resultados corretos?
- [ ] Hover em card → effects funcionam?
- [ ] Clicar "Experimentar"
- [ ] Roupa aplica?
- [ ] Ver resultado no painel de avatar
- [ ] OK?

#### Fluxo 2: Erro Handling
- [ ] Desligar backend
- [ ] Tentar carregar catálogo
- [ ] Erro aparece?
- [ ] Toast de erro?
- [ ] Mensagem clara?
- [ ] Religar backend
- [ ] Retry funciona?

#### Fluxo 3: Performance
- [ ] Carregar 100+ produtos
- [ ] FPS mantém 60?
- [ ] Scroll suave?
- [ ] Filtrar produtos
- [ ] Lag perceptível?
- [ ] Aplicar roupa
- [ ] FPS drop?

#### Diferentes Resoluções
- [ ] Testar 1920x1080:
  - [ ] Layout OK?
  - [ ] Textos legíveis?
  - [ ] Botões clicáveis?
- [ ] Testar 1280x720:
  - [ ] Layout adapta?
  - [ ] Grid columns correto?
  - [ ] Não quebra?
- [ ] Testar 1024x768:
  - [ ] 4:3 funciona?
  - [ ] Sidebar OK?
  - [ ] Top bar OK?

#### Bug List
- [ ] Listar TODOS os bugs encontrados
- [ ] Classificar severidade:
  - [ ] Crítico (bloqueia uso)
  - [ ] Alto (afeta funcionalidade)
  - [ ] Médio (usabilidade)
  - [ ] Baixo (cosmético)
- [ ] Corrigir bugs críticos e altos
- [ ] Documentar bugs médios/baixos para depois

#### Performance Profiling
- [ ] Window → Analysis → Profiler
- [ ] Rodar 1 minuto de uso
- [ ] Verificar:
  - [ ] CPU usage < 80%
  - [ ] Memory leaks?
  - [ ] GC spikes?
  - [ ] Draw calls < 100?
  - [ ] Batching funcionando?
- [ ] Otimizar se necessário

**✅ Checkpoint FASE 3 COMPLETA:** Sistema robusto e testado!

---

## 🚀 FASE 4: ADVANCED FEATURES (Dias 22+)

### Shopping Cart System (Semana 4)

#### CartManager Singleton
- [ ] Criar `CartManager.cs`
- [ ] Singleton pattern
- [ ] List<CartItem> items
- [ ] AddItem(ProductData)
- [ ] RemoveItem(productId)
- [ ] UpdateQuantity(productId, qty)
- [ ] GetTotal()
- [ ] Clear()
- [ ] Save to PlayerPrefs (persistence)
- [ ] Load on Start

#### Cart UI
- [ ] Editar CartPanel
- [ ] Criar CartItem prefab:
  - [ ] Product image (small)
  - [ ] Product name
  - [ ] Quantity selector (- [n] +)
  - [ ] Price
  - [ ] Remove button
- [ ] Criar Cart list (ScrollView)
- [ ] Criar Cart summary:
  - [ ] Subtotal
  - [ ] Shipping (optional)
  - [ ] Total
- [ ] Criar Checkout button
- [ ] Empty state: "Carrinho vazio"

#### Integration
- [ ] Adicionar "Add to Cart" button nos cards
- [ ] Badge count no Cart button (sidebar)
- [ ] Toast ao adicionar item
- [ ] Animation ao adicionar (fly to cart)
- [ ] Conectar com backend (POST /api/cart)

#### Testing
- [ ] Adicionar 5 items
- [ ] Badge atualiza?
- [ ] Cart panel mostra items?
- [ ] Quantity funciona?
- [ ] Remove funciona?
- [ ] Total calcula certo?
- [ ] Persiste ao reabrir?

**✅ Checkpoint:** Carrinho funcional!

---

### Favorites & History (Semana 5)

#### Favorites System
- [ ] Criar `FavoritesManager.cs`
- [ ] List<string> favoriteIds
- [ ] Add(productId)
- [ ] Remove(productId)
- [ ] Contains(productId)
- [ ] GetAll()
- [ ] Save to PlayerPrefs
- [ ] Load on Start

#### History System
- [ ] Criar `HistoryManager.cs`
- [ ] List<HistoryItem> (productId, timestamp)
- [ ] AddView(productId)
- [ ] AddTryOn(productId)
- [ ] GetRecent(limit)
- [ ] Clear()

#### UI
- [ ] Favorites tab no Catalog:
  - [ ] Filter para mostrar apenas favoritos
  - [ ] Count de favoritos
- [ ] History tab no Shop:
  - [ ] "Vistos recentemente"
  - [ ] "Já experimentados"
  - [ ] Timestamps

#### Export/Share
- [ ] Screenshot do avatar com roupa
- [ ] Save to file
- [ ] Share nas redes sociais (WebGL share API)
- [ ] Copy to clipboard

**✅ Checkpoint:** Engagement features!

---

### 3D Preview Advanced (Semana 6)

#### ProductPreview3D Component
- [ ] Criar `ProductPreview3D.cs`
- [ ] Modal 3D viewer
- [ ] Camera controls:
  - [ ] Mouse drag to rotate
  - [ ] Mouse wheel to zoom
  - [ ] Pan (middle mouse)
- [ ] Lighting setup:
  - [ ] 3-point lighting
  - [ ] Adjustable intensity
- [ ] Texture quality selector:
  - [ ] Low, Medium, High
- [ ] Loading optimization:
  - [ ] Progressive loading
  - [ ] Show wireframe while loading

#### Integration
- [ ] Conectar Quick View button
- [ ] Abrir modal 3D
- [ ] Carregar modelo do produto
- [ ] Controles funcionam?
- [ ] Close button
- [ ] "Experimentar" button no modal

#### Performance
- [ ] LOD system
- [ ] Texture streaming
- [ ] Object pooling
- [ ] Mantém 60 FPS?

**✅ Checkpoint:** Preview 3D premium!

---

### Premium Features (Semana 7+)

#### Audio Manager
- [ ] Criar `AudioManager.cs`
- [ ] Sound effects:
  - [ ] UI hover
  - [ ] UI click
  - [ ] Success chime
  - [ ] Error buzz
  - [ ] Notification pop
  - [ ] Whoosh (transitions)
- [ ] Volume controls
- [ ] Mute toggle
- [ ] Sound pool

#### Voice Commands (Experimental)
- [ ] Integrar speech recognition API
- [ ] Commands:
  - [ ] "Mostrar vestidos"
  - [ ] "Experimentar"
  - [ ] "Próximo produto"
  - [ ] "Adicionar ao carrinho"
- [ ] Visual feedback de comando
- [ ] Error handling

#### Advanced Touch Gestures
- [ ] Swipe left/right: Next/prev product
- [ ] Pinch: Zoom avatar
- [ ] Two-finger rotate: Rotate avatar
- [ ] Long press: Quick actions menu

#### AR Try-On
- [ ] Pesquisar ARCore/ARKit integration
- [ ] Camera feed
- [ ] Body tracking
- [ ] Virtual clothing overlay
- [ ] Real-time rendering

**✅ Checkpoint:** Next-gen features!

---

## 📊 FINAL CHECKLIST

### Funcionalidade
- [ ] Todas as 4 páginas funcionam
- [ ] Navegação suave
- [ ] Todos os botões respondem
- [ ] Filtros funcionam corretamente
- [ ] Busca retorna resultados corretos
- [ ] Modals abrem e fecham
- [ ] Loading aparece em operações async
- [ ] Toasts mostram feedback correto
- [ ] Avatar creation funciona
- [ ] Try-on aplica roupas
- [ ] Carrinho adiciona/remove items
- [ ] Favoritos salvam
- [ ] Histórico registra
- [ ] 3D preview funciona

### Performance
- [ ] 60 FPS constante
- [ ] Load time < 3s
- [ ] No memory leaks
- [ ] No GC spikes
- [ ] Bundle size aceitável
- [ ] Otimizações aplicadas

### Visual
- [ ] Design system consistente
- [ ] Cores corretas
- [ ] Fontes legíveis
- [ ] Espaçamentos adequados
- [ ] Animações fluidas
- [ ] Glassmorphism aplicado
- [ ] Sombras consistentes
- [ ] Icons coerentes

### UX
- [ ] Feedback em todas ações
- [ ] Loading em operações lentas
- [ ] Erros com mensagens claras
- [ ] Success com confirmação visual
- [ ] Hover effects em botões
- [ ] Click feedback
- [ ] Navegação intuitiva
- [ ] Empty states
- [ ] Accessibility básica

### Code Quality
- [ ] 0 erros no Console
- [ ] 0 warnings críticos
- [ ] Scripts documentados
- [ ] Code review passado
- [ ] No código duplicado
- [ ] Naming conventions
- [ ] Estrutura organizada

### Build & Deploy
- [ ] Build WebGL funciona
- [ ] Sem erros de build
- [ ] Compression configurada
- [ ] Load otimizado
- [ ] Deploy testado
- [ ] URL acessível
- [ ] Cross-browser testado

---

## 🎉 CONGRATULATIONS!

Se você chegou até aqui e marcou tudo, você tem:

✅ **Frontend completo e funcional**
✅ **7 componentes UI integrados**
✅ **Experiência premium e polida**
✅ **Performance otimizada**
✅ **Sistema robusto e testado**

**🚀 Projeto pronto para apresentação!**

---

*Checklist criado em: 6 de novembro de 2025*
*Última atualização: 6 de novembro de 2025*
*Use este arquivo para acompanhar seu progresso!*

**💡 Dica:** Imprima ou use um markdown viewer com checkboxes interativos!

# 🚀 Guia de Implementação - Frontend Premium

## ⏱️ Tempo Estimado: 2-3 horas

---

## 📋 Checklist de Implementação

### ✅ Fase 1: Setup Inicial (15 min)
- [ ] Importar TextMesh Pro (Window → TextMesh Pro → Import Essential Resources)
- [ ] Criar pasta `Assets/Prefabs/UI/`
- [ ] Criar pasta `Assets/Sprites/UI/`
- [ ] Configurar Canvas

### ✅ Fase 2: Criar Prefabs Básicos (30 min)
- [ ] Toast Prefab
- [ ] Loading Panel Prefab
- [ ] Product Card Prefab
- [ ] Modal Templates

### ✅ Fase 3: Configurar Scene (30 min)
- [ ] Setup UICanvas
- [ ] Criar painéis principais
- [ ] Adicionar navegação
- [ ] Top bar

### ✅ Fase 4: Integração (45 min)
- [ ] Conectar sistemas
- [ ] Testar fluxos
- [ ] Ajustar animações

### ✅ Fase 5: Polish (30 min)
- [ ] Ajustar cores
- [ ] Testar responsividade
- [ ] Validar UX

---

## 🎯 Fase 1: Setup Inicial

### 1.1 Importar TextMesh Pro
```
1. Window → TextMesh Pro → Import TMP Essential Resources
2. Window → TextMesh Pro → Import TMP Examples & Extras (opcional)
```

### 1.2 Estrutura de Pastas
Criar esta estrutura:
```
Assets/
├── Prefabs/
│   └── UI/
│       ├── Toast.prefab
│       ├── LoadingPanel.prefab
│       ├── ProductCard.prefab
│       ├── ProductModal.prefab
│       ├── ConfirmModal.prefab
│       └── AvatarModal.prefab
├── Sprites/
│   └── UI/
│       ├── Icons/
│       │   ├── heart.png
│       │   ├── heart_filled.png
│       │   ├── eye.png
│       │   ├── settings.png
│       │   ├── bell.png
│       │   ├── user.png
│       │   ├── avatar.png
│       │   ├── catalog.png
│       │   ├── shop.png
│       │   └── cart.png
│       ├── Backgrounds/
│       │   ├── card_bg.png
│       │   ├── button_bg.png
│       │   └── panel_bg.png
│       └── Effects/
│           ├── glow.png
│           └── blur.png
```

### 1.3 Criar Canvas Principal
```
Hierarchy → Right Click → UI → Canvas

Canvas Settings:
- Render Mode: Screen Space - Overlay
- Canvas Scaler:
  * UI Scale Mode: Scale With Screen Size
  * Reference Resolution: 1920 x 1080
  * Screen Match Mode: Match Width Or Height
  * Match: 0.5
- Graphic Raycaster: Yes
```

---

## 🎨 Fase 2: Criar Prefabs

### 2.1 Toast Prefab

**Estrutura:**
```
Toast (RectTransform)
├── Background (Image)
├── Icon (Image)
└── Text (TextMeshProUGUI)
```

**Passo-a-passo:**

1. **Criar Toast Root**
```
Right Click no Canvas → UI → Panel
Rename: "Toast"
RectTransform:
- Width: 400
- Height: 80
- Pivot: (1, 1) - Top Right
```

2. **Background**
```
Já existe como filho do Panel
Image Component:
- Color: White (será mudado em runtime)
- Sprite: UI/Skin/Background (ou sprite customizado)
- Image Type: Sliced

Add Component: Shadow
- Effect Distance: (2, -2)
- Effect Color: Black 50% opacity
```

3. **Icon**
```
Right Click Toast → UI → Image
Name: Icon
RectTransform:
- Anchors: Left-Center
- Pos X: 20
- Width: 40
- Height: 40
```

4. **Text**
```
Right Click Toast → UI → Text - TextMeshPro
Name: Text
RectTransform:
- Anchors: Stretch-Stretch
- Left: 70, Right: 20, Top: 10, Bottom: 10
- Alignment: Middle Left
- Font Size: 18
- Color: White
```

5. **Adicionar Componentes**
```
Select Toast → Add Component:
- Canvas Group (para animações de fade)
- UIAnimator (script criado)
  * Animation Type: SlideFromTop
  * Easing Type: EaseOut
  * Duration: 0.3
```

6. **Salvar Prefab**
```
Drag Toast from Hierarchy to Assets/Prefabs/UI/
Delete from Hierarchy
```

---

### 2.2 Loading Panel Prefab

**Estrutura:**
```
LoadingPanel (RectTransform)
├── Backdrop (Image - fill screen)
├── Content (VerticalLayoutGroup)
│   ├── Spinner (Image + Rotation Animation)
│   ├── ProgressBar (Slider)
│   ├── DotsContainer (HorizontalLayoutGroup)
│   │   ├── Dot1, Dot2, Dot3
│   ├── PulseImage (Image)
│   └── StatusText (TextMeshProUGUI)
```

**Passo-a-passo:**

1. **Loading Panel Root**
```
Canvas → UI → Panel
Name: LoadingPanel
RectTransform: Fill (Stretch all)
Canvas Group: Alpha 1
```

2. **Backdrop**
```
Child of LoadingPanel → UI → Image
Name: Backdrop
RectTransform: Fill
Image:
- Color: Black 70% opacity
- Raycast Target: Yes (bloqueia cliques)
```

3. **Content Container**
```
Child of LoadingPanel → UI → Empty (Create Empty)
Name: Content
RectTransform:
- Anchors: Middle-Center
- Width: 400, Height: 300
Add Component: Vertical Layout Group
- Spacing: 20
- Child Alignment: Middle Center
- Child Controls Size: Width and Height
- Child Force Expand: No
```

4. **Spinner**
```
Child of Content → UI → Image
Name: Spinner
RectTransform: Width 80, Height 80
Image:
- Sprite: (círculo com seta ou ícone loading)
- Color: Lilac #C29AFF
```

5. **Progress Bar**
```
Child of Content → UI → Slider
Name: ProgressBar
RectTransform: Width 300, Height 20

Configure Slider:
- Fill Area → Fill: 
  * Color: Lilac #C29AFF
- Background:
  * Color: Gray 30% opacity

Add Child: PercentageText
- Right side of slider
- TextMeshPro showing "0%"
```

6. **Dots Container**
```
Child of Content → Create Empty
Name: DotsContainer
RectTransform: Width 120, Height 40
Add: Horizontal Layout Group
- Spacing: 20
- Child Alignment: Middle Center

Create 3 Dots (UI → Image):
- Names: Dot1, Dot2, Dot3
- Width/Height: 20
- Color: Lilac #C29AFF
- Shape: Circle
```

7. **Pulse Image**
```
Child of Content → UI → Image
Name: PulseImage
Width/Height: 100
Color: Lilac #C29AFF 50% opacity
```

8. **Status Text**
```
Child of Content → UI → Text - TextMeshPro
Name: StatusText
Width: 350, Height: 30
Alignment: Center
Font Size: 16
Text: "Carregando..."
```

9. **Add LoadingIndicator Script**
```
Select LoadingPanel → Add Component: LoadingIndicator
Assign in Inspector:
- Spinner Image: Spinner
- Progress Bar: ProgressBar
- Progress Text: PercentageText
- Dots Container: DotsContainer
- Pulse Image: PulseImage
- Status Text: StatusText
- Loading Style: Spinner (default)
- Spin Speed: 180
- Loading Messages: ["Carregando...", "Aguarde...", "Processando..."]
```

10. **Save Prefab**

---

### 2.3 Product Card Prefab

**Estrutura:**
```
ProductCard
├── CardBackground (Image)
├── ProductImage (RawImage)
├── Badge3D (Image + Text)
├── QuickViewButton (Button)
│   └── Icon (Image)
├── FavoriteButton (Button)
│   └── HeartIcon (Image)
├── InfoContainer (VerticalLayoutGroup)
│   ├── ProductName (TMP)
│   ├── Category (TMP)
│   └── PriceContainer (HorizontalLayoutGroup)
│       ├── PriceLabel (TMP)
│       └── TryOnButton (Button)
├── GlowEffect (Image)
└── ShimmerParticles (ParticleSystem)
```

**Passo-a-passo:**

1. **Card Root**
```
Canvas → UI → Panel
Name: ProductCard
RectTransform: Width 300, Height 420
Add Components:
- ProductCardEnhanced (script)
- Canvas Group
- UIAnimator (ScaleIn, EaseOut, 0.3s)
```

2. **Card Background**
```
Child (Image)
Name: CardBackground
Fill parent
Color: White
Add: Shadow (2, -2)
Material: (opcional) UI/Blur para glassmorphism
```

3. **Product Image**
```
Child → UI → Raw Image
Name: ProductImage
RectTransform: 
- Top: 0, Height: 250
- Left: 0, Right: 0
Image: Gray placeholder
Aspect Ratio: Free
```

4. **Badge 3D**
```
Child → UI → Image
Name: Badge3D
Position: Top-Right corner
Width: 60, Height: 30
Color: Lilac #C29AFF

Child Text:
- Text: "3D"
- Font Size: 14
- Bold
- Color: White
- Alignment: Center
```

5. **Quick View Button**
```
Child → UI → Button
Name: QuickViewButton
Position: Center of ProductImage
Width/Height: 60
Initially: SetActive(false) - aparece no hover

Image:
- Color: Lilac 80% opacity
- Sprite: Circle

Icon Child:
- Sprite: Eye icon
- Color: White
- Size: 30x30
```

6. **Favorite Button**
```
Child → UI → Button
Name: FavoriteButton
Position: Top-Right (below Badge3D)
Width/Height: 50

HeartIcon Child:
- Sprite: heart outline
- Color: White (or Lilac when favorited)
- Size: 30x30
```

7. **Info Container**
```
Child → Create Empty
Name: InfoContainer
RectTransform:
- Top: 250, Bottom: 0
- Left: 15, Right: 15

Add: Vertical Layout Group
- Spacing: 8
- Padding: All 10
- Child Alignment: Upper Left
```

8. **Product Name**
```
Child of InfoContainer → TMP
Name: ProductName
Height: 50
Font Size: 18
Bold
Color: #333333
Wrap: Enabled
Overflow: Ellipsis
```

9. **Category**
```
Child of InfoContainer → TMP
Name: Category
Height: 25
Font Size: 14
Color: #666666
```

10. **Price Container**
```
Child of InfoContainer → Create Empty
Name: PriceContainer
Height: 50

Add: Horizontal Layout Group
- Spacing: 10
- Child Alignment: Middle Left

Children:
- PriceLabel (TMP):
  * Text: "R$ 0,00"
  * Font Size: 22
  * Bold
  * Color: Lilac #C29AFF

- TryOnButton (Button):
  * Width: 100, Height: 40
  * Text: "Experimentar"
  * Font Size: 14
  * Background: Lilac
  * Text Color: White
```

11. **Glow Effect**
```
Child → UI → Image
Name: GlowEffect
Fill parent (slightly larger)
Scale: 1.1
Sprite: Soft gradient circle
Color: Lilac 0% opacity (animated to 30% on hover)
Raycast Target: No
Initially: SetActive(false)
```

12. **Shimmer Particles**
```
Child → Effects → Particle System
Name: ShimmerParticles
Settings:
- Duration: 1
- Looping: Yes
- Start Lifetime: 0.5-1.0
- Start Speed: 0.5
- Start Size: 0.1-0.3
- Start Color: White → Lilac gradient
- Max Particles: 20
- Emission Rate: 10
- Shape: Box (bounds of card)
- Color over Lifetime: Fade out
- Renderer: Billboard, Additive blend

Initially: Stop (plays on hover)
```

13. **Configure ProductCardEnhanced**
```
Select ProductCard root → Inspector
Assign all references:
- Product Image: ProductImage
- Product Name: ProductName
- Category Text: Category
- Price Text: PriceLabel
- Badge 3D: Badge3D GameObject
- Quick View Button: QuickViewButton
- Favorite Button: FavoriteButton
- Heart Icon: HeartIcon
- Try On Button: TryOnButton
- Card Background: CardBackground Image
- Glow Effect: GlowEffect
- Shimmer Particles: ShimmerParticles

Settings:
- Hover Scale: 1.05
- Hover Duration: 0.2
- Click Scale: 0.95
- Category Colors: (configure 5-6 colors)
```

14. **Save Prefab**

---

### 2.4 Modal Templates

Criar 3 modais:

**A) Product Modal**
```
ProductModal
├── Backdrop (Image)
├── ModalWindow (Image)
│   ├── CloseButton (Button)
│   ├── ProductImage (RawImage)
│   ├── ProductInfo (VerticalLayout)
│   │   ├── Title
│   │   ├── Description
│   │   ├── Price
│   │   ├── SizeSelector (Dropdown)
│   │   └── ColorSelector (HorizontalLayout of buttons)
│   └── ActionButtons (HorizontalLayout)
│       ├── TryOnButton
│       └── BuyButton
```

**B) Confirm Modal**
```
ConfirmModal
├── Backdrop
└── ModalWindow
    ├── CloseButton
    ├── Icon (Image)
    ├── Title (TMP)
    ├── Message (TMP)
    └── ButtonContainer
        ├── CancelButton
        └── ConfirmButton
```

**C) Avatar Modal**
```
AvatarModal
├── Backdrop
└── ModalWindow
    ├── CloseButton
    ├── Title: "Criar Avatar"
    ├── Instructions (TMP)
    ├── UserIdInput (TMP_InputField)
    ├── PhotoInputs (VerticalLayout)
    │   ├── FrontPhotoContainer
    │   │   ├── Label: "Foto Frontal"
    │   │   ├── PreviewImage
    │   │   └── SelectButton
    │   └── SidePhotoContainer
    │       ├── Label: "Foto Lateral"
    │       ├── PreviewImage
    │       └── SelectButton
    ├── ProgressBar (initially hidden)
    └── SubmitButton
```

**⚠️ Instruções rápidas** (detalhamento completo seria muito longo):
- Backdrop: Fill screen, Black 80% opacity, blur material
- Modal Window: Center screen, 600x700, White, rounded corners, shadow
- Buttons: Lilac background, White text, hover scale 1.05
- Add UIAnimator: SlideFromBottom, EaseOut, 0.3s

---

## 🔧 Fase 3: Configurar Scene

### 3.1 Hierarquia da Scene

```
MainScene
└── UICanvas (UIManagerEnhanced attached)
    ├── BackgroundImage (full screen gradient)
    ├── TransitionParticles (ParticleSystem)
    ├── Sidebar
    │   ├── Logo
    │   └── Navigation
    │       ├── AvatarButton
    │       ├── CatalogButton
    │       ├── ShopButton
    │       └── CartButton
    ├── TopBar
    │   ├── SearchBar
    │   ├── SettingsButton
    │   ├── NotificationsButton
    │   └── ProfileButton
    ├── Content
    │   ├── AvatarPanel
    │   ├── CatalogPanel
    │   │   ├── FiltersSection (ProductFilterSystem)
    │   │   └── ProductGrid (Grid Layout)
    │   ├── ShopPanel
    │   └── CartPanel
    ├── ToastContainer (top-right)
    ├── LoadingOverlay (LoadingPanel prefab instance)
    └── ModalContainer (ModalSystem)
```

### 3.2 Criar Sidebar

```
1. Create Empty → Name: Sidebar
   RectTransform:
   - Anchors: Left-Stretch
   - Width: 80
   - Pivot: (0, 0.5)

2. Add: Vertical Layout Group
   - Spacing: 20
   - Padding: Top 100
   - Child Alignment: Upper Center

3. Create 4 Navigation Buttons:
   For each (Avatar, Catalog, Shop, Cart):
   
   - UI → Button
   - Width: 60, Height: 60
   - Background: Transparent
   - Add child Image (icon)
     * Size: 40x40
     * Color: Gray
   - Add child TMP (label)
     * Font Size: 10
     * Color: Gray
     * Position below icon

4. Add UIAnimator to each button:
   - Animation: Bounce
   - Play on Enable: No (trigger manualmente)
```

### 3.3 Criar Top Bar

```
1. Create Empty → Name: TopBar
   RectTransform:
   - Anchors: Top-Stretch
   - Height: 70
   - Left: 80 (after sidebar)

2. Add: Horizontal Layout Group
   - Padding: All 15
   - Spacing: 15
   - Child Alignment: Middle Left

3. SearchBar:
   - TMP_InputField
   - Width: 400
   - Placeholder: "Buscar produtos..."
   - Background: White 20% opacity

4. Spacer:
   - Empty GameObject
   - Layout Element: Flexible Width

5. Three Buttons (Settings, Notifications, Profile):
   - Button with icon
   - Size: 50x50
   - Icon: 30x30
   - Add UIAnimator (Bounce on click)
```

### 3.4 Criar Painéis

**Avatar Panel:**
```
1. Empty GameObject → Name: AvatarPanel
2. RectTransform: Fill
3. Add sections:
   - AvatarPreview (3D viewport)
   - Controls (rotate, zoom buttons)
   - CreateButton (large, centered)
   - InfoText (instructions)
```

**Catalog Panel:**
```
1. Empty GameObject → Name: CatalogPanel
2. Children:

   A) FiltersSection (left, width 300):
      - Add ProductFilterSystem script
      - SearchInput (TMP_InputField)
      - CategoryDropdown (TMP_Dropdown)
      - PriceSliders (2 sliders for min/max)
      - Toggles (Has3D, InStock)
      - SortDropdown
      - ClearFiltersButton
      - ResultsCountText
   
   B) ProductGrid (right, fill):
      - Add: Grid Layout Group
      - Cell Size: 320x440
      - Spacing: 20x20
      - Constraint: Fixed Column Count = 3
      - Child Alignment: Upper Left
      - Add: Scroll Rect (for scrolling)
```

**Shop Panel:**
```
(Placeholder for now)
- Text: "Loja em breve!"
```

**Cart Panel:**
```
(Placeholder for now)
- Text: "Carrinho em breve!"
```

### 3.5 Configurar UIManagerEnhanced

```
Select UICanvas → Add Component: UIManagerEnhanced

Assign in Inspector:
- Panels array (size 4):
  [0] = AvatarPanel
  [1] = CatalogPanel
  [2] = ShopPanel
  [3] = CartPanel

- Navigation Indicators array (size 4):
  [0] = AvatarButton
  [1] = CatalogButton
  [2] = ShopButton
  [3] = CartButton

- Background Gradient: BackgroundImage
- Transition Particles: TransitionParticles

- Top Bar Buttons:
  * Settings Button: SettingsButton
  * Notifications Button: NotificationsButton
  * Profile Button: ProfileButton

- System References:
  * Modal System: ModalContainer (add ModalSystem script)
  * Filter System: FiltersSection (add ProductFilterSystem script)
  * Loading Indicator: LoadingOverlay

- Existing References:
  * Avatar Manager: (find in scene)
  * Catalog Loader: (find in scene)
  * Try On Controller: (find in scene)

- Settings:
  * Default Panel: 1 (Catalog)
  * Panel Transition Duration: 0.3
```

---

## 🔗 Fase 4: Integração

### 4.1 Integrar com CatalogLoader

Editar `CatalogLoader.cs`:

```csharp
using Barbara.UI;

public class CatalogLoader : MonoBehaviour
{
    [SerializeField] private ProductFilterSystem filterSystem;
    [SerializeField] private Transform productGrid;
    [SerializeField] private GameObject productCardPrefab;
    
    private List<ProductData> allProducts = new List<ProductData>();
    
    private async void Start()
    {
        LoadingIndicator.ShowGlobal(LoadingIndicator.LoadingStyle.Spinner, "Carregando catálogo...");
        
        await LoadCatalog();
        
        LoadingIndicator.HideGlobal();
    }
    
    private async Task LoadCatalog()
    {
        try
        {
            var response = await APIClient.Get("/api/catalog");
            allProducts = JsonConvert.DeserializeObject<List<ProductData>>(response);
            
            // Configurar filtros
            filterSystem.SetProducts(allProducts);
            filterSystem.ProductGridUpdated += OnProductsFiltered;
            
            // Exibir todos inicialmente
            DisplayProducts(allProducts);
            
            ToastNotification.Success($"{allProducts.Count} produtos carregados!");
        }
        catch (Exception e)
        {
            ToastNotification.Error("Erro ao carregar produtos: " + e.Message);
        }
    }
    
    private void OnProductsFiltered(List<ProductData> filteredProducts)
    {
        DisplayProducts(filteredProducts);
    }
    
    private void DisplayProducts(List<ProductData> products)
    {
        // Limpar grid
        foreach (Transform child in productGrid)
        {
            Destroy(child.gameObject);
        }
        
        // Criar cards
        foreach (var product in products)
        {
            var cardObj = Instantiate(productCardPrefab, productGrid);
            var card = cardObj.GetComponent<ProductCardEnhanced>();
            card.Setup(product);
        }
    }
}
```

### 4.2 Configurar ModalSystem

```csharp
// No UICanvas, create empty GameObject: ModalContainer
// Add ModalSystem script
// Assign in Inspector:
// - Modal Panel: (full screen panel)
// - Backdrop: (full screen image)
// - Modal Content: (center container)
// - Close Button: (X button)
// - Product Modal Template: (prefab)
// - Confirm Modal Template: (prefab)
// - Avatar Modal Template: (prefab)
```

### 4.3 Configurar ToastNotification

```csharp
// No UICanvas, create:
// GameObject → Empty → Name: ToastContainer
// RectTransform:
// - Anchors: Top-Right
// - Pivot: (1, 1)
// - Pos X: -20, Pos Y: -20

// Add Component: Vertical Layout Group
// - Spacing: 10
// - Child Alignment: Upper Right

// Assign to ToastNotification prefab:
// - Toast Container: ToastContainer
// - Toast Prefab: Toast prefab
```

---

## ✨ Fase 5: Polish

### 5.1 Ajustar Cores

Criar Material com shader de blur para glassmorphism:
```
Assets → Create → Material → UI_Blur
Shader: UI/Default (or custom blur shader)
```

Aplicar em:
- Card backgrounds (30% opacity)
- Modal backgrounds (50% opacity)
- Top bar (20% opacity)

### 5.2 Testar Responsividade

Testar em resoluções:
- 1920x1080 (Full HD)
- 1280x720 (HD)
- 1024x768 (4:3)
- 750x1334 (Mobile Portrait)

Ajustar:
- Canvas Scaler Match value
- Font sizes
- Grid layout cell sizes

### 5.3 Configurar Particle Systems

**Transition Particles:**
```
Settings:
- Duration: 1
- Looping: No
- Start Lifetime: 0.5-1.5
- Start Speed: 2-5
- Start Size: 0.1-0.3
- Start Color: Lilac gradient
- Emission: Burst 20-30
- Shape: Cone
- Velocity over Lifetime: Upwards
- Color over Lifetime: Fade out
```

**Shimmer Particles (per card):**
```
Settings:
- Duration: 2
- Looping: Yes
- Start Lifetime: 1-2
- Start Speed: 0.2
- Start Size: 0.05-0.15
- Max Particles: 10
- Emission Rate: 5
- Shape: Box (card bounds)
- Color over Lifetime: Pulse
```

---

## 🧪 Testing Checklist

### Funcionalidade
- [ ] Navegar entre painéis (1-4)
- [ ] Buscar produtos
- [ ] Filtrar por categoria
- [ ] Filtrar por preço
- [ ] Ordenar produtos
- [ ] Hover em product card
- [ ] Favoritar produto
- [ ] Quick view
- [ ] Try on
- [ ] Abrir modal de produto
- [ ] Confirmar ações
- [ ] Criar avatar
- [ ] Toasts aparecem
- [ ] Loading indicators

### Performance
- [ ] 60 FPS durante animações
- [ ] Sem lag ao filtrar
- [ ] Busca com debounce funciona
- [ ] Cards carregam rápido
- [ ] Imagens carregam async

### Visual
- [ ] Cores consistentes
- [ ] Fontes legíveis
- [ ] Espaçamentos adequados
- [ ] Responsivo
- [ ] Animações suaves

---

## 🎉 Conclusão

Após completar todos os passos, você terá:

✅ **7 componentes UI** funcionais e integrados
✅ **Sistema de navegação** fluido
✅ **Filtros e busca** avançados
✅ **Animações** profissionais
✅ **Feedback visual** rico
✅ **UX moderna** e intuitiva

**Seu frontend estará pronto para impressionar!** 🚀

---

## 🆘 Troubleshooting

**Cards não aparecem:**
- Verificar se productCardPrefab está atribuído
- Verificar se CatalogLoader tem referência ao grid
- Verificar console por erros de NullReference

**Animações não funcionam:**
- Verificar se UIAnimator está adicionado
- Verificar se CanvasGroup existe
- Verificar se playOnEnable está marcado

**Toasts não aparecem:**
- Verificar se ToastNotification.Instance não é null
- Verificar se toastContainer está atribuído
- Verificar se toastPrefab existe

**Loading não aparece:**
- Verificar se LoadingIndicator.Global não é null
- Verificar se loadingPanel está ativo na scene
- Usar SetActive(true) antes de Show()

---

*Guia criado em: 6 de novembro de 2025*
*Versão: 1.0.0*
*Tempo estimado de implementação: 2-3 horas*

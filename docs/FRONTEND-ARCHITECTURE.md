# 🏗️ Arquitetura de Componentes - Frontend Bárbara

## 📐 Diagrama de Componentes

```
┌─────────────────────────────────────────────────────────────────┐
│                         UICanvas                                │
│                    (UIManagerEnhanced)                          │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌──────────┐  ┌───────────────────────────────────────────┐  │
│  │          │  │              Top Bar                      │  │
│  │          │  │  [Search] [Settings] [Notify] [Profile] │  │
│  │  Sidebar │  ├───────────────────────────────────────────┤  │
│  │          │  │                                           │  │
│  │  🎭 Avatar│  │          Active Panel                    │  │
│  │  👗 Catalog│ │    (Avatar/Catalog/Shop/Cart)           │  │
│  │  🛍️ Shop  │  │                                          │  │
│  │  🛒 Cart  │  │                                          │  │
│  │          │  │                                           │  │
│  └──────────┘  └───────────────────────────────────────────┘  │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐  │
│  │           Toast Container (Top-Right)                   │  │
│  │  🟢 Success toast                                       │  │
│  │  🔴 Error toast                                         │  │
│  └─────────────────────────────────────────────────────────┘  │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐  │
│  │           Loading Overlay (Full Screen)                 │  │
│  │  [■■■■■■░░░░] 60% Carregando...                        │  │
│  └─────────────────────────────────────────────────────────┘  │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐  │
│  │           Modal Container (Full Screen)                 │  │
│  │  ░░░░░░░ Backdrop ░░░░░░░                              │  │
│  │      ┌─────────────────────┐                           │  │
│  │      │  Modal Window [X]   │                           │  │
│  │      │  Content Here...    │                           │  │
│  │      │  [Cancel] [OK]      │                           │  │
│  │      └─────────────────────┘                           │  │
│  └─────────────────────────────────────────────────────────┘  │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## 🔗 Hierarquia de GameObjects

```
MainScene
└── UICanvas (Canvas + UIManagerEnhanced)
    ├── BackgroundImage
    ├── TransitionParticles
    │
    ├── Sidebar
    │   ├── Logo
    │   └── Navigation
    │       ├── AvatarButton (+ UIAnimator)
    │       ├── CatalogButton (+ UIAnimator)
    │       ├── ShopButton (+ UIAnimator)
    │       └── CartButton (+ UIAnimator)
    │
    ├── TopBar
    │   ├── SearchBar (TMP_InputField)
    │   ├── Spacer
    │   ├── SettingsButton (+ UIAnimator)
    │   ├── NotificationsButton (+ UIAnimator)
    │   └── ProfileButton (+ UIAnimator)
    │
    ├── ContentArea
    │   ├── AvatarPanel (active: true)
    │   │   ├── AvatarPreview
    │   │   ├── Controls
    │   │   ├── CreateAvatarButton
    │   │   └── InfoText
    │   │
    │   ├── CatalogPanel (active: false)
    │   │   ├── FiltersSection (+ ProductFilterSystem)
    │   │   │   ├── SearchInput
    │   │   │   ├── CategoryDropdown
    │   │   │   ├── PriceSliders
    │   │   │   ├── Toggles (Has3D, InStock)
    │   │   │   ├── SortDropdown
    │   │   │   ├── ClearButton
    │   │   │   └── ResultsCount
    │   │   │
    │   │   └── ProductGrid (ScrollView)
    │   │       └── Content (GridLayoutGroup)
    │   │           ├── ProductCard #1 (prefab instance)
    │   │           ├── ProductCard #2 (prefab instance)
    │   │           └── ... (dynamic)
    │   │
    │   ├── ShopPanel (active: false)
    │   │   └── PlaceholderText
    │   │
    │   └── CartPanel (active: false)
    │       └── PlaceholderText
    │
    ├── ToastContainer
    │   ├── Toast #1 (prefab instance, if active)
    │   ├── Toast #2 (prefab instance, if active)
    │   └── Toast #3 (prefab instance, if active)
    │
    ├── LoadingOverlay (LoadingPanel prefab instance)
    │   ├── Backdrop
    │   └── Content
    │       ├── Spinner
    │       ├── ProgressBar
    │       ├── DotsContainer
    │       ├── PulseImage
    │       └── StatusText
    │
    └── ModalContainer (+ ModalSystem)
        ├── ModalPanel
        ├── Backdrop
        ├── ModalContent
        └── CloseButton
```

---

## 🧩 Relacionamento entre Componentes

```
┌──────────────────────────────────────────────────────────────┐
│                    UIManagerEnhanced                         │
│                   (Orquestrador Principal)                   │
└───────────┬──────────────────────────────────────────────────┘
            │
            ├─────────────┐
            │             │
┌───────────▼───┐   ┌────▼──────────┐
│ ModalSystem   │   │ ProductFilter │
│               │   │ System        │
│ - Product     │   │               │
│ - Confirm     │   │ Event:        │
│ - Avatar      │   │ GridUpdated   │
└───────┬───────┘   └────┬──────────┘
        │                │
        │                │
┌───────▼────────────────▼──────┐
│      ToastNotification        │
│      (Global Singleton)       │
└───────┬───────────────────────┘
        │
        │
┌───────▼───────────────────────┐
│      LoadingIndicator         │
│      (Global Singleton)       │
└───────────────────────────────┘


┌──────────────────────────────────────────────────────────────┐
│                  ProductCardEnhanced                         │
│                                                              │
│  Usa:                                                        │
│  ├─ UIAnimator (hover, click animations)                    │
│  ├─ ModalSystem (ShowProductModal)                          │
│  ├─ LoadingIndicator (try-on feedback)                      │
│  ├─ ToastNotification (favorite feedback)                   │
│  └─ TryOnController (apply clothing)                        │
└──────────────────────────────────────────────────────────────┘


┌──────────────────────────────────────────────────────────────┐
│                     UIAnimator                               │
│                  (Usado por TODOS)                           │
│                                                              │
│  - Painéis (fade in/out)                                    │
│  - Cards (scale in, hover)                                  │
│  - Buttons (bounce, hover)                                  │
│  - Modals (slide from bottom)                               │
│  - Toasts (slide from top)                                  │
└──────────────────────────────────────────────────────────────┘
```

---

## 🔄 Fluxo de Dados

### Carregamento de Catálogo
```
CatalogLoader
    ↓ (API call)
Backend API
    ↓ (JSON response)
ProductData[]
    ↓ (SetProducts)
ProductFilterSystem
    ↓ (event: ProductGridUpdated)
UIManagerEnhanced / CatalogPanel
    ↓ (instantiate cards)
ProductCardEnhanced instances
```

### Try-On Flow
```
ProductCardEnhanced
    ↓ (OnTryOnClick)
LoadingIndicator.ShowGlobal()
    ↓
TryOnController.ApplyClothing()
    ↓ (API call)
Backend API
    ↓ (response)
TryOnController (apply 3D model)
    ↓
LoadingIndicator.HideGlobal()
    ↓
ToastNotification.Success()
    ↓
UIManagerEnhanced.ShowPanel(0) // Avatar
```

### Filtros
```
User input (search/category/price/toggles)
    ↓
ProductFilterSystem (debounce for search)
    ↓ (ApplyFilters)
LINQ filtering + sorting
    ↓ (event)
ProductGridUpdated(filteredProducts)
    ↓
CatalogPanel listener
    ↓
Clear grid + instantiate new cards
```

### Modal Workflow
```
User action (click product)
    ↓
ModalSystem.ShowProductModal(product)
    ↓ (instantiate template)
Modal Window
    ↓ (setup content)
UIAnimator.Play() (slide in)
    ↓ (user actions)
Try On / Buy buttons
    ↓ (close)
ModalSystem.Close()
    ↓
UIAnimator.PlayReverse() (slide out)
```

---

## 🎭 Estados dos Componentes

### UIManagerEnhanced States
```
┌─────────┐
│  Init   │ → ShowWelcomeSequence()
└────┬────┘
     │
     ▼
┌─────────┐
│ Panel 0 │ ⟷ Panel 1 ⟷ Panel 2 ⟷ Panel 3
│ Avatar  │   Catalog   Shop      Cart
└─────────┘
     ▲
     │ ShowAvatarCreation()
     │
┌─────────┐
│  Modal  │
│ Opened  │
└─────────┘
```

### ProductCard States
```
     ┌────────┐
     │ Normal │ (idle state)
     └───┬────┘
         │
    ┌────▼────┐
    │  Hover  │ (mouse over)
    └────┬────┘
         │
    ┌────▼────┐
    │ Clicked │ (press animation)
    └────┬────┘
         │
    ┌────▼────┐
    │ Loading │ (try-on processing)
    └────┬────┘
         │
    ┌────▼────┐
    │ Success │ (applied)
    └─────────┘
         │
         ▼
     Back to Normal


    ┌─────────────┐
    │  Favorited  │ (toggle state)
    └─────────────┘
```

### Toast Queue
```
Queue: []
    ↓ (Success toast added)
Queue: [Toast1]
    ↓ (Error toast added)
Queue: [Toast1, Toast2]
    ↓ (Info toast added)
Queue: [Toast1, Toast2, Toast3] (MAX)
    ↓ (another toast tries to add)
Queue: [Toast1, Toast2, Toast3] + pending [Toast4]
    ↓ (Toast1 finishes, removed)
Queue: [Toast2, Toast3, Toast4]
```

---

## 🎨 Dependency Graph

```
                    ┌──────────────┐
                    │   Unity UI   │
                    │   TextMeshPro│
                    └──────┬───────┘
                           │
        ┌──────────────────┼──────────────────┐
        │                  │                  │
   ┌────▼────┐     ┌──────▼─────┐     ┌─────▼──────┐
   │UIAnimator│     │ToastNotif. │     │LoadingInd. │
   └────┬────┘     └──────┬─────┘     └─────┬──────┘
        │                  │                  │
        └─────────┬────────┴────────┬─────────┘
                  │                 │
          ┌───────▼────┐    ┌──────▼───────┐
          │ModalSystem │    │ProductCard   │
          │            │    │Enhanced      │
          └───────┬────┘    └──────┬───────┘
                  │                 │
          ┌───────▼─────────────────▼───────┐
          │   ProductFilterSystem            │
          └───────┬──────────────────────────┘
                  │
          ┌───────▼────────┐
          │UIManager       │
          │Enhanced        │
          └────────────────┘
```

### External Dependencies
```
UIManagerEnhanced
    ├─ AvatarManager (existing)
    ├─ CatalogLoader (existing)
    ├─ TryOnController (existing)
    └─ APIClient (existing)

ProductCardEnhanced
    ├─ TryOnController
    └─ ProductData struct

CatalogLoader
    ├─ APIClient
    └─ ProductData struct
```

---

## 📦 Component Interfaces

### UIAnimator
```csharp
public class UIAnimator : MonoBehaviour
{
    // Public Methods
    public void Play()
    public void PlayReverse()
    public void Reset()
    
    // Public Properties
    public AnimationType animationType
    public EasingType easingType
    public float duration
    public float delay
}
```

### ToastNotification
```csharp
public class ToastNotification : MonoBehaviour
{
    // Static Singleton
    public static ToastNotification Instance
    
    // Static Methods
    public static void Success(string message)
    public static void Error(string message)
    public static void Warning(string message)
    public static void Info(string message)
    public static void Show(string msg, ToastType type, float duration)
}
```

### LoadingIndicator
```csharp
public class LoadingIndicator : MonoBehaviour
{
    // Static Global Access
    public static LoadingIndicator Global
    
    // Static Methods
    public static void ShowGlobal(LoadingStyle style, string message)
    public static void HideGlobal()
    public static void UpdateProgress(float progress)
    
    // Instance Methods
    public void Show()
    public void Hide()
    public void SetProgress(float progress)
}
```

### ProductCardEnhanced
```csharp
public class ProductCardEnhanced : MonoBehaviour
{
    // Public Methods
    public void Setup(ProductData product)
    public void OnQuickViewClick()
    public void OnFavoriteClick()
    public void OnTryOnClick()
    
    // Public Properties
    public bool isFavorited
    public ProductData currentProduct
}
```

### ModalSystem
```csharp
public class ModalSystem : MonoBehaviour
{
    // Static Singleton
    public static ModalSystem Instance
    
    // Public Methods
    public void ShowProductModal(ProductData product)
    public void ShowConfirmModal(string title, string message, 
                                 Action onConfirm, Action onCancel)
    public void ShowAvatarCreationModal(Action<string, string, string> onSubmit)
    public void Close()
}
```

### ProductFilterSystem
```csharp
public class ProductFilterSystem : MonoBehaviour
{
    // Public Methods
    public void SetProducts(List<ProductData> products)
    public void ApplyFilters()
    public void ClearAllFilters()
    
    // Public Events
    public event Action<List<ProductData>> ProductGridUpdated
    
    // Public Properties
    public List<ProductData> filteredProducts
}
```

### UIManagerEnhanced
```csharp
public class UIManagerEnhanced : MonoBehaviour
{
    // Public Methods
    public void ShowPanel(int panelIndex)
    public void ShowAvatarCreation()
    
    // Public Properties
    public int currentPanelIndex
}
```

---

## 🔐 Access Patterns

### Singleton Access
```csharp
// Toast
ToastNotification.Success("Message");

// Loading
LoadingIndicator.ShowGlobal(style, "Message");

// Modal
ModalSystem.Instance.ShowProductModal(product);
```

### Component References
```csharp
// Via Inspector
[SerializeField] private UIAnimator animator;
[SerializeField] private ProductFilterSystem filterSystem;

// Find in scene
var uiManager = FindObjectOfType<UIManagerEnhanced>();
```

### Event Subscription
```csharp
// Subscribe
filterSystem.ProductGridUpdated += OnProductsFiltered;

// Unsubscribe
filterSystem.ProductGridUpdated -= OnProductsFiltered;
```

---

## 🎯 Responsabilidades

| Componente | Responsabilidade |
|------------|------------------|
| **UIManagerEnhanced** | Navegação, orquestração, fluxos principais |
| **UIAnimator** | Animações de UI elements |
| **ToastNotification** | Feedback de sucesso/erro/info/warning |
| **LoadingIndicator** | Estados de carregamento e progresso |
| **ProductCardEnhanced** | Exibição e interação com produtos |
| **ModalSystem** | Dialogs e modais customizados |
| **ProductFilterSystem** | Busca, filtragem e ordenação |

---

**🏗️ Arquitetura limpa, modular e escalável!**

*Arquitetura v1.0.0*
*Data: 6 de novembro de 2025*

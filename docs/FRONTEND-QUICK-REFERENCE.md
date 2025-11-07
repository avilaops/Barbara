# 🎨 Quick Reference - Componentes UI

## 📖 Guia Visual Rápido

---

## 🎬 UIAnimator

```csharp
// SETUP
animator.animationType = UIAnimator.AnimationType.FadeIn;
animator.easingType = UIAnimator.EasingType.EaseOut;
animator.duration = 0.3f;

// USAR
animator.Play();                          // ▶️ Executar
animator.PlayReverse();                   // ◀️ Reverter
animator.Reset();                         // 🔄 Reset
```

### 10 Tipos de Animação
```
FadeIn      ╱╲     Aparece gradualmente
FadeOut     ╲╱     Desaparece gradualmente
ScaleIn     ⊙→●    Cresce do centro
ScaleOut    ●→⊙    Encolhe ao centro
SlideLeft   ←──    Desliza da direita
SlideRight  ──→    Desliza da esquerda
SlideUp     ↑      Desliza de baixo
SlideDown   ↓      Desliza de cima
Bounce      ↕↕↕    Pula elasticamente
Shake       ≈≈≈    Treme horizontalmente
```

### 6 Tipos de Easing
```
Linear      ────   Velocidade constante
EaseIn      ╱──    Acelera no início
EaseOut     ──╲    Desacelera no final
EaseInOut   ╱──╲   Suave início e fim
Elastic     ≋≋≋    Efeito mola
Bounce      ⤴⤵    Quica no final
```

---

## 🔔 ToastNotification

```csharp
// 4 TIPOS
ToastNotification.Success("✓ Sucesso!");    // 🟢 Verde
ToastNotification.Error("✗ Erro!");         // 🔴 Vermelho
ToastNotification.Warning("⚠ Atenção!");    // 🟡 Amarelo
ToastNotification.Info("ℹ Info");          // 🔵 Azul

// COM DURAÇÃO
ToastNotification.Show("Mensagem", ToastType.Info, 5f);
```

### Visual Reference
```
┌─────────────────────────────┐
│ 🟢 Operação concluída!      │  ← Success (verde)
└─────────────────────────────┘

┌─────────────────────────────┐
│ 🔴 Erro ao processar        │  ← Error (vermelho)
└─────────────────────────────┘

┌─────────────────────────────┐
│ 🟡 Atenção necessária       │  ← Warning (amarelo)
└─────────────────────────────┘

┌─────────────────────────────┐
│ 🔵 Informação útil          │  ← Info (azul)
└─────────────────────────────┘
```

---

## ⏳ LoadingIndicator

```csharp
// MOSTRAR
LoadingIndicator.ShowGlobal(style, "Mensagem");

// PROGRESSO
LoadingIndicator.UpdateProgress(0.5f);    // 50%

// ESCONDER
LoadingIndicator.HideGlobal();
```

### 5 Estilos Visuais
```
Spinner      ⟳      Círculo girando
                    
ProgressBar  ▓▓▓░░  Barra 0-100%
                    
Skeleton     ▬▬▬    Placeholder pulsando
             ▬▬▬
             
Dots         • • •  Pontos animados
                    
Pulse        ◉      Círculo respirando
```

### Exemplo de Uso
```csharp
// Início
LoadingIndicator.ShowGlobal(
    LoadingIndicator.LoadingStyle.ProgressBar,
    "Carregando..."
);

// Durante
for (int i = 0; i <= 100; i += 10) {
    LoadingIndicator.UpdateProgress(i / 100f);
    await Task.Delay(200);
}

// Fim
LoadingIndicator.HideGlobal();
```

---

## 🃏 ProductCardEnhanced

```csharp
// SETUP
card.Setup(productData);

// EVENTOS
card.OnQuickViewClick();    // 👁️ Preview rápido
card.OnFavoriteClick();     // ❤️ Toggle favorito
card.OnTryOnClick();        // 👗 Experimentar
```

### Estrutura Visual
```
┌─────────────────────────┐
│  [Imagem do Produto]    │  ← RawImage
│                         │
│  👁️ Quick View    ❤️   │  ← Hover buttons
│                         │
│  🏷️ 3D                  │  ← Badge (se tem 3D)
├─────────────────────────┤
│  Nome do Produto        │  ← TMP Text
│  Categoria              │  ← TMP Text Small
│                         │
│  R$ 99,90  [Experimentar]│  ← Price + Button
└─────────────────────────┘

HOVER EFFECTS:
- Scale 1.05x
- Glow effect
- Shimmer particles
- Quick view appears
```

### Estados
```
NORMAL       📦  Sem hover
HOVER        ✨  Mouse em cima (glow + scale)
FAVORITED    ❤️  Heart preenchido
LOADING      ⟳   Aplicando no avatar
```

---

## 🪟 ModalSystem

```csharp
// PRODUTO
ModalSystem.Instance.ShowProductModal(product);

// CONFIRMAÇÃO
ModalSystem.Instance.ShowConfirmModal(
    "Título",
    "Mensagem",
    onConfirm: () => {},
    onCancel: () => {}
);

// AVATAR
ModalSystem.Instance.ShowAvatarCreationModal((id, front, side) => {});

// FECHAR
ModalSystem.Instance.Close();
```

### Layout Visual
```
┌─────────────────────────────────────┐
│░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░│  ← Backdrop (blur)
│░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░│
│░░░░░┌─────────────────────┐░░░░░░░│
│░░░░░│   [X]               │░░░░░░░│  ← Modal Window
│░░░░░│                     │░░░░░░░│
│░░░░░│   Conteúdo Aqui    │░░░░░░░│
│░░░░░│                     │░░░░░░░│
│░░░░░│   [Cancelar] [OK]  │░░░░░░░│
│░░░░░└─────────────────────┘░░░░░░░│
│░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░│
└─────────────────────────────────────┘
```

### 3 Templates
```
ProductModal     👗  Detalhes do produto
ConfirmModal     ❓  Confirmação de ação
AvatarModal      📸  Upload de fotos
```

---

## 🔍 ProductFilterSystem

```csharp
// CONFIGURAR
filterSystem.SetProducts(products);

// ESCUTAR MUDANÇAS
filterSystem.ProductGridUpdated += (filteredProducts) => {
    UpdateGrid(filteredProducts);
};

// LIMPAR
filterSystem.ClearAllFilters();
```

### UI Components
```
┌─────────────────────────┐
│  🔍 [Buscar produtos]   │  ← Search (debounced)
│                         │
│  📁 Categoria: [▼]      │  ← Dropdown
│                         │
│  💰 Preço               │  ← Sliders
│  Min: ─●────── R$ 50   │
│  Max: ───────●─ R$ 200 │
│                         │
│  ☑ Apenas com modelo 3D │  ← Toggles
│  ☑ Apenas em estoque    │
│                         │
│  📊 Ordenar: [▼]        │  ← Sort dropdown
│                         │
│  [🗑️ Limpar Filtros]   │  ← Clear button
│                         │
│  📝 125 produtos        │  ← Results count
└─────────────────────────┘
```

### Filtros Disponíveis
```
Search       🔍  Busca em nome, descrição, categoria
Category     📁  Todas, Vestidos, Camisetas, Calças, etc.
Price        💰  Range min-max com sliders
Has3D        🎮  Toggle para apenas com modelo 3D
InStock      ✅  Toggle para apenas em estoque
Sort         📊  5 opções (nome asc/desc, preço, novo)
```

---

## 🎮 UIManagerEnhanced

```csharp
// MUDAR PAINEL
uiManager.ShowPanel(0);  // Avatar
uiManager.ShowPanel(1);  // Catalog
uiManager.ShowPanel(2);  // Shop
uiManager.ShowPanel(3);  // Cart

// ATALHOS DE TECLADO
// Pressione 1, 2, 3 ou 4

// CRIAR AVATAR
uiManager.ShowAvatarCreation();
```

### Layout Completo
```
┌────┬──────────────────────────────────────────────┐
│    │  [🔍 Search]  [⚙️] [🔔] [👤]                │  ← Top Bar
├────┼──────────────────────────────────────────────┤
│ 🎭 │                                             │
│ 👗 │          CONTENT AREA                       │  ← Active Panel
│ 🛍️ │          (Avatar/Catalog/Shop/Cart)         │
│ 🛒 │                                             │
│    │                                             │
│ ↑  │                                             │
│Nav │                                             │
│Bar │                                             │
└────┴──────────────────────────────────────────────┘
```

### 4 Painéis
```
0️⃣ AvatarPanel   🎭  Visualizar e customizar avatar
1️⃣ CatalogPanel  👗  Browse produtos com filtros
2️⃣ ShopPanel     🛍️  Loja e histórico
3️⃣ CartPanel     🛒  Carrinho de compras
```

---

## 🎨 Design Tokens

### Cores
```
Primary    #C29AFF  ████  Lilac (botões, highlights)
Success    #33CC66  ████  Green (confirmações)
Error      #E64D4D  ████  Red (erros)
Warning    #FFB347  ████  Orange (avisos)
Info       #4D9BE6  ████  Blue (informações)
Text       #333333  ████  Dark Gray (texto principal)
Subtle     #999999  ████  Gray (texto secundário)
Background #FAFAFA  ████  Off-White
```

### Tipografia
```
Heading    Poppins Bold      28px, 22px, 18px
Body       Inter Regular     16px, 14px
Small      Inter Regular     12px
Button     Inter Semi-Bold   16px, 14px
```

### Espaçamentos
```
xs   4px   ▫
sm   8px   ▫▫
md   16px  ▫▫▫▫
lg   24px  ▫▫▫▫▫▫
xl   32px  ▫▫▫▫▫▫▫▫
2xl  48px  ▫▫▫▫▫▫▫▫▫▫▫▫
```

### Animações
```
Fast      0.1s  ━
Normal    0.3s  ━━━
Slow      0.5s  ━━━━━

Easing:
- Interface: EaseOut
- Feedback:  EaseInOut
- Playful:   Elastic/Bounce
```

---

## 🚀 Common Patterns

### Pattern 1: Loading + Success/Error
```csharp
LoadingIndicator.ShowGlobal(style, "Processando...");

try {
    var result = await APICall();
    LoadingIndicator.HideGlobal();
    ToastNotification.Success("✓ Sucesso!");
}
catch (Exception e) {
    LoadingIndicator.HideGlobal();
    ToastNotification.Error($"✗ Erro: {e.Message}");
}
```

### Pattern 2: Confirm → Action → Feedback
```csharp
ModalSystem.Instance.ShowConfirmModal(
    "Confirmar",
    "Tem certeza?",
    onConfirm: async () => {
        LoadingIndicator.ShowGlobal(style, "Processando...");
        await PerformAction();
        LoadingIndicator.HideGlobal();
        ToastNotification.Success("✓ Feito!");
    }
);
```

### Pattern 3: Sequential Animations
```csharp
IEnumerator AnimateSequence() {
    animator1.Play();
    yield return new WaitForSeconds(0.2f);
    
    animator2.Play();
    yield return new WaitForSeconds(0.2f);
    
    animator3.Play();
}
```

### Pattern 4: Form Submit
```csharp
async Task OnSubmit() {
    // Validar
    if (string.IsNullOrEmpty(input.text)) {
        ToastNotification.Warning("Preencha todos os campos");
        return;
    }
    
    // Enviar
    LoadingIndicator.ShowGlobal(style, "Enviando...");
    
    try {
        await API.Post(data);
        LoadingIndicator.HideGlobal();
        ToastNotification.Success("✓ Enviado!");
        ModalSystem.Instance.Close();
    }
    catch (Exception e) {
        LoadingIndicator.HideGlobal();
        ToastNotification.Error($"Erro: {e.Message}");
    }
}
```

---

## 🎯 Inspector Quick Setup

### UIAnimator
```
Animation Type:  [FadeIn ▼]
Easing Type:     [EaseOut ▼]
Duration:        0.3
Delay:           0
Play On Enable:  ☑
```

### ProductCardEnhanced
```
Product Image:      [Assign]
Product Name:       [Assign]
Category Text:      [Assign]
Price Text:         [Assign]
Badge 3D:           [Assign]
Quick View Button:  [Assign]
Favorite Button:    [Assign]
Try On Button:      [Assign]
Card Background:    [Assign]
Glow Effect:        [Assign]
Shimmer Particles:  [Assign]

Hover Scale:        1.05
Hover Duration:     0.2
Click Scale:        0.95
```

### LoadingIndicator
```
Loading Style:      [Spinner ▼]
Spinner Image:      [Assign]
Progress Bar:       [Assign]
Progress Text:      [Assign]
Dots Container:     [Assign]
Pulse Image:        [Assign]
Status Text:        [Assign]

Spin Speed:         180
Loading Messages:
  [0] "Carregando..."
  [1] "Aguarde..."
  [2] "Processando..."
```

### UIManagerEnhanced
```
Panels (4):
  [0] Avatar Panel
  [1] Catalog Panel
  [2] Shop Panel
  [3] Cart Panel

Navigation Indicators (4):
  [0] Avatar Button
  [1] Catalog Button
  [2] Shop Button
  [3] Cart Button

Default Panel:      1
Transition Duration: 0.3

Systems:
  Modal System:      [Assign]
  Filter System:     [Assign]
  Loading Indicator: [Assign]
  Avatar Manager:    [Assign]
  Catalog Loader:    [Assign]
  Try On Controller: [Assign]
```

---

## 🐛 Troubleshooting Rápido

### Problema: Cards não aparecem
```
✅ Verificar:
- productCardPrefab atribuído?
- gridContainer existe?
- products.Count > 0?
- Console tem erros?
```

### Problema: Animações não funcionam
```
✅ Verificar:
- UIAnimator adicionado?
- CanvasGroup presente?
- duration > 0?
- playOnEnable configurado?
```

### Problema: Toasts não aparecem
```
✅ Verificar:
- ToastNotification.Instance != null?
- toastContainer no Canvas?
- toastPrefab atribuído?
- Canvas ativo?
```

### Problema: Loading não some
```
✅ Verificar:
- HideGlobal() sendo chamado?
- finally block existe?
- Coroutine não foi parada?
- loadingPanel existe?
```

---

## 📚 Referência Rápida de APIs

### UIAnimator
```csharp
.Play()                          // Executar animação
.PlayReverse()                   // Reverter animação
.Reset()                         // Voltar ao inicial
.Play(callback)                  // Com callback ao fim
.animationType = ...             // Mudar tipo
.easingType = ...                // Mudar easing
.duration = float                // Duração em segundos
```

### ToastNotification
```csharp
.Success(message)                // Toast verde
.Error(message)                  // Toast vermelho
.Warning(message)                // Toast amarelo
.Info(message)                   // Toast azul
.Show(msg, type, duration)       // Toast customizado
```

### LoadingIndicator
```csharp
.ShowGlobal(style, message)      // Mostrar global
.HideGlobal()                    // Esconder global
.UpdateProgress(0.0-1.0)         // Atualizar progresso
.UpdateMessage(string)           // Mudar mensagem
.Show()                          // Mostrar local
.Hide()                          // Esconder local
```

### ProductCardEnhanced
```csharp
.Setup(ProductData)              // Configurar card
.OnQuickViewClick()              // Preview 3D
.OnFavoriteClick()               // Toggle favorite
.OnTryOnClick()                  // Aplicar no avatar
.isFavorited = bool              // Set/get favorito
```

### ModalSystem
```csharp
.ShowProductModal(product)       // Modal de produto
.ShowConfirmModal(...)           // Modal de confirmação
.ShowAvatarCreationModal(...)    // Modal de avatar
.Close()                         // Fechar modal atual
```

### ProductFilterSystem
```csharp
.SetProducts(List)               // Definir produtos
.ApplyFilters()                  // Aplicar filtros
.ClearAllFilters()               // Limpar tudo
.ProductGridUpdated += ...       // Event listener
```

### UIManagerEnhanced
```csharp
.ShowPanel(int)                  // Mudar painel
.ShowAvatarCreation()            // Modal de avatar
```

---

**📌 Salve esta página como referência rápida!**

*Quick Reference criado em: 6 de novembro de 2025*
*Versão: 1.0.0*

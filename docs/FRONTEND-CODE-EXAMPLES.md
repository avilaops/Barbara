# 💻 Exemplos de Código - Frontend Premium

## 📖 Guia Prático de Uso dos Componentes

---

## 🎬 UIAnimator - Animações

### Exemplo 1: Animar Panel ao Abrir
```csharp
using Barbara.UI;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    private UIAnimator animator;
    
    void Awake()
    {
        animator = GetComponent<UIAnimator>();
    }
    
    public void OpenPanel()
    {
        gameObject.SetActive(true);
        animator.Play();
    }
    
    public void ClosePanel()
    {
        animator.PlayReverse(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
```

### Exemplo 2: Sequência de Animações
```csharp
using System.Collections;

public class AnimationSequence : MonoBehaviour
{
    [SerializeField] private UIAnimator title;
    [SerializeField] private UIAnimator subtitle;
    [SerializeField] private UIAnimator button;
    
    private IEnumerator Start()
    {
        // Título aparece primeiro
        title.Play();
        yield return new WaitForSeconds(0.2f);
        
        // Depois o subtítulo
        subtitle.Play();
        yield return new WaitForSeconds(0.2f);
        
        // Por último o botão
        button.Play();
    }
}
```

### Exemplo 3: Animação Personalizada
```csharp
public void CustomAnimation()
{
    var animator = GetComponent<UIAnimator>();
    
    // Configurar via código
    animator.animationType = UIAnimator.AnimationType.ScaleIn;
    animator.easingType = UIAnimator.EasingType.Elastic;
    animator.duration = 0.5f;
    animator.delay = 0.1f;
    
    // Executar com callback
    animator.Play(() =>
    {
        Debug.Log("Animação concluída!");
    });
}
```

---

## 🔔 ToastNotification - Notificações

### Exemplo 1: Toasts Simples
```csharp
using Barbara.UI;

public class NotificationExamples : MonoBehaviour
{
    public void OnSaveSuccess()
    {
        ToastNotification.Success("Dados salvos com sucesso!");
    }
    
    public void OnLoadError()
    {
        ToastNotification.Error("Erro ao carregar dados");
    }
    
    public void OnWarningCondition()
    {
        ToastNotification.Warning("Alguns dados estão incompletos");
    }
    
    public void OnInfoMessage()
    {
        ToastNotification.Info("Dica: Use filtros para encontrar mais rápido");
    }
}
```

### Exemplo 2: Toast com Duração Customizada
```csharp
public void LongMessage()
{
    ToastNotification.Show(
        "Esta é uma mensagem longa que precisa de mais tempo para ser lida",
        ToastNotification.ToastType.Info,
        duration: 7f  // 7 segundos
    );
}
```

### Exemplo 3: Feedback de Operações Assíncronas
```csharp
using System.Threading.Tasks;

public async Task SaveDataAsync()
{
    try
    {
        ToastNotification.Info("Salvando dados...");
        
        await APIClient.Post("/api/save", data);
        
        ToastNotification.Success("✓ Dados salvos!");
    }
    catch (Exception e)
    {
        ToastNotification.Error($"✗ Erro: {e.Message}");
    }
}
```

### Exemplo 4: Múltiplas Notificações em Sequência
```csharp
private IEnumerator MultipleNotifications()
{
    ToastNotification.Info("Iniciando processo...");
    yield return new WaitForSeconds(1f);
    
    ToastNotification.Info("Validando dados...");
    yield return new WaitForSeconds(1f);
    
    ToastNotification.Info("Enviando para servidor...");
    yield return new WaitForSeconds(1f);
    
    ToastNotification.Success("Processo concluído!");
}
```

---

## ⏳ LoadingIndicator - Estados de Carregamento

### Exemplo 1: Loading Simples
```csharp
using Barbara.UI;

public async Task LoadProducts()
{
    // Mostrar loading
    LoadingIndicator.ShowGlobal(
        LoadingIndicator.LoadingStyle.Spinner,
        "Carregando produtos..."
    );
    
    try
    {
        var products = await APIClient.Get("/api/products");
        DisplayProducts(products);
    }
    finally
    {
        // Sempre esconder no final
        LoadingIndicator.HideGlobal();
    }
}
```

### Exemplo 2: Loading com Progresso
```csharp
public async Task UploadImages(List<Texture2D> images)
{
    LoadingIndicator.ShowGlobal(
        LoadingIndicator.LoadingStyle.ProgressBar,
        "Enviando imagens..."
    );
    
    for (int i = 0; i < images.Count; i++)
    {
        await UploadImage(images[i]);
        
        // Atualizar progresso
        float progress = (i + 1) / (float)images.Count;
        LoadingIndicator.UpdateProgress(progress);
    }
    
    LoadingIndicator.HideGlobal();
    ToastNotification.Success("Todas as imagens foram enviadas!");
}
```

### Exemplo 3: Loading com Mensagens Rotativas
```csharp
public void ShowLoadingWithMessages()
{
    LoadingIndicator.ShowGlobal(
        LoadingIndicator.LoadingStyle.Dots,
        "Processando..."
    );
    
    // Mensagens serão rotacionadas automaticamente
    // Mas você pode mudar manualmente também:
    StartCoroutine(UpdateLoadingMessages());
}

private IEnumerator UpdateLoadingMessages()
{
    string[] messages = {
        "Analisando imagem...",
        "Processando modelo 3D...",
        "Aplicando texturas...",
        "Finalizando..."
    };
    
    foreach (var message in messages)
    {
        LoadingIndicator.UpdateMessage(message);
        yield return new WaitForSeconds(2f);
    }
}
```

### Exemplo 4: Loading Skeleton (Placeholder)
```csharp
// Ideal para quando você já sabe o layout mas os dados ainda não carregaram
public void ShowProductsSkeleton()
{
    // Mostrar skeleton no lugar dos cards
    for (int i = 0; i < 6; i++)
    {
        var skeleton = Instantiate(skeletonCardPrefab, productGrid);
        skeleton.GetComponent<LoadingIndicator>().Show(
            LoadingIndicator.LoadingStyle.Skeleton
        );
    }
    
    // Quando dados chegarem, remover skeletons e mostrar cards reais
}
```

---

## 🃏 ProductCardEnhanced - Cards de Produto

### Exemplo 1: Setup Básico
```csharp
using Barbara.UI;

public class ProductGrid : MonoBehaviour
{
    [SerializeField] private GameObject productCardPrefab;
    [SerializeField] private Transform gridContainer;
    
    public void DisplayProducts(List<ProductData> products)
    {
        // Limpar grid
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }
        
        // Criar cards
        foreach (var product in products)
        {
            var cardObj = Instantiate(productCardPrefab, gridContainer);
            var card = cardObj.GetComponent<ProductCardEnhanced>();
            card.Setup(product);
        }
    }
}
```

### Exemplo 2: Customizar Card
```csharp
public void CreateSpecialCard(ProductData product)
{
    var cardObj = Instantiate(productCardPrefab, gridContainer);
    var card = cardObj.GetComponent<ProductCardEnhanced>();
    
    // Setup normal
    card.Setup(product);
    
    // Customizações
    card.hoverScale = 1.1f;  // Hover mais pronunciado
    card.hoverDuration = 0.3f;  // Transição mais lenta
    
    // Adicionar badge especial
    if (product.isNew)
    {
        var newBadge = Instantiate(newBadgePrefab, card.transform);
        newBadge.GetComponent<RectTransform>().anchoredPosition = new Vector2(10, -10);
    }
}
```

### Exemplo 3: Eventos Customizados
```csharp
public class ProductCardCustomEvents : MonoBehaviour
{
    private ProductCardEnhanced card;
    
    void Start()
    {
        card = GetComponent<ProductCardEnhanced>();
        
        // Adicionar listeners aos botões
        card.quickViewButton.onClick.AddListener(OnQuickView);
        card.favoriteButton.onClick.AddListener(OnFavorite);
        card.tryOnButton.onClick.AddListener(OnTryOn);
    }
    
    private void OnQuickView()
    {
        Debug.Log("Quick view clicked for: " + card.currentProduct.name);
        // Abrir preview 3D
        ProductPreview3D.Show(card.currentProduct);
    }
    
    private void OnFavorite()
    {
        bool isFavorited = !card.isFavorited;
        card.isFavorited = isFavorited;
        
        // Salvar nos favoritos do usuário
        if (isFavorited)
        {
            FavoritesManager.Add(card.currentProduct.id);
            ToastNotification.Success("❤️ Adicionado aos favoritos");
        }
        else
        {
            FavoritesManager.Remove(card.currentProduct.id);
            ToastNotification.Info("Removido dos favoritos");
        }
    }
    
    private async void OnTryOn()
    {
        LoadingIndicator.ShowGlobal(LoadingIndicator.LoadingStyle.Spinner, "Aplicando...");
        
        try
        {
            await TryOnController.ApplyClothing(card.currentProduct);
            ToastNotification.Success("✓ Roupa aplicada no avatar!");
        }
        catch (Exception e)
        {
            ToastNotification.Error("Erro ao aplicar: " + e.Message);
        }
        finally
        {
            LoadingIndicator.HideGlobal();
        }
    }
}
```

---

## 🪟 ModalSystem - Modais

### Exemplo 1: Modal de Produto
```csharp
using Barbara.UI;

public class ProductDetailsButton : MonoBehaviour
{
    [SerializeField] private ProductData product;
    
    public void ShowDetails()
    {
        ModalSystem.Instance.ShowProductModal(product);
    }
}
```

### Exemplo 2: Modal de Confirmação
```csharp
public void DeleteProduct(string productId)
{
    ModalSystem.Instance.ShowConfirmModal(
        title: "Confirmar Exclusão",
        message: "Tem certeza que deseja excluir este produto? Esta ação não pode ser desfeita.",
        onConfirm: async () =>
        {
            LoadingIndicator.ShowGlobal(LoadingIndicator.LoadingStyle.Spinner);
            await APIClient.Delete($"/api/products/{productId}");
            LoadingIndicator.HideGlobal();
            ToastNotification.Success("Produto excluído!");
            RefreshProductList();
        },
        onCancel: () =>
        {
            Debug.Log("Exclusão cancelada");
        }
    );
}
```

### Exemplo 3: Modal de Criação de Avatar
```csharp
public void CreateAvatar()
{
    ModalSystem.Instance.ShowAvatarCreationModal(async (userId, frontUrl, sideUrl) =>
    {
        LoadingIndicator.ShowGlobal(LoadingIndicator.LoadingStyle.ProgressBar, "Criando avatar...");
        
        try
        {
            var avatarData = new AvatarData
            {
                userId = userId,
                frontImageUrl = frontUrl,
                sideImageUrl = sideUrl
            };
            
            // Simular progresso
            for (int i = 0; i <= 100; i += 10)
            {
                LoadingIndicator.UpdateProgress(i / 100f);
                await Task.Delay(200);
            }
            
            var result = await APIClient.Post("/api/avatar/create", avatarData);
            
            LoadingIndicator.HideGlobal();
            ToastNotification.Success("✓ Avatar criado com sucesso!");
            
            // Carregar avatar
            AvatarManager.LoadAvatar(result.avatarId);
        }
        catch (Exception e)
        {
            LoadingIndicator.HideGlobal();
            ToastNotification.Error($"Erro: {e.Message}");
        }
    });
}
```

### Exemplo 4: Modal Customizado
```csharp
public void ShowCustomModal()
{
    // Usar template genérico
    var modalContent = ModalSystem.Instance.ShowModal(customModalTemplate, (content) =>
    {
        // Configurar conteúdo do modal
        var title = content.Find("Title").GetComponent<TextMeshProUGUI>();
        title.text = "Modal Customizado";
        
        var inputField = content.Find("InputField").GetComponent<TMP_InputField>();
        
        var submitButton = content.Find("SubmitButton").GetComponent<Button>();
        submitButton.onClick.AddListener(() =>
        {
            string value = inputField.text;
            Debug.Log("Valor enviado: " + value);
            ModalSystem.Instance.Close();
        });
    });
}
```

---

## 🔍 ProductFilterSystem - Filtros

### Exemplo 1: Setup e Uso Básico
```csharp
using Barbara.UI;

public class CatalogController : MonoBehaviour
{
    [SerializeField] private ProductFilterSystem filterSystem;
    [SerializeField] private Transform productGrid;
    [SerializeField] private GameObject productCardPrefab;
    
    private void Start()
    {
        // Carregar produtos
        LoadProducts();
        
        // Escutar mudanças nos filtros
        filterSystem.ProductGridUpdated += OnProductsFiltered;
    }
    
    private async void LoadProducts()
    {
        LoadingIndicator.ShowGlobal(LoadingIndicator.LoadingStyle.Spinner);
        
        var products = await APIClient.Get<List<ProductData>>("/api/products");
        
        // Configurar filtros com todos os produtos
        filterSystem.SetProducts(products);
        
        LoadingIndicator.HideGlobal();
        ToastNotification.Success($"{products.Count} produtos carregados!");
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
        
        // Feedback visual
        if (products.Count == 0)
        {
            ShowEmptyState();
        }
    }
}
```

### Exemplo 2: Filtros Programáticos
```csharp
public class QuickFilters : MonoBehaviour
{
    [SerializeField] private ProductFilterSystem filterSystem;
    
    public void ShowOnlyDresses()
    {
        filterSystem.categoryDropdown.value = 1; // "Vestidos"
        filterSystem.ApplyFilters();
    }
    
    public void ShowCheapProducts()
    {
        filterSystem.minPriceSlider.value = 0;
        filterSystem.maxPriceSlider.value = 100;
        filterSystem.ApplyFilters();
    }
    
    public void ShowOnly3DProducts()
    {
        filterSystem.has3DToggle.isOn = true;
        filterSystem.ApplyFilters();
    }
    
    public void ShowNewArrivals()
    {
        filterSystem.sortDropdown.value = 4; // SortOrder.Newest
        filterSystem.ApplyFilters();
    }
    
    public void ResetAll()
    {
        filterSystem.ClearAllFilters();
        ToastNotification.Info("Filtros limpos");
    }
}
```

### Exemplo 3: Salvar/Carregar Filtros
```csharp
public class FilterPresets : MonoBehaviour
{
    [SerializeField] private ProductFilterSystem filterSystem;
    
    [System.Serializable]
    public class FilterState
    {
        public string searchText;
        public int categoryIndex;
        public float minPrice;
        public float maxPrice;
        public bool has3D;
        public bool inStock;
        public int sortIndex;
    }
    
    public void SaveCurrentFilters(string presetName)
    {
        var state = new FilterState
        {
            searchText = filterSystem.searchInput.text,
            categoryIndex = filterSystem.categoryDropdown.value,
            minPrice = filterSystem.minPriceSlider.value,
            maxPrice = filterSystem.maxPriceSlider.value,
            has3D = filterSystem.has3DToggle.isOn,
            inStock = filterSystem.inStockToggle.isOn,
            sortIndex = filterSystem.sortDropdown.value
        };
        
        string json = JsonUtility.ToJson(state);
        PlayerPrefs.SetString($"FilterPreset_{presetName}", json);
        PlayerPrefs.Save();
        
        ToastNotification.Success($"Preset '{presetName}' salvo!");
    }
    
    public void LoadFilters(string presetName)
    {
        string json = PlayerPrefs.GetString($"FilterPreset_{presetName}", null);
        
        if (string.IsNullOrEmpty(json))
        {
            ToastNotification.Warning("Preset não encontrado");
            return;
        }
        
        var state = JsonUtility.FromJson<FilterState>(json);
        
        filterSystem.searchInput.text = state.searchText;
        filterSystem.categoryDropdown.value = state.categoryIndex;
        filterSystem.minPriceSlider.value = state.minPrice;
        filterSystem.maxPriceSlider.value = state.maxPrice;
        filterSystem.has3DToggle.isOn = state.has3D;
        filterSystem.inStockToggle.isOn = state.inStock;
        filterSystem.sortDropdown.value = state.sortIndex;
        
        filterSystem.ApplyFilters();
        
        ToastNotification.Info($"Preset '{presetName}' carregado");
    }
}
```

---

## 🎮 UIManagerEnhanced - Gerenciador

### Exemplo 1: Navegação entre Painéis
```csharp
using Barbara.UI;

public class NavigationExample : MonoBehaviour
{
    private UIManagerEnhanced uiManager;
    
    void Start()
    {
        uiManager = FindObjectOfType<UIManagerEnhanced>();
    }
    
    public void GoToAvatar()
    {
        uiManager.ShowPanel(0);
    }
    
    public void GoToCatalog()
    {
        uiManager.ShowPanel(1);
    }
    
    public void GoToShop()
    {
        uiManager.ShowPanel(2);
    }
    
    public void GoToCart()
    {
        uiManager.ShowPanel(3);
    }
}
```

### Exemplo 2: Fluxo de Onboarding
```csharp
public class OnboardingFlow : MonoBehaviour
{
    private UIManagerEnhanced uiManager;
    
    private IEnumerator Start()
    {
        uiManager = FindObjectOfType<UIManagerEnhanced>();
        
        // Bem-vindo
        ToastNotification.Info("Bem-vinda ao Bárbara! ✨");
        yield return new WaitForSeconds(2f);
        
        // Passo 1: Criar avatar
        ToastNotification.Info("Primeiro, vamos criar seu avatar!");
        yield return new WaitForSeconds(1f);
        uiManager.ShowAvatarCreation();
        
        // Esperar criação do avatar
        yield return new WaitUntil(() => AvatarManager.HasAvatar());
        
        // Passo 2: Ir para catálogo
        ToastNotification.Success("Avatar criado! Agora vamos ver as roupas");
        yield return new WaitForSeconds(1f);
        uiManager.ShowPanel(1); // Catalog
        
        // Passo 3: Dica de uso
        yield return new WaitForSeconds(2f);
        ToastNotification.Info("💡 Dica: Passe o mouse nos produtos para ver opções");
    }
}
```

### Exemplo 3: Integração com Sistemas
```csharp
public class UIIntegration : MonoBehaviour
{
    private UIManagerEnhanced uiManager;
    
    void Start()
    {
        uiManager = FindObjectOfType<UIManagerEnhanced>();
        
        // Configurar eventos
        SetupEvents();
    }
    
    private void SetupEvents()
    {
        // Quando produto for experimentado, mudar para painel do avatar
        TryOnController.OnClothingApplied += (product) =>
        {
            ToastNotification.Success($"✓ {product.name} aplicado!");
            uiManager.ShowPanel(0); // Avatar panel para ver resultado
        };
        
        // Quando produto for adicionado ao carrinho, mostrar badge
        CartManager.OnItemAdded += (item) =>
        {
            ToastNotification.Success("Adicionado ao carrinho");
            UpdateCartBadge();
        };
        
        // Quando filtros mudarem, animações
        var filterSystem = FindObjectOfType<ProductFilterSystem>();
        filterSystem.ProductGridUpdated += (products) =>
        {
            // Trigger particle effect na transição
            uiManager.transitionParticles?.Play();
        };
    }
    
    private void UpdateCartBadge()
    {
        int itemCount = CartManager.GetItemCount();
        // Atualizar badge do botão de carrinho
    }
}
```

---

## 🎯 Exemplos Completos de Fluxos

### Fluxo 1: Buscar e Experimentar Roupa
```csharp
public class TryOnFlow : MonoBehaviour
{
    [SerializeField] private UIManagerEnhanced uiManager;
    [SerializeField] private ProductFilterSystem filterSystem;
    
    public async Task SearchAndTryOn(string searchTerm)
    {
        // 1. Ir para catálogo
        uiManager.ShowPanel(1);
        
        // 2. Buscar produto
        filterSystem.searchInput.text = searchTerm;
        filterSystem.ApplyFilters();
        
        await Task.Delay(500); // Esperar debounce
        
        // 3. Verificar resultados
        var results = filterSystem.GetFilteredProducts();
        
        if (results.Count == 0)
        {
            ToastNotification.Warning($"Nenhum produto encontrado para '{searchTerm}'");
            return;
        }
        
        // 4. Pegar primeiro resultado
        var product = results[0];
        ToastNotification.Info($"Encontrado: {product.name}");
        
        // 5. Experimentar
        LoadingIndicator.ShowGlobal(LoadingIndicator.LoadingStyle.Spinner, "Aplicando...");
        
        try
        {
            await TryOnController.ApplyClothing(product);
            LoadingIndicator.HideGlobal();
            
            // 6. Mostrar resultado no avatar
            uiManager.ShowPanel(0);
            ToastNotification.Success("✓ Roupa aplicada! Como ficou?");
        }
        catch (Exception e)
        {
            LoadingIndicator.HideGlobal();
            ToastNotification.Error($"Erro: {e.Message}");
        }
    }
}
```

### Fluxo 2: Compra Completa
```csharp
public class PurchaseFlow : MonoBehaviour
{
    public async Task CompletePurchase(ProductData product)
    {
        // 1. Confirmar intenção
        bool confirmed = false;
        
        ModalSystem.Instance.ShowConfirmModal(
            "Confirmar Compra",
            $"Deseja comprar {product.name} por R$ {product.price:F2}?",
            onConfirm: () => confirmed = true,
            onCancel: () => confirmed = false
        );
        
        // Esperar resposta
        await Task.Run(() => { while (!confirmed) Task.Delay(100); });
        
        if (!confirmed) return;
        
        // 2. Processar pagamento
        LoadingIndicator.ShowGlobal(
            LoadingIndicator.LoadingStyle.ProgressBar,
            "Processando pagamento..."
        );
        
        try
        {
            // Simular etapas de pagamento
            LoadingIndicator.UpdateProgress(0.2f);
            LoadingIndicator.UpdateMessage("Validando dados...");
            await Task.Delay(1000);
            
            LoadingIndicator.UpdateProgress(0.5f);
            LoadingIndicator.UpdateMessage("Processando...");
            await Task.Delay(1000);
            
            LoadingIndicator.UpdateProgress(0.8f);
            LoadingIndicator.UpdateMessage("Confirmando...");
            await Task.Delay(1000);
            
            var result = await APIClient.Post("/api/purchase", new { productId = product.id });
            
            LoadingIndicator.UpdateProgress(1f);
            await Task.Delay(500);
            
            LoadingIndicator.HideGlobal();
            
            // 3. Sucesso!
            ToastNotification.Success("🎉 Compra realizada com sucesso!");
            
            // 4. Mostrar confirmação
            ModalSystem.Instance.ShowConfirmModal(
                "Compra Confirmada",
                $"Parabéns! Você comprou {product.name}. Um email de confirmação foi enviado.",
                onConfirm: () =>
                {
                    // Ir para histórico de pedidos
                    uiManager.ShowPanel(2); // Shop panel
                },
                confirmText: "Ver Pedidos",
                cancelText: "Continuar Comprando"
            );
        }
        catch (Exception e)
        {
            LoadingIndicator.HideGlobal();
            ToastNotification.Error($"Erro na compra: {e.Message}");
        }
    }
}
```

### Fluxo 3: Criar Avatar Completo
```csharp
public class AvatarCreationFlow : MonoBehaviour
{
    public void StartAvatarCreation()
    {
        ModalSystem.Instance.ShowAvatarCreationModal(async (userId, frontUrl, sideUrl) =>
        {
            // Validar inputs
            if (string.IsNullOrEmpty(userId))
            {
                ToastNotification.Error("Por favor, insira seu ID");
                return;
            }
            
            if (string.IsNullOrEmpty(frontUrl) || string.IsNullOrEmpty(sideUrl))
            {
                ToastNotification.Error("Por favor, selecione as duas fotos");
                return;
            }
            
            // Fechar modal
            ModalSystem.Instance.Close();
            
            // Mostrar progresso
            LoadingIndicator.ShowGlobal(
                LoadingIndicator.LoadingStyle.ProgressBar,
                "Criando seu avatar..."
            );
            
            try
            {
                // Etapa 1: Upload das fotos
                LoadingIndicator.UpdateMessage("Enviando fotos...");
                LoadingIndicator.UpdateProgress(0.1f);
                
                var frontImageData = await UploadImage(frontUrl);
                LoadingIndicator.UpdateProgress(0.3f);
                
                var sideImageData = await UploadImage(sideUrl);
                LoadingIndicator.UpdateProgress(0.5f);
                
                // Etapa 2: Processar modelo 3D
                LoadingIndicator.UpdateMessage("Gerando modelo 3D...");
                
                var avatarData = new AvatarData
                {
                    userId = userId,
                    frontImageUrl = frontImageData.url,
                    sideImageUrl = sideImageData.url
                };
                
                var result = await APIClient.Post("/api/avatar/create", avatarData);
                LoadingIndicator.UpdateProgress(0.8f);
                
                // Etapa 3: Carregar avatar
                LoadingIndicator.UpdateMessage("Carregando avatar...");
                
                await AvatarManager.LoadAvatar(result.avatarId);
                LoadingIndicator.UpdateProgress(1f);
                
                await Task.Delay(500);
                LoadingIndicator.HideGlobal();
                
                // Sucesso!
                ToastNotification.Success("🎉 Avatar criado com sucesso!");
                
                // Ir para painel do avatar
                uiManager.ShowPanel(0);
                
                // Sequência de dicas
                await Task.Delay(2000);
                ToastNotification.Info("💡 Agora você pode experimentar roupas!");
                
                await Task.Delay(3000);
                ToastNotification.Info("👉 Vá para o Catálogo para começar");
            }
            catch (Exception e)
            {
                LoadingIndicator.HideGlobal();
                ToastNotification.Error($"Erro ao criar avatar: {e.Message}");
                
                // Reabrir modal para tentar novamente
                await Task.Delay(2000);
                StartAvatarCreation();
            }
        });
    }
    
    private async Task<ImageUploadResult> UploadImage(string localPath)
    {
        // Implementar upload real
        await Task.Delay(1000); // Simulação
        return new ImageUploadResult { url = "https://..." };
    }
}
```

---

## 🎨 Customizações Visuais

### Exemplo: Tema Escuro/Claro
```csharp
public class ThemeManager : MonoBehaviour
{
    [System.Serializable]
    public class Theme
    {
        public Color backgroundColor;
        public Color primaryColor;
        public Color textColor;
        public Color cardColor;
    }
    
    public Theme lightTheme;
    public Theme darkTheme;
    
    private Theme currentTheme;
    
    public void ToggleTheme()
    {
        currentTheme = (currentTheme == lightTheme) ? darkTheme : lightTheme;
        ApplyTheme();
    }
    
    private void ApplyTheme()
    {
        // Aplicar cores em todos os componentes
        Camera.main.backgroundColor = currentTheme.backgroundColor;
        
        // Atualizar cards
        var cards = FindObjectsOfType<ProductCardEnhanced>();
        foreach (var card in cards)
        {
            card.cardBackground.color = currentTheme.cardColor;
        }
        
        // Toasts
        ToastNotification.Info($"Tema alterado para {(currentTheme == lightTheme ? "Claro" : "Escuro")}");
    }
}
```

---

## 📱 Responsividade

### Exemplo: Ajustar Layout por Resolução
```csharp
public class ResponsiveLayout : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup productGrid;
    
    void Start()
    {
        AdjustLayout();
    }
    
    void Update()
    {
        if (Screen.width != lastScreenWidth)
        {
            AdjustLayout();
            lastScreenWidth = Screen.width;
        }
    }
    
    private int lastScreenWidth;
    
    private void AdjustLayout()
    {
        int columns = Screen.width switch
        {
            <= 768 => 1,     // Mobile
            <= 1024 => 2,    // Tablet
            <= 1920 => 3,    // Desktop
            _ => 4           // Ultra-wide
        };
        
        productGrid.constraintCount = columns;
        
        float spacing = Screen.width switch
        {
            <= 768 => 10,
            _ => 20
        };
        
        productGrid.spacing = new Vector2(spacing, spacing);
    }
}
```

---

*Exemplos criados em: 6 de novembro de 2025*
*Versão: 1.0.0*

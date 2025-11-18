using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Barbara.UI
{
    /// <summary>
    /// UIManager Premium - Gerencia toda a experiência do usuário
    /// </summary>
    public class UIManagerEnhanced : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject avatarPanel;
        [SerializeField] private GameObject catalogPanel;
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private GameObject cartPanel;

        [Header("Navigation")]
        [SerializeField] private Button avatarButton;
        [SerializeField] private Button catalogButton;
        [SerializeField] private Button shopButton;
        [SerializeField] private Button cartButton;
        [SerializeField] private Image[] navigationIndicators;

        [Header("Top Bar")]
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button notificationsButton;
        [SerializeField] private Button profileButton;
        [SerializeField] private GameObject notificationBadge;

        [Header("Systems")]
        [SerializeField] private ModalSystem modalSystem;
        [SerializeField] private ProductFilterSystem filterSystem;
        [SerializeField] private LoadingIndicator loadingIndicator;

        [Header("References")]
        [SerializeField] private Barbara.Core.AvatarManager avatarManager;
        [SerializeField] private Barbara.Core.CatalogLoader catalogLoader;
        [SerializeField] private Barbara.Core.TryOnController tryOnController;

        [Header("Visual Effects")]
        [SerializeField] private ParticleSystem transitionParticles;
        [SerializeField] private GameObject backgroundGradient;

        private int currentPanelIndex = 1; // Começar no catálogo
        private GameObject[] panels;

        private void Start()
        {
            SetupPanels();
            SetupButtons();
            SetupSystems();
            
            // Mostrar catálogo inicialmente
            ShowPanel(currentPanelIndex, false);

            // Feedback de boas-vindas
            StartCoroutine(ShowWelcomeSequence());
        }

        private void SetupPanels()
        {
            panels = new GameObject[] 
            { 
                avatarPanel, 
                catalogPanel, 
                shopPanel, 
                cartPanel 
            };
        }

        private void SetupButtons()
        {
            if (avatarButton != null)
                avatarButton.onClick.AddListener(() => ShowPanel(0));

            if (catalogButton != null)
                catalogButton.onClick.AddListener(() => ShowPanel(1));

            if (shopButton != null)
                shopButton.onClick.AddListener(() => ShowPanel(2));

            if (cartButton != null)
                cartButton.onClick.AddListener(() => ShowPanel(3));

            if (settingsButton != null)
                settingsButton.onClick.AddListener(ShowSettings);

            if (notificationsButton != null)
                notificationsButton.onClick.AddListener(ShowNotifications);

            if (profileButton != null)
                profileButton.onClick.AddListener(ShowProfile);
        }

        private void SetupSystems()
        {
            // Integrar sistema de filtros com catálogo
            if (filterSystem != null)
            {
                filterSystem.ProductGridUpdated += OnProductsFiltered;
            }

            // Escutar eventos do catálogo
            if (catalogLoader != null)
            {
                // TODO: Subscribe to catalog events
            }
        }

        public void ShowPanel(int index, bool animate = true)
        {
            if (index < 0 || index >= panels.Length) return;

            // Salvar índice anterior
            int previousIndex = currentPanelIndex;
            currentPanelIndex = index;

            // Ocultar todos os painéis
            foreach (var panel in panels)
            {
                if (panel == null) continue;

                if (animate)
                {
                    var animator = panel.GetComponent<UIAnimator>();
                    if (animator != null)
                        animator.PlayReverse();
                }
                else
                {
                    panel.SetActive(false);
                }
            }

            // Aguardar animação antes de mostrar novo painel
            if (animate)
            {
                StartCoroutine(ShowPanelAfterDelay(index, 0.3f));
            }
            else
            {
                ShowPanelImmediate(index);
            }

            // Atualizar indicadores de navegação
            UpdateNavigationIndicators(index);

            // Efeitos visuais
            if (transitionParticles != null)
                transitionParticles.Play();

            // Trigger eventos específicos do painel
            OnPanelChanged(index, previousIndex);

            // Feedback sonoro
            // AudioManager.PlaySound("ui_panel_switch");
        }

        private IEnumerator ShowPanelAfterDelay(int index, float delay)
        {
            yield return new WaitForSeconds(delay);
            ShowPanelImmediate(index);
        }

        private void ShowPanelImmediate(int index)
        {
            if (panels[index] == null) return;

            panels[index].SetActive(true);

            var animator = panels[index].GetComponent<UIAnimator>();
            if (animator != null)
                animator.Play();

            Debug.Log($"Panel {index} ({GetPanelName(index)}) ativo");
        }

        private void UpdateNavigationIndicators(int activeIndex)
        {
            if (navigationIndicators == null || navigationIndicators.Length == 0)
                return;

            for (int i = 0; i < navigationIndicators.Length; i++)
            {
                if (navigationIndicators[i] == null) continue;

                bool isActive = i == activeIndex;
                Color targetColor = isActive 
                    ? new Color(0.76f, 0.6f, 1f) // Lilás
                    : new Color(0.5f, 0.5f, 0.5f); // Cinza

                navigationIndicators[i].color = targetColor;

                // Animação de scale
                StartCoroutine(AnimateIndicator(navigationIndicators[i].transform, isActive));
            }
        }

        private IEnumerator AnimateIndicator(Transform indicator, bool active)
        {
            Vector3 targetScale = active ? Vector3.one * 1.2f : Vector3.one;
            float duration = 0.2f;
            float elapsed = 0f;

            Vector3 startScale = indicator.localScale;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                indicator.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
                yield return null;
            }

            indicator.localScale = targetScale;
        }

        private void OnPanelChanged(int newIndex, int previousIndex)
        {
            string panelName = GetPanelName(newIndex);

            switch (newIndex)
            {
                case 0: // Avatar
                    OnAvatarPanelOpened();
                    break;

                case 1: // Catalog
                    OnCatalogPanelOpened();
                    break;

                case 2: // Shop
                    OnShopPanelOpened();
                    break;

                case 3: // Cart
                    OnCartPanelOpened();
                    break;
            }
        }

        private void OnAvatarPanelOpened()
        {
            Debug.Log("Avatar panel opened - Showing avatar customization");
            
            // Focar na câmera do avatar
            if (avatarManager != null)
            {
                // TODO: Focus camera on avatar
            }
        }

        private void OnCatalogPanelOpened()
        {
            Debug.Log("Catalog panel opened - Loading products");

            // Carregar catálogo se ainda não foi carregado
            if (catalogLoader != null)
            {
                catalogLoader.LoadCatalog();
            }
        }

        private void OnShopPanelOpened()
        {
            Debug.Log("Shop panel opened - Ready for purchases");
        }

        private void OnCartPanelOpened()
        {
            Debug.Log("Cart panel opened - Showing cart items");
        }

        private string GetPanelName(int index)
        {
            switch (index)
            {
                case 0: return "Avatar";
                case 1: return "Catalog";
                case 2: return "Shop";
                case 3: return "Cart";
                default: return "Unknown";
            }
        }

        private void OnProductsFiltered(System.Collections.Generic.List<ProductData> products)
        {
            Debug.Log($"Products filtered: {products.Count} results");
            // TODO: Atualizar grid de produtos com produtos filtrados
        }

        public void ShowAvatarCreation()
        {
            if (modalSystem != null)
            {
                modalSystem.ShowAvatarCreationModal((userId, frontUrl, sideUrl) =>
                {
                    if (avatarManager != null)
                    {
                        LoadingIndicator.ShowGlobal(
                            LoadingIndicator.LoadingStyle.ProgressBar, 
                            "Gerando seu avatar personalizado..."
                        );

                        avatarManager.RequestCustomAvatar(userId, frontUrl, sideUrl);
                        
                        // Simular progresso
                        StartCoroutine(SimulateAvatarProgress());
                    }
                });
            }
        }

        private IEnumerator SimulateAvatarProgress()
        {
            float progress = 0f;
            while (progress < 1f)
            {
                progress += Time.deltaTime * 0.2f;
                LoadingIndicator.UpdateProgress(progress);
                yield return null;
            }

            yield return new WaitForSeconds(1f);
            LoadingIndicator.HideGlobal();
            ToastNotification.Success("Avatar criado com sucesso!");
            ShowPanel(0); // Mostrar painel do avatar
        }

        private void ShowSettings()
        {
            ToastNotification.Info("Configurações em breve!");
            // TODO: Implementar modal de settings
        }

        private void ShowNotifications()
        {
            if (notificationBadge != null)
                notificationBadge.SetActive(false);

            ToastNotification.Info("Nenhuma notificação nova");
            // TODO: Implementar sistema de notificações
        }

        private void ShowProfile()
        {
            ToastNotification.Info("Perfil do usuário em desenvolvimento");
            // TODO: Implementar modal de perfil
        }

        private IEnumerator ShowWelcomeSequence()
        {
            yield return new WaitForSeconds(0.5f);
            
            ToastNotification.Show(
                "Bem-vinda ao Bárbara! ✨",
                ToastNotification.ToastType.Success,
                3f
            );

            yield return new WaitForSeconds(3.5f);

            ToastNotification.Info(
                "Explore o catálogo e experimente roupas no seu avatar!",
                4f
            );
        }

        // Atalhos de teclado
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                ShowPanel(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                ShowPanel(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                ShowPanel(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                ShowPanel(3);
        }
    }
}

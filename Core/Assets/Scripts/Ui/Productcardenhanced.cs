using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

namespace Barbara.UI
{
    /// <summary>
    /// Card de produto premium com micro-interações e preview 3D
    /// </summary>
    public class ProductCardEnhanced : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [Header("UI References")]
        [SerializeField] private Image productImage;
        [SerializeField] private TextMeshProUGUI productName;
        [SerializeField] private TextMeshProUGUI productPrice;
        [SerializeField] private TextMeshProUGUI productCategory;
        [SerializeField] private GameObject badge3D;
        [SerializeField] private GameObject quickViewButton;
        [SerializeField] private GameObject favoriteButton;
        [SerializeField] private Image favoriteIcon;

        [Header("Hover Effects")]
        [SerializeField] private float hoverScale = 1.05f;
        [SerializeField] private float hoverDuration = 0.2f;
        [SerializeField] private GameObject glowEffect;
        [SerializeField] private ParticleSystem shimmerParticles;

        [Header("Colors")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color hoverColor = new Color(0.76f, 0.6f, 1f); // Lilás
        [SerializeField] private Color favoriteColor = new Color(1f, 0.3f, 0.5f);

        private ProductData productData;
        private bool isFavorite = false;
        private Vector3 originalScale;
        private Coroutine hoverCoroutine;
        private Image cardBackground;

        private void Awake()
        {
            originalScale = transform.localScale;
            cardBackground = GetComponent<Image>();

            if (quickViewButton != null)
                quickViewButton.SetActive(false);

            if (glowEffect != null)
                glowEffect.SetActive(false);
        }

        public void Setup(ProductData product)
        {
            productData = product;

            if (productName != null)
                productName.text = product.name;

            if (productPrice != null)
                productPrice.text = $"R$ {product.price:F2}";

            if (productCategory != null)
            {
                productCategory.text = product.category.ToUpper();
                productCategory.color = GetCategoryColor(product.category);
            }

            if (badge3D != null)
                badge3D.SetActive(!string.IsNullOrEmpty(product.model3dUrl));

            // Carregar imagem
            if (!string.IsNullOrEmpty(product.images[0]) && productImage != null)
            {
                StartCoroutine(LoadProductImage(product.images[0]));
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (hoverCoroutine != null)
                StopCoroutine(hoverCoroutine);
            
            hoverCoroutine = StartCoroutine(AnimateHover(true));

            if (quickViewButton != null)
            {
                quickViewButton.SetActive(true);
                var animator = quickViewButton.GetComponent<UIAnimator>();
                if (animator != null)
                    animator.Play();
            }

            if (glowEffect != null)
                glowEffect.SetActive(true);

            if (shimmerParticles != null && !shimmerParticles.isPlaying)
                shimmerParticles.Play();

            // Feedback sonoro sutil (se tiver AudioManager)
            // AudioManager.PlaySound("ui_hover");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (hoverCoroutine != null)
                StopCoroutine(hoverCoroutine);
            
            hoverCoroutine = StartCoroutine(AnimateHover(false));

            if (quickViewButton != null)
                quickViewButton.SetActive(false);

            if (glowEffect != null)
                glowEffect.SetActive(false);

            if (shimmerParticles != null)
                shimmerParticles.Stop();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // Animação de click
            StartCoroutine(AnimateClick());

            // Abrir detalhes do produto
            if (ModalSystem.Instance != null)
            {
                ModalSystem.Instance.ShowProductModal(productData);
            }

            ToastNotification.Success($"Visualizando {productData.name}");
        }

        public void OnQuickViewClick()
        {
            // Preview 3D rápido sem abrir modal completo
            if (ProductPreview3D.Instance != null)
            {
                ProductPreview3D.Instance.ShowPreview(productData);
            }
        }

        public void OnFavoriteClick()
        {
            isFavorite = !isFavorite;

            if (favoriteIcon != null)
            {
                favoriteIcon.color = isFavorite ? favoriteColor : normalColor;
                
                // Animação de bounce
                StartCoroutine(AnimateFavoriteIcon());
            }

            if (isFavorite)
            {
                ToastNotification.Success($"{productData.name} adicionado aos favoritos!");
                // Salvar no PlayerPrefs ou backend
            }
            else
            {
                ToastNotification.Info($"{productData.name} removido dos favoritos");
            }
        }

        public void OnTryOnClick()
        {
            LoadingIndicator.ShowGlobal(LoadingIndicator.LoadingStyle.Spinner, "Aplicando roupa...");

            // Aplicar roupa no avatar
            var tryOnController = FindObjectOfType<Barbara.Core.TryOnController>();
            if (tryOnController != null)
            {
                tryOnController.ApplyClothing(productData);
                StartCoroutine(HideLoadingAfterDelay(1f));
            }
        }

        private IEnumerator AnimateHover(bool hovering)
        {
            Vector3 targetScale = hovering ? originalScale * hoverScale : originalScale;
            Color targetColor = hovering ? hoverColor : normalColor;

            float elapsed = 0f;

            while (elapsed < hoverDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / hoverDuration;

                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, t);

                if (cardBackground != null)
                    cardBackground.color = Color.Lerp(cardBackground.color, targetColor, t);

                yield return null;
            }

            transform.localScale = targetScale;
            if (cardBackground != null)
                cardBackground.color = targetColor;
        }

        private IEnumerator AnimateClick()
        {
            Vector3 pressScale = originalScale * 0.95f;
            
            // Press down
            float elapsed = 0f;
            while (elapsed < 0.1f)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(originalScale, pressScale, elapsed / 0.1f);
                yield return null;
            }

            // Release
            elapsed = 0f;
            while (elapsed < 0.1f)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(pressScale, originalScale, elapsed / 0.1f);
                yield return null;
            }

            transform.localScale = originalScale;
        }

        private IEnumerator AnimateFavoriteIcon()
        {
            Vector3 originalIconScale = favoriteIcon.transform.localScale;
            Vector3 bounceScale = originalIconScale * 1.3f;

            // Bounce up
            float elapsed = 0f;
            while (elapsed < 0.15f)
            {
                elapsed += Time.deltaTime;
                favoriteIcon.transform.localScale = Vector3.Lerp(originalIconScale, bounceScale, elapsed / 0.15f);
                yield return null;
            }

            // Bounce down
            elapsed = 0f;
            while (elapsed < 0.15f)
            {
                elapsed += Time.deltaTime;
                favoriteIcon.transform.localScale = Vector3.Lerp(bounceScale, originalIconScale, elapsed / 0.15f);
                yield return null;
            }

            favoriteIcon.transform.localScale = originalIconScale;
        }

        private IEnumerator LoadProductImage(string url)
        {
            // TODO: Implementar carregamento real via UnityWebRequest
            // Por enquanto, placeholder
            yield return null;
            Debug.Log($"Loading product image: {url}");
        }

        private IEnumerator HideLoadingAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            LoadingIndicator.HideGlobal();
            ToastNotification.Success("Roupa aplicada com sucesso!");
        }

        private Color GetCategoryColor(string category)
        {
            switch (category.ToLower())
            {
                case "vestidos": return new Color(1f, 0.4f, 0.7f);
                case "camisetas": return new Color(0.4f, 0.7f, 1f);
                case "calças": return new Color(0.5f, 0.5f, 1f);
                case "acessórios": return new Color(1f, 0.8f, 0.4f);
                default: return Color.white;
            }
        }

        public ProductData GetProductData() => productData;
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Barbara.Core
{
    /// <summary>
    /// Componente de UI para card de produto no catálogo.
    /// </summary>
    public class ProductCard : MonoBehaviour
    {
        [SerializeField] private Text productNameText;
        [SerializeField] private Text priceText;
        [SerializeField] private Text categoryText;
        [SerializeField] private Button tryOnButton;

        private ProductData productData;

        public void Setup(ProductData product)
        {
            productData = product;

            if (productNameText != null)
                productNameText.text = product.name;

            if (priceText != null)
                priceText.text = $"R$ {product.price:F2}";

            if (categoryText != null)
                categoryText.text = product.category;

            if (tryOnButton != null)
            {
                tryOnButton.onClick.RemoveAllListeners();
                tryOnButton.onClick.AddListener(OnTryOnClicked);
            }
        }

        private void OnTryOnClicked()
        {
            Debug.Log($"Experimentando produto: {productData.name}");
            var tryOnController = FindObjectOfType<TryOnController>();
            if (tryOnController != null)
            {
                tryOnController.ApplyClothing(productData);
            }
        }
    }
}

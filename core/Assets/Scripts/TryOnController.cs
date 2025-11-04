using UnityEngine;

namespace Barbara.Core
{
    /// <summary>
    /// Aplica roupas do catálogo no avatar atual.
    /// </summary>
    public class TryOnController : MonoBehaviour
    {
        [SerializeField] private AvatarManager avatarManager;
        [SerializeField] private SkinnedMeshRenderer avatarBodyRenderer;

        private ProductData currentProduct;

        public void ApplyClothing(ProductData product)
        {
            currentProduct = product;
            Debug.Log($"Aplicando roupa: {product.name}");

            // Estratégia 1: Trocar material/textura
            if (!string.IsNullOrEmpty(product.model3dUrl))
            {
                LoadClothingModel(product.model3dUrl);
            }
            else
            {
                // Fallback: trocar apenas textura/cor
                ApplySimpleTexture(product);
            }
        }

        /// <summary>
        /// Carrega modelo 3D da roupa e aplica ao avatar.
        /// </summary>
        private void LoadClothingModel(string model3dUrl)
        {
            Debug.Log($"Carregando modelo 3D de roupa: {model3dUrl}");
            // TODO: Implementar carregamento de .glb com GLTFUtility
            // Exemplo: Importer.LoadFromUri(model3dUrl, settings, OnClothingLoaded);
        }

        /// <summary>
        /// Aplica textura ou cor simples (fallback).
        /// </summary>
        private void ApplySimpleTexture(ProductData product)
        {
            if (avatarBodyRenderer != null && !string.IsNullOrEmpty(product.color))
            {
                Color color;
                if (ColorUtility.TryParseHtmlString(product.color, out color))
                {
                    avatarBodyRenderer.material.color = color;
                    Debug.Log($"Cor aplicada: {product.color}");
                }
            }
        }

        /// <summary>
        /// Callback quando modelo de roupa for carregado.
        /// </summary>
        private void OnClothingLoaded(GameObject clothingObject)
        {
            // Anexar ao avatar
            if (clothingObject != null && avatarManager != null)
            {
                clothingObject.transform.SetParent(avatarManager.transform);
                clothingObject.transform.localPosition = Vector3.zero;
                Debug.Log("Roupa aplicada ao avatar!");
            }
        }

        public ProductData GetCurrentProduct()
        {
            return currentProduct;
        }
    }
}

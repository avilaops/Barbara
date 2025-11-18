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
        private GameObject currentClothingInstance;

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
            GlbLoader.Instance.Load(
                model3dUrl,
                onSuccess: OnClothingLoaded,
                onError: (error) =>
                {
                    Debug.LogError($"Falha ao carregar modelo 3D: {error}");
                    ApplySimpleTexture(currentProduct);
                });
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
            if (clothingObject == null || avatarManager == null)
            {
                Debug.LogWarning("Modelo de roupa inválido ou AvatarManager ausente.");
                return;
            }

            if (currentClothingInstance != null)
            {
                Destroy(currentClothingInstance);
            }

            clothingObject.transform.SetParent(avatarManager.AvatarRoot, false);
            clothingObject.transform.localPosition = Vector3.zero;
            clothingObject.transform.localRotation = Quaternion.identity;
            clothingObject.transform.localScale = Vector3.one;

            currentClothingInstance = clothingObject;
            Debug.Log("Roupa aplicada ao avatar!");
        }

        public ProductData GetCurrentProduct()
        {
            return currentProduct;
        }
    }
}

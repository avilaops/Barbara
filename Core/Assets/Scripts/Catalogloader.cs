using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Barbara.Core
{
    /// <summary>
    /// Gerencia carregamento e exibição do catálogo de produtos.
    /// </summary>
    public class CatalogLoader : MonoBehaviour
    {
        [SerializeField] private Transform catalogContainer;
        [SerializeField] private GameObject productCardPrefab;
        [SerializeField] private Text statusText;

        private List<ProductData> products = new List<ProductData>();

        private void Start()
        {
            LoadCatalog();
        }

        public void LoadCatalog()
        {
            if (statusText != null)
            {
                statusText.text = "Carregando catálogo...";
            }

            StartCoroutine(LoadCatalogCoroutine());
        }

        private IEnumerator LoadCatalogCoroutine()
        {
            yield return APIClient.Instance.GetCatalog(
                onSuccess: (response) =>
                {
                    products.Clear();
                    products.AddRange(response.items);
                    DisplayProducts();

                    if (statusText != null)
                    {
                        statusText.text = $"{products.Count} produtos carregados.";
                    }

                    Debug.Log($"Catálogo carregado: {products.Count} produtos.");
                },
                onError: (error) =>
                {
                    Debug.LogError($"Erro ao carregar catálogo: {error}");
                    if (statusText != null)
                    {
                        statusText.text = "Erro ao carregar catálogo.";
                    }
                }
            );
        }

        private void DisplayProducts()
        {
            // Limpar cards anteriores
            foreach (Transform child in catalogContainer)
            {
                Destroy(child.gameObject);
            }

            // Criar cards de produtos
            foreach (var product in products)
            {
                if (productCardPrefab != null)
                {
                    GameObject card = Instantiate(productCardPrefab, catalogContainer);
                    var cardUI = card.GetComponent<ProductCard>();
                    if (cardUI != null)
                    {
                        cardUI.Setup(product);
                    }
                }
            }
        }

        public ProductData GetProductById(string productId)
        {
            return products.Find(p => p.id == productId);
        }
    }
}

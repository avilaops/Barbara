using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Barbara.UI
{
    /// <summary>
    /// Sistema de filtros e busca para catálogo de produtos
    /// </summary>
    public class ProductFilterSystem : MonoBehaviour
    {
        [Header("Search")]
        [SerializeField] private TMP_InputField searchInput;
        [SerializeField] private Button searchButton;
        [SerializeField] private float searchDebounceTime = 0.5f;

        [Header("Filters")]
        [SerializeField] private TMP_Dropdown categoryDropdown;
        [SerializeField] private Slider priceMinSlider;
        [SerializeField] private Slider priceMaxSlider;
        [SerializeField] private TextMeshProUGUI priceMinText;
        [SerializeField] private TextMeshProUGUI priceMaxText;
        [SerializeField] private Toggle has3DToggle;
        [SerializeField] private Toggle inStockToggle;

        [Header("Sort")]
        [SerializeField] private TMP_Dropdown sortDropdown;

        [Header("Results")]
        [SerializeField] private TextMeshProUGUI resultsCountText;
        [SerializeField] private Button clearFiltersButton;

        [Header("References")]
        [SerializeField] private Barbara.Core.CatalogLoader catalogLoader;

        private List<ProductData> allProducts = new List<ProductData>();
        private List<ProductData> filteredProducts = new List<ProductData>();
        private Coroutine searchCoroutine;

        public enum SortOrder
        {
            NameAsc,
            NameDesc,
            PriceAsc,
            PriceDesc,
            Newest
        }

        private void Start()
        {
            SetupListeners();
            SetupDropdowns();
        }

        private void SetupListeners()
        {
            if (searchInput != null)
                searchInput.onValueChanged.AddListener(OnSearchTextChanged);

            if (searchButton != null)
                searchButton.onClick.AddListener(ApplyFilters);

            if (categoryDropdown != null)
                categoryDropdown.onValueChanged.AddListener((val) => ApplyFilters());

            if (priceMinSlider != null)
            {
                priceMinSlider.onValueChanged.AddListener((val) =>
                {
                    UpdatePriceText();
                    ApplyFilters();
                });
            }

            if (priceMaxSlider != null)
            {
                priceMaxSlider.onValueChanged.AddListener((val) =>
                {
                    UpdatePriceText();
                    ApplyFilters();
                });
            }

            if (has3DToggle != null)
                has3DToggle.onValueChanged.AddListener((val) => ApplyFilters());

            if (inStockToggle != null)
                inStockToggle.onValueChanged.AddListener((val) => ApplyFilters());

            if (sortDropdown != null)
                sortDropdown.onValueChanged.AddListener((val) => ApplyFilters());

            if (clearFiltersButton != null)
                clearFiltersButton.onClick.AddListener(ClearAllFilters);
        }

        private void SetupDropdowns()
        {
            // Setup category dropdown
            if (categoryDropdown != null)
            {
                categoryDropdown.ClearOptions();
                var categories = new List<string> { "Todas", "Vestidos", "Camisetas", "Calças", "Acessórios", "Sapatos" };
                categoryDropdown.AddOptions(categories);
            }

            // Setup sort dropdown
            if (sortDropdown != null)
            {
                sortDropdown.ClearOptions();
                var sortOptions = new List<string> 
                { 
                    "Nome (A-Z)", 
                    "Nome (Z-A)", 
                    "Preço (Menor)", 
                    "Preço (Maior)", 
                    "Mais Recentes" 
                };
                sortDropdown.AddOptions(sortOptions);
            }
        }

        public void SetProducts(List<ProductData> products)
        {
            allProducts = new List<ProductData>(products);
            filteredProducts = new List<ProductData>(products);
            ApplyFilters();
        }

        private void OnSearchTextChanged(string searchText)
        {
            if (searchCoroutine != null)
                StopCoroutine(searchCoroutine);

            searchCoroutine = StartCoroutine(SearchWithDebounce(searchText));
        }

        private IEnumerator SearchWithDebounce(string searchText)
        {
            yield return new WaitForSeconds(searchDebounceTime);
            ApplyFilters();
        }

        public void ApplyFilters()
        {
            filteredProducts = new List<ProductData>(allProducts);

            // Filtro de busca
            if (searchInput != null && !string.IsNullOrEmpty(searchInput.text))
            {
                string searchLower = searchInput.text.ToLower();
                filteredProducts = filteredProducts.Where(p => 
                    p.name.ToLower().Contains(searchLower) ||
                    p.description.ToLower().Contains(searchLower) ||
                    p.category.ToLower().Contains(searchLower)
                ).ToList();
            }

            // Filtro de categoria
            if (categoryDropdown != null && categoryDropdown.value > 0)
            {
                string selectedCategory = categoryDropdown.options[categoryDropdown.value].text.ToLower();
                filteredProducts = filteredProducts.Where(p => 
                    p.category.ToLower() == selectedCategory
                ).ToList();
            }

            // Filtro de preço
            if (priceMinSlider != null && priceMaxSlider != null)
            {
                float minPrice = priceMinSlider.value;
                float maxPrice = priceMaxSlider.value;
                filteredProducts = filteredProducts.Where(p => 
                    p.price >= minPrice && p.price <= maxPrice
                ).ToList();
            }

            // Filtro de modelo 3D
            if (has3DToggle != null && has3DToggle.isOn)
            {
                filteredProducts = filteredProducts.Where(p => 
                    !string.IsNullOrEmpty(p.model3dUrl)
                ).ToList();
            }

            // Filtro de estoque
            if (inStockToggle != null && inStockToggle.isOn)
            {
                filteredProducts = filteredProducts.Where(p => 
                    p.stock > 0
                ).ToList();
            }

            // Aplicar ordenação
            ApplySorting();

            // Atualizar UI
            UpdateResultsCount();
            DisplayFilteredProducts();

            // Feedback visual
            AnimateFilterUpdate();
        }

        private void ApplySorting()
        {
            if (sortDropdown == null) return;

            SortOrder sortOrder = (SortOrder)sortDropdown.value;

            switch (sortOrder)
            {
                case SortOrder.NameAsc:
                    filteredProducts = filteredProducts.OrderBy(p => p.name).ToList();
                    break;

                case SortOrder.NameDesc:
                    filteredProducts = filteredProducts.OrderByDescending(p => p.name).ToList();
                    break;

                case SortOrder.PriceAsc:
                    filteredProducts = filteredProducts.OrderBy(p => p.price).ToList();
                    break;

                case SortOrder.PriceDesc:
                    filteredProducts = filteredProducts.OrderByDescending(p => p.price).ToList();
                    break;

                case SortOrder.Newest:
                    // Assumindo que ID maior = mais recente
                    filteredProducts = filteredProducts.OrderByDescending(p => p.id).ToList();
                    break;
            }
        }

        private void UpdateResultsCount()
        {
            if (resultsCountText != null)
            {
                resultsCountText.text = $"{filteredProducts.Count} produto{(filteredProducts.Count != 1 ? "s" : "")} encontrado{(filteredProducts.Count != 1 ? "s" : "")}";
            }
        }

        private void UpdatePriceText()
        {
            if (priceMinText != null && priceMinSlider != null)
                priceMinText.text = $"R$ {priceMinSlider.value:F2}";

            if (priceMaxText != null && priceMaxSlider != null)
                priceMaxText.text = $"R$ {priceMaxSlider.value:F2}";
        }

        private void DisplayFilteredProducts()
        {
            if (catalogLoader != null)
            {
                // TODO: Integrar com CatalogLoader para exibir produtos filtrados
                Debug.Log($"Displaying {filteredProducts.Count} products");
                
                // Enviar evento para atualizar o grid de produtos
                ProductGridUpdated?.Invoke(filteredProducts);
            }
        }

        private void AnimateFilterUpdate()
        {
            // Feedback visual de que filtros foram aplicados
            if (resultsCountText != null)
            {
                var animator = resultsCountText.GetComponent<UIAnimator>();
                if (animator != null)
                    animator.Play();
            }
        }

        public void ClearAllFilters()
        {
            if (searchInput != null)
                searchInput.text = "";

            if (categoryDropdown != null)
                categoryDropdown.value = 0;

            if (priceMinSlider != null)
                priceMinSlider.value = priceMinSlider.minValue;

            if (priceMaxSlider != null)
                priceMaxSlider.value = priceMaxSlider.maxValue;

            if (has3DToggle != null)
                has3DToggle.isOn = false;

            if (inStockToggle != null)
                inStockToggle.isOn = false;

            if (sortDropdown != null)
                sortDropdown.value = 0;

            ApplyFilters();
            ToastNotification.Info("Filtros limpos!");
        }

        public List<ProductData> GetFilteredProducts() => new List<ProductData>(filteredProducts);

        // Event para notificar quando produtos filtrados mudarem
        public System.Action<List<ProductData>> ProductGridUpdated;
    }
}

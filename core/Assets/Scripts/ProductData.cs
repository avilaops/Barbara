using UnityEngine;

namespace Barbara.Core
{
    /// <summary>
    /// Estrutura de dados para produtos do catálogo.
    /// </summary>
    [System.Serializable]
    public class ProductData
    {
        public string id;
        public string name;
        public string sku;
        public string description;
        public string category;
        public string size;
        public string color;
        public float price;
        public string[] images;
        public string model3dUrl;
        public int stock;
        public bool active;
    }

    [System.Serializable]
    public class CatalogResponse
    {
        public ProductData[] items;
        public int total;
        public int page;
        public int pages;
    }
}

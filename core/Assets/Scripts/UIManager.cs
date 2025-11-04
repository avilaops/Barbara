using UnityEngine;
using UnityEngine.UI;

namespace Barbara.Core
{
    /// <summary>
    /// Gerencia interface do usuário (menu lateral, telas).
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject avatarPanel;
        [SerializeField] private GameObject catalogPanel;
        [SerializeField] private GameObject shopPanel;

        [Header("Buttons")]
        [SerializeField] private Button avatarButton;
        [SerializeField] private Button catalogButton;
        [SerializeField] private Button shopButton;

        [Header("References")]
        [SerializeField] private AvatarManager avatarManager;
        [SerializeField] private CatalogLoader catalogLoader;

        private void Start()
        {
            SetupButtons();
            ShowCatalog(); // Iniciar com catálogo visível
        }

        private void SetupButtons()
        {
            if (avatarButton != null)
                avatarButton.onClick.AddListener(ShowAvatar);

            if (catalogButton != null)
                catalogButton.onClick.AddListener(ShowCatalog);

            if (shopButton != null)
                shopButton.onClick.AddListener(ShowShop);
        }

        public void ShowAvatar()
        {
            HideAllPanels();
            if (avatarPanel != null)
                avatarPanel.SetActive(true);

            Debug.Log("Painel Avatar ativo");
        }

        public void ShowCatalog()
        {
            HideAllPanels();
            if (catalogPanel != null)
                catalogPanel.SetActive(true);

            Debug.Log("Painel Catálogo ativo");
        }

        public void ShowShop()
        {
            HideAllPanels();
            if (shopPanel != null)
                shopPanel.SetActive(true);

            Debug.Log("Painel Loja ativo");
        }

        private void HideAllPanels()
        {
            if (avatarPanel != null) avatarPanel.SetActive(false);
            if (catalogPanel != null) catalogPanel.SetActive(false);
            if (shopPanel != null) shopPanel.SetActive(false);
        }

        /// <summary>
        /// Solicita geração de avatar personalizado (chamado por botão na UI).
        /// </summary>
        public void OnRequestCustomAvatar()
        {
            // Exemplo: pegar URLs de campos de input
            string userId = "user123";
            string frontUrl = "https://example.com/front.jpg";
            string sideUrl = "https://example.com/side.jpg";

            if (avatarManager != null)
            {
                avatarManager.RequestCustomAvatar(userId, frontUrl, sideUrl);
            }
        }

        /// <summary>
        /// Recarrega catálogo (chamado por botão refresh).
        /// </summary>
        public void OnRefreshCatalog()
        {
            if (catalogLoader != null)
            {
                catalogLoader.LoadCatalog();
            }
        }
    }
}

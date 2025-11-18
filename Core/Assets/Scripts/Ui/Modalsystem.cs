using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

namespace Barbara.UI
{
    /// <summary>
    /// Sistema de modais reutilizável com backdrop blur
    /// </summary>
    public class ModalSystem : MonoBehaviour
    {
        [Header("Modal Components")]
        [SerializeField] private GameObject modalPanel;
        [SerializeField] private GameObject backdrop;
        [SerializeField] private Transform modalContent;
        [SerializeField] private Button closeButton;

        [Header("Product Modal Template")]
        [SerializeField] private GameObject productModalTemplate;

        [Header("Confirm Modal Template")]
        [SerializeField] private GameObject confirmModalTemplate;

        [Header("Avatar Modal Template")]
        [SerializeField] private GameObject avatarModalTemplate;

        private static ModalSystem _instance;
        public static ModalSystem Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<ModalSystem>();
                return _instance;
            }
        }

        private GameObject currentModal;
        private Action onCloseCallback;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            if (closeButton != null)
                closeButton.onClick.AddListener(Close);

            if (backdrop != null)
            {
                var backdropButton = backdrop.GetComponent<Button>();
                if (backdropButton == null)
                    backdropButton = backdrop.AddComponent<Button>();
                backdropButton.onClick.AddListener(Close);
            }

            HideModal();
        }

        public void ShowProductModal(ProductData product)
        {
            if (productModalTemplate == null)
            {
                Debug.LogError("Product modal template not assigned!");
                return;
            }

            ShowModal(productModalTemplate, (modal) =>
            {
                // Preencher informações do produto
                var nameText = modal.transform.Find("ProductName")?.GetComponent<TextMeshProUGUI>();
                if (nameText != null)
                    nameText.text = product.name;

                var descText = modal.transform.Find("Description")?.GetComponent<TextMeshProUGUI>();
                if (descText != null)
                    descText.text = product.description;

                var priceText = modal.transform.Find("Price")?.GetComponent<TextMeshProUGUI>();
                if (priceText != null)
                    priceText.text = $"R$ {product.price:F2}";

                // Botões de ação
                var tryOnBtn = modal.transform.Find("TryOnButton")?.GetComponent<Button>();
                if (tryOnBtn != null)
                {
                    tryOnBtn.onClick.RemoveAllListeners();
                    tryOnBtn.onClick.AddListener(() =>
                    {
                        OnProductTryOn(product);
                        Close();
                    });
                }

                var buyBtn = modal.transform.Find("BuyButton")?.GetComponent<Button>();
                if (buyBtn != null)
                {
                    buyBtn.onClick.RemoveAllListeners();
                    buyBtn.onClick.AddListener(() =>
                    {
                        OnProductBuy(product);
                        Close();
                    });
                }

                // Preview 3D
                var preview3D = modal.transform.Find("3DPreview");
                if (preview3D != null && !string.IsNullOrEmpty(product.model3dUrl))
                {
                    // TODO: Carregar modelo 3D
                    Debug.Log($"Loading 3D model: {product.model3dUrl}");
                }
            });
        }

        public void ShowConfirmModal(string title, string message, Action onConfirm, Action onCancel = null)
        {
            if (confirmModalTemplate == null)
            {
                Debug.LogError("Confirm modal template not assigned!");
                return;
            }

            ShowModal(confirmModalTemplate, (modal) =>
            {
                var titleText = modal.transform.Find("Title")?.GetComponent<TextMeshProUGUI>();
                if (titleText != null)
                    titleText.text = title;

                var messageText = modal.transform.Find("Message")?.GetComponent<TextMeshProUGUI>();
                if (messageText != null)
                    messageText.text = message;

                var confirmBtn = modal.transform.Find("ConfirmButton")?.GetComponent<Button>();
                if (confirmBtn != null)
                {
                    confirmBtn.onClick.RemoveAllListeners();
                    confirmBtn.onClick.AddListener(() =>
                    {
                        onConfirm?.Invoke();
                        Close();
                    });
                }

                var cancelBtn = modal.transform.Find("CancelButton")?.GetComponent<Button>();
                if (cancelBtn != null)
                {
                    cancelBtn.onClick.RemoveAllListeners();
                    cancelBtn.onClick.AddListener(() =>
                    {
                        onCancel?.Invoke();
                        Close();
                    });
                }
            });
        }

        public void ShowAvatarCreationModal(Action<string, string, string> onSubmit)
        {
            if (avatarModalTemplate == null)
            {
                Debug.LogError("Avatar modal template not assigned!");
                return;
            }

            ShowModal(avatarModalTemplate, (modal) =>
            {
                var userIdInput = modal.transform.Find("UserIdInput")?.GetComponent<TMP_InputField>();
                var frontUrlInput = modal.transform.Find("FrontUrlInput")?.GetComponent<TMP_InputField>();
                var sideUrlInput = modal.transform.Find("SideUrlInput")?.GetComponent<TMP_InputField>();
                
                var uploadFrontBtn = modal.transform.Find("UploadFrontButton")?.GetComponent<Button>();
                var uploadSideBtn = modal.transform.Find("UploadSideButton")?.GetComponent<Button>();

                // TODO: Implementar file picker para imagens
                if (uploadFrontBtn != null)
                {
                    uploadFrontBtn.onClick.RemoveAllListeners();
                    uploadFrontBtn.onClick.AddListener(() =>
                    {
                        OpenFilePicker((path) =>
                        {
                            if (frontUrlInput != null)
                                frontUrlInput.text = path;
                        });
                    });
                }

                if (uploadSideBtn != null)
                {
                    uploadSideBtn.onClick.RemoveAllListeners();
                    uploadSideBtn.onClick.AddListener(() =>
                    {
                        OpenFilePicker((path) =>
                        {
                            if (sideUrlInput != null)
                                sideUrlInput.text = path;
                        });
                    });
                }

                var submitBtn = modal.transform.Find("SubmitButton")?.GetComponent<Button>();
                if (submitBtn != null)
                {
                    submitBtn.onClick.RemoveAllListeners();
                    submitBtn.onClick.AddListener(() =>
                    {
                        string userId = userIdInput?.text ?? "user";
                        string frontUrl = frontUrlInput?.text ?? "";
                        string sideUrl = sideUrlInput?.text ?? "";

                        if (string.IsNullOrEmpty(frontUrl) || string.IsNullOrEmpty(sideUrl))
                        {
                            ToastNotification.Error("Por favor, envie as duas fotos!");
                            return;
                        }

                        onSubmit?.Invoke(userId, frontUrl, sideUrl);
                        Close();
                    });
                }
            });
        }

        private void ShowModal(GameObject template, Action<GameObject> setupCallback)
        {
            if (currentModal != null)
                Destroy(currentModal);

            currentModal = Instantiate(template, modalContent);
            setupCallback?.Invoke(currentModal);

            // Animar entrada
            if (modalPanel != null)
            {
                modalPanel.SetActive(true);
                var animator = modalPanel.GetComponent<UIAnimator>();
                if (animator != null)
                    animator.Play();
            }

            if (backdrop != null)
            {
                backdrop.SetActive(true);
                var backdropAnimator = backdrop.GetComponent<UIAnimator>();
                if (backdropAnimator != null)
                    backdropAnimator.Play();
            }
        }

        public void Close()
        {
            if (modalPanel != null)
            {
                var animator = modalPanel.GetComponent<UIAnimator>();
                if (animator != null)
                {
                    animator.PlayReverse();
                    StartCoroutine(HideAfterAnimation(0.3f));
                }
                else
                {
                    HideModal();
                }
            }

            onCloseCallback?.Invoke();
            onCloseCallback = null;
        }

        private IEnumerator HideAfterAnimation(float delay)
        {
            yield return new WaitForSeconds(delay);
            HideModal();
        }

        private void HideModal()
        {
            if (modalPanel != null)
                modalPanel.SetActive(false);

            if (backdrop != null)
                backdrop.SetActive(false);

            if (currentModal != null)
                Destroy(currentModal);
        }

        private void OnProductTryOn(ProductData product)
        {
            LoadingIndicator.ShowGlobal(LoadingIndicator.LoadingStyle.Spinner, "Aplicando roupa...");

            var tryOnController = FindObjectOfType<Barbara.Core.TryOnController>();
            if (tryOnController != null)
            {
                tryOnController.ApplyClothing(product);
                StartCoroutine(CompleteAction("Roupa aplicada com sucesso!"));
            }
        }

        private void OnProductBuy(ProductData product)
        {
            ShowConfirmModal(
                "Confirmar Compra",
                $"Deseja comprar {product.name} por R$ {product.price:F2}?",
                () =>
                {
                    // TODO: Integrar com sistema de pagamento
                    ToastNotification.Success($"{product.name} adicionado ao carrinho!");
                    Debug.Log($"Buying product: {product.id}");
                }
            );
        }

        private void OpenFilePicker(Action<string> onFileSelected)
        {
            // TODO: Implementar file picker nativo
            // Por enquanto, usar input field como fallback
            ToastNotification.Info("Upload de arquivos em desenvolvimento");
            Debug.Log("File picker opened");
        }

        private IEnumerator CompleteAction(string message)
        {
            yield return new WaitForSeconds(1f);
            LoadingIndicator.HideGlobal();
            ToastNotification.Success(message);
        }
    }
}

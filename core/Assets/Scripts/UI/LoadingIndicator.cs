using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace Barbara.UI
{
    /// <summary>
    /// Sistema de loading states com múltiplos estilos
    /// </summary>
    public class LoadingIndicator : MonoBehaviour
    {
        public enum LoadingStyle
        {
            Spinner,
            ProgressBar,
            Skeleton,
            Dots,
            Pulse
        }

        [Header("Components")]
        [SerializeField] private LoadingStyle style = LoadingStyle.Spinner;
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private Image spinnerImage;
        [SerializeField] private Slider progressBar;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Image pulseImage;

        [Header("Spinner Settings")]
        [SerializeField] private float spinSpeed = 180f;

        [Header("Dots Settings")]
        [SerializeField] private Transform dotsContainer;
        [SerializeField] private float dotAnimSpeed = 0.5f;

        [Header("Messages")]
        [SerializeField] private string[] loadingMessages = new string[]
        {
            "Carregando...",
            "Preparando experiência...",
            "Processando...",
            "Quase lá..."
        };

        private bool isLoading = false;
        private float progress = 0f;
        private int currentMessageIndex = 0;

        private void Update()
        {
            if (!isLoading) return;

            switch (style)
            {
                case LoadingStyle.Spinner:
                    UpdateSpinner();
                    break;
                case LoadingStyle.Dots:
                    UpdateDots();
                    break;
                case LoadingStyle.Pulse:
                    UpdatePulse();
                    break;
            }
        }

        public void Show(LoadingStyle loadingStyle = LoadingStyle.Spinner, string message = "")
        {
            style = loadingStyle;
            isLoading = true;
            progress = 0f;

            if (loadingPanel != null)
                loadingPanel.SetActive(true);

            if (!string.IsNullOrEmpty(message))
                SetMessage(message);
            else
                StartCoroutine(RotateMessages());

            SetupStyle();
        }

        public void Hide()
        {
            isLoading = false;
            StopAllCoroutines();

            if (loadingPanel != null)
            {
                var animator = loadingPanel.GetComponent<UIAnimator>();
                if (animator != null)
                    animator.PlayReverse();
                else
                    loadingPanel.SetActive(false);
            }
        }

        public void SetProgress(float value)
        {
            progress = Mathf.Clamp01(value);
            
            if (progressBar != null)
                progressBar.value = progress;

            if (statusText != null)
                statusText.text = $"{Mathf.RoundToInt(progress * 100)}%";
        }

        public void SetMessage(string message)
        {
            if (statusText != null)
                statusText.text = message;
        }

        private void SetupStyle()
        {
            // Ocultar todos os elementos
            if (spinnerImage != null) spinnerImage.gameObject.SetActive(false);
            if (progressBar != null) progressBar.gameObject.SetActive(false);
            if (dotsContainer != null) dotsContainer.gameObject.SetActive(false);
            if (pulseImage != null) pulseImage.gameObject.SetActive(false);

            // Ativar apenas o style selecionado
            switch (style)
            {
                case LoadingStyle.Spinner:
                    if (spinnerImage != null) spinnerImage.gameObject.SetActive(true);
                    break;

                case LoadingStyle.ProgressBar:
                    if (progressBar != null)
                    {
                        progressBar.gameObject.SetActive(true);
                        progressBar.value = 0f;
                    }
                    break;

                case LoadingStyle.Dots:
                    if (dotsContainer != null) dotsContainer.gameObject.SetActive(true);
                    break;

                case LoadingStyle.Pulse:
                    if (pulseImage != null) pulseImage.gameObject.SetActive(true);
                    break;

                case LoadingStyle.Skeleton:
                    // Skeleton é tratado por componentes filhos
                    break;
            }
        }

        private void UpdateSpinner()
        {
            if (spinnerImage == null) return;
            
            spinnerImage.transform.Rotate(0f, 0f, -spinSpeed * Time.deltaTime);
        }

        private void UpdateDots()
        {
            if (dotsContainer == null) return;

            float time = Time.time * dotAnimSpeed;
            
            for (int i = 0; i < dotsContainer.childCount; i++)
            {
                Transform dot = dotsContainer.GetChild(i);
                float scale = 1f + 0.5f * Mathf.Sin(time + i * 0.5f);
                dot.localScale = Vector3.one * scale;
            }
        }

        private void UpdatePulse()
        {
            if (pulseImage == null) return;

            float scale = 1f + 0.2f * Mathf.Sin(Time.time * 2f);
            pulseImage.transform.localScale = Vector3.one * scale;

            float alpha = 0.5f + 0.5f * Mathf.Sin(Time.time * 2f);
            Color color = pulseImage.color;
            color.a = alpha;
            pulseImage.color = color;
        }

        private IEnumerator RotateMessages()
        {
            while (isLoading)
            {
                if (loadingMessages.Length > 0)
                {
                    SetMessage(loadingMessages[currentMessageIndex]);
                    currentMessageIndex = (currentMessageIndex + 1) % loadingMessages.Length;
                }

                yield return new WaitForSeconds(2f);
            }
        }

        // Métodos estáticos para uso global
        private static LoadingIndicator _globalInstance;

        public static LoadingIndicator Global
        {
            get
            {
                if (_globalInstance == null)
                {
                    _globalInstance = FindObjectOfType<LoadingIndicator>();
                    if (_globalInstance == null)
                    {
                        Debug.LogWarning("No LoadingIndicator found in scene!");
                    }
                }
                return _globalInstance;
            }
        }

        public static void ShowGlobal(LoadingStyle style = LoadingStyle.Spinner, string message = "")
        {
            if (Global != null)
                Global.Show(style, message);
        }

        public static void HideGlobal()
        {
            if (Global != null)
                Global.Hide();
        }

        public static void UpdateProgress(float progress)
        {
            if (Global != null)
                Global.SetProgress(progress);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

namespace Barbara.UI
{
    /// <summary>
    /// Sistema de notificações toast moderno com queue
    /// </summary>
    public class ToastNotification : MonoBehaviour
    {
        public enum ToastType
        {
            Success,
            Error,
            Warning,
            Info
        }

        [System.Serializable]
        public class ToastStyle
        {
            public Color backgroundColor;
            public Color textColor;
            public Sprite icon;
        }

        [Header("Prefab")]
        [SerializeField] private GameObject toastPrefab;
        [SerializeField] private Transform toastContainer;

        [Header("Styles")]
        [SerializeField] private ToastStyle successStyle = new ToastStyle 
        { 
            backgroundColor = new Color(0.2f, 0.8f, 0.4f, 0.95f), 
            textColor = Color.white 
        };
        [SerializeField] private ToastStyle errorStyle = new ToastStyle 
        { 
            backgroundColor = new Color(0.9f, 0.3f, 0.3f, 0.95f), 
            textColor = Color.white 
        };
        [SerializeField] private ToastStyle warningStyle = new ToastStyle 
        { 
            backgroundColor = new Color(1f, 0.7f, 0.2f, 0.95f), 
            textColor = Color.black 
        };
        [SerializeField] private ToastStyle infoStyle = new ToastStyle 
        { 
            backgroundColor = new Color(0.3f, 0.6f, 0.9f, 0.95f), 
            textColor = Color.white 
        };

        [Header("Settings")]
        [SerializeField] private float defaultDuration = 3f;
        [SerializeField] private int maxToasts = 3;

        private static ToastNotification _instance;
        public static ToastNotification Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("ToastNotification");
                    _instance = go.AddComponent<ToastNotification>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        private Queue<ToastData> toastQueue = new Queue<ToastData>();
        private List<GameObject> activeToasts = new List<GameObject>();
        private bool isProcessing = false;

        private class ToastData
        {
            public string message;
            public ToastType type;
            public float duration;
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public static void Show(string message, ToastType type = ToastType.Info, float duration = 0f)
        {
            Instance.ShowToast(message, type, duration > 0f ? duration : Instance.defaultDuration);
        }

        public static void Success(string message, float duration = 0f)
        {
            Show(message, ToastType.Success, duration);
        }

        public static void Error(string message, float duration = 0f)
        {
            Show(message, ToastType.Error, duration);
        }

        public static void Warning(string message, float duration = 0f)
        {
            Show(message, ToastType.Warning, duration);
        }

        public static void Info(string message, float duration = 0f)
        {
            Show(message, ToastType.Info, duration);
        }

        private void ShowToast(string message, ToastType type, float duration)
        {
            var toastData = new ToastData
            {
                message = message,
                type = type,
                duration = duration
            };

            toastQueue.Enqueue(toastData);

            if (!isProcessing)
                StartCoroutine(ProcessToastQueue());
        }

        private IEnumerator ProcessToastQueue()
        {
            isProcessing = true;

            while (toastQueue.Count > 0)
            {
                // Limitar número de toasts visíveis
                while (activeToasts.Count >= maxToasts)
                {
                    yield return null;
                }

                var toastData = toastQueue.Dequeue();
                yield return StartCoroutine(DisplayToast(toastData));
            }

            isProcessing = false;
        }

        private IEnumerator DisplayToast(ToastData data)
        {
            if (toastPrefab == null || toastContainer == null)
            {
                Debug.LogWarning("Toast system not properly configured!");
                yield break;
            }

            // Criar toast
            GameObject toast = Instantiate(toastPrefab, toastContainer);
            activeToasts.Add(toast);

            // Configurar visual
            Image background = toast.GetComponent<Image>();
            TextMeshProUGUI text = toast.GetComponentInChildren<TextMeshProUGUI>();
            Image iconImage = toast.transform.Find("Icon")?.GetComponent<Image>();

            ToastStyle style = GetStyle(data.type);

            if (background != null)
                background.color = style.backgroundColor;

            if (text != null)
            {
                text.text = data.message;
                text.color = style.textColor;
            }

            if (iconImage != null && style.icon != null)
                iconImage.sprite = style.icon;

            // Animação de entrada
            UIAnimator animator = toast.GetComponent<UIAnimator>();
            if (animator == null)
                animator = toast.AddComponent<UIAnimator>();

            animator.Play();

            // Aguardar duração
            yield return new WaitForSeconds(data.duration);

            // Animação de saída
            animator.PlayReverse();
            yield return new WaitForSeconds(0.3f);

            // Remover
            activeToasts.Remove(toast);
            Destroy(toast);
        }

        private ToastStyle GetStyle(ToastType type)
        {
            switch (type)
            {
                case ToastType.Success:
                    return successStyle;
                case ToastType.Error:
                    return errorStyle;
                case ToastType.Warning:
                    return warningStyle;
                case ToastType.Info:
                default:
                    return infoStyle;
            }
        }
    }
}

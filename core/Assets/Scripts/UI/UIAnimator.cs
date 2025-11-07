using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Barbara.UI
{
    /// <summary>
    /// Sistema de animações fluidas para UI com easing functions
    /// </summary>
    public class UIAnimator : MonoBehaviour
    {
        public enum AnimationType
        {
            FadeIn,
            FadeOut,
            SlideFromLeft,
            SlideFromRight,
            SlideFromTop,
            SlideFromBottom,
            ScaleIn,
            ScaleOut,
            Bounce,
            Shake
        }

        public enum EasingType
        {
            Linear,
            EaseInOut,
            EaseOut,
            EaseIn,
            Elastic,
            Bounce
        }

        [Header("Animation Settings")]
        [SerializeField] private AnimationType animationType = AnimationType.FadeIn;
        [SerializeField] private EasingType easingType = EasingType.EaseInOut;
        [SerializeField] private float duration = 0.3f;
        [SerializeField] private float delay = 0f;
        [SerializeField] private bool playOnEnable = true;

        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private Vector3 originalPosition;
        private Vector3 originalScale;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();

            originalPosition = rectTransform.anchoredPosition;
            originalScale = rectTransform.localScale;
        }

        private void OnEnable()
        {
            if (playOnEnable)
                Play();
        }

        public void Play()
        {
            StopAllCoroutines();
            StartCoroutine(PlayAnimation());
        }

        public void PlayReverse()
        {
            StopAllCoroutines();
            StartCoroutine(PlayAnimationReverse());
        }

        private IEnumerator PlayAnimation()
        {
            if (delay > 0)
                yield return new WaitForSeconds(delay);

            float elapsed = 0f;

            // Setup inicial baseado no tipo de animação
            SetupInitialState();

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float easedT = ApplyEasing(t, easingType);

                ApplyAnimationFrame(easedT);

                yield return null;
            }

            // Garantir estado final
            ApplyAnimationFrame(1f);
        }

        private IEnumerator PlayAnimationReverse()
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float easedT = ApplyEasing(1f - t, easingType);

                ApplyAnimationFrame(easedT);

                yield return null;
            }

            SetupInitialState();
        }

        private void SetupInitialState()
        {
            switch (animationType)
            {
                case AnimationType.FadeIn:
                    canvasGroup.alpha = 0f;
                    break;
                
                case AnimationType.FadeOut:
                    canvasGroup.alpha = 1f;
                    break;

                case AnimationType.SlideFromLeft:
                    rectTransform.anchoredPosition = originalPosition + Vector2.left * 1000f;
                    break;

                case AnimationType.SlideFromRight:
                    rectTransform.anchoredPosition = originalPosition + Vector2.right * 1000f;
                    break;

                case AnimationType.SlideFromTop:
                    rectTransform.anchoredPosition = originalPosition + Vector2.up * 1000f;
                    break;

                case AnimationType.SlideFromBottom:
                    rectTransform.anchoredPosition = originalPosition + Vector2.down * 1000f;
                    break;

                case AnimationType.ScaleIn:
                    rectTransform.localScale = Vector3.zero;
                    break;

                case AnimationType.ScaleOut:
                    rectTransform.localScale = originalScale;
                    break;
            }
        }

        private void ApplyAnimationFrame(float t)
        {
            switch (animationType)
            {
                case AnimationType.FadeIn:
                    canvasGroup.alpha = t;
                    break;

                case AnimationType.FadeOut:
                    canvasGroup.alpha = 1f - t;
                    break;

                case AnimationType.SlideFromLeft:
                case AnimationType.SlideFromRight:
                case AnimationType.SlideFromTop:
                case AnimationType.SlideFromBottom:
                    Vector2 targetPos = originalPosition;
                    Vector2 startPos = GetSlideStartPosition();
                    rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
                    break;

                case AnimationType.ScaleIn:
                    rectTransform.localScale = Vector3.Lerp(Vector3.zero, originalScale, t);
                    break;

                case AnimationType.ScaleOut:
                    rectTransform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
                    break;

                case AnimationType.Bounce:
                    rectTransform.localScale = originalScale * (1f + Mathf.Sin(t * Mathf.PI) * 0.2f);
                    break;

                case AnimationType.Shake:
                    Vector2 shake = Random.insideUnitCircle * 10f * (1f - t);
                    rectTransform.anchoredPosition = originalPosition + shake;
                    break;
            }
        }

        private Vector2 GetSlideStartPosition()
        {
            switch (animationType)
            {
                case AnimationType.SlideFromLeft:
                    return originalPosition + Vector2.left * 1000f;
                case AnimationType.SlideFromRight:
                    return originalPosition + Vector2.right * 1000f;
                case AnimationType.SlideFromTop:
                    return originalPosition + Vector2.up * 1000f;
                case AnimationType.SlideFromBottom:
                    return originalPosition + Vector2.down * 1000f;
                default:
                    return originalPosition;
            }
        }

        private float ApplyEasing(float t, EasingType easing)
        {
            switch (easing)
            {
                case EasingType.Linear:
                    return t;

                case EasingType.EaseInOut:
                    return t < 0.5f 
                        ? 2f * t * t 
                        : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;

                case EasingType.EaseOut:
                    return 1f - Mathf.Pow(1f - t, 3f);

                case EasingType.EaseIn:
                    return t * t * t;

                case EasingType.Elastic:
                    const float c4 = (2f * Mathf.PI) / 3f;
                    return t == 0f ? 0f 
                        : t == 1f ? 1f 
                        : Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;

                case EasingType.Bounce:
                    const float n1 = 7.5625f;
                    const float d1 = 2.75f;
                    if (t < 1f / d1)
                        return n1 * t * t;
                    else if (t < 2f / d1)
                        return n1 * (t -= 1.5f / d1) * t + 0.75f;
                    else if (t < 2.5f / d1)
                        return n1 * (t -= 2.25f / d1) * t + 0.9375f;
                    else
                        return n1 * (t -= 2.625f / d1) * t + 0.984375f;

                default:
                    return t;
            }
        }

        public void Reset()
        {
            StopAllCoroutines();
            rectTransform.anchoredPosition = originalPosition;
            rectTransform.localScale = originalScale;
            canvasGroup.alpha = 1f;
        }
    }
}

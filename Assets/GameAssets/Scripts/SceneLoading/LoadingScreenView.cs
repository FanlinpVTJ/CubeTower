using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CubeGame.SceneLoading
{
    public sealed class LoadingScreenView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        private Tween fadeTween;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            SetVisible(false);
        }

        public async UniTask ShowAsync(float fadeDuration, Ease fadeEase)
        {
            KillFadeTween();
            gameObject.SetActive(true);

            if (canvasGroup == null)
            {
                return;
            }

            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;

            if (fadeDuration <= 0f)
            {
                canvasGroup.alpha = 1f;

                return;
            }

            fadeTween = canvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEase);
            await fadeTween.AsyncWaitForCompletion();
            fadeTween = null;
        }

        public async UniTask HideAsync(float fadeDuration, Ease fadeEase)
        {
            KillFadeTween();

            if (canvasGroup == null)
            {
                gameObject.SetActive(false);

                return;
            }

            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;

            if (fadeDuration <= 0f)
            {
                canvasGroup.alpha = 0f;
                gameObject.SetActive(false);

                return;
            }

            fadeTween = canvasGroup.DOFade(0f, fadeDuration).SetEase(fadeEase);
            await fadeTween.AsyncWaitForCompletion();
            fadeTween = null;
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            KillFadeTween();

            if (canvasGroup == null)
            {
                return;
            }

            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        private void Reset()
        {
            canvasGroup = GetComponentInChildren<CanvasGroup>(true);
        }

        private void OnValidate()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponentInChildren<CanvasGroup>(true);
            }
        }

        private void SetVisible(bool isVisible)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = isVisible ? 1f : 0f;
                canvasGroup.blocksRaycasts = isVisible;
                canvasGroup.interactable = isVisible;
            }

            gameObject.SetActive(isVisible);
        }

        private void KillFadeTween()
        {
            if (fadeTween == null)
            {
                return;
            }

            fadeTween.Kill();
            fadeTween = null;
        }
    }
}

using CubeGame.ObjectPoolManager;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CubeGame.Feedback
{
    public sealed class FeedbackItemView : PooledObject
    {
        [SerializeField] private RectTransform root;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text textView;

        private Sequence lifetimeSequence;

        public RectTransform Root => root != null ? root : (RectTransform)transform;

        public void Show(string text, FeedbackConfig feedbackConfig)
        {
            KillLifetimeSequence();
            Root.localScale = Vector3.one;

            if (textView != null)
            {
                textView.text = text;
            }

            if (canvasGroup == null)
            {
                Despawn();

                return;
            }

            canvasGroup.alpha = 0f;

            float visibleDuration = ResolveVisibleDuration(feedbackConfig);
            float fadeDuration = ResolveFadeDuration(feedbackConfig);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(canvasGroup.DOFade(1f, fadeDuration).SetEase(ResolveFadeInEase(feedbackConfig)));
            sequence.AppendInterval(visibleDuration);
            sequence.Append(canvasGroup.DOFade(0f, fadeDuration).SetEase(ResolveFadeOutEase(feedbackConfig)));
            sequence.OnComplete(OnLifetimeCompleted);
            lifetimeSequence = sequence;
        }

        private void OnDisable()
        {
            KillLifetimeSequence();

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }
        }

        private void OnLifetimeCompleted()
        {
            lifetimeSequence = null;
            Despawn();
        }

        private float ResolveVisibleDuration(FeedbackConfig feedbackConfig)
        {
            if (feedbackConfig == null)
            {
                return 1f;
            }

            return feedbackConfig.VisibleDuration;
        }

        private float ResolveFadeDuration(FeedbackConfig feedbackConfig)
        {
            if (feedbackConfig == null)
            {
                return 0.15f;
            }

            return feedbackConfig.FadeDuration;
        }

        private Ease ResolveFadeInEase(FeedbackConfig feedbackConfig)
        {
            if (feedbackConfig == null)
            {
                return Ease.OutQuad;
            }

            return feedbackConfig.FadeInEase;
        }

        private Ease ResolveFadeOutEase(FeedbackConfig feedbackConfig)
        {
            if (feedbackConfig == null)
            {
                return Ease.InQuad;
            }

            return feedbackConfig.FadeOutEase;
        }

        private void KillLifetimeSequence()
        {
            if (lifetimeSequence == null)
            {
                return;
            }

            lifetimeSequence.Kill();
            lifetimeSequence = null;
        }
    }
}

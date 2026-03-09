using DG.Tweening;
using UnityEngine;

namespace CubeGame.Feedback
{
    [CreateAssetMenu(menuName = "CubeGame/Feedback/Config", fileName = "FeedbackConfig")]
    public sealed class FeedbackConfig : ScriptableObject
    {
        [Header("Lifetime")]
        [SerializeField] private float visibleDuration = 1f;

        [Header("Fade Animation")]
        [SerializeField] private float fadeDuration = 0.15f;
        [SerializeField] private Ease fadeInEase = Ease.OutQuad;
        [SerializeField] private Ease fadeOutEase = Ease.InQuad;

        public float VisibleDuration => visibleDuration;
        public float FadeDuration => fadeDuration;
        public Ease FadeInEase => fadeInEase;
        public Ease FadeOutEase => fadeOutEase;
    }
}

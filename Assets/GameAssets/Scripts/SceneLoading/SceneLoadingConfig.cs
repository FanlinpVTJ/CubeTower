using DG.Tweening;
using UnityEngine;

namespace CubeGame.SceneLoading
{
    [CreateAssetMenu(menuName = "CubeGame/Scene Loading/Config", fileName = "SceneLoadingConfig")]
    public sealed class SceneLoadingConfig : ScriptableObject
    {
        [Header("Loading")]
        [SerializeField] private float minimumLoadingDuration = 1f;

        [Header("Fade Animation")]
        [SerializeField] private float fadeDuration = 0.2f;
        [SerializeField] private Ease fadeInEase = Ease.OutQuad;
        [SerializeField] private Ease fadeOutEase = Ease.InQuad;

        public float MinimumLoadingDuration => minimumLoadingDuration;
        public float FadeDuration => fadeDuration;
        public Ease FadeInEase => fadeInEase;
        public Ease FadeOutEase => fadeOutEase;
    }
}

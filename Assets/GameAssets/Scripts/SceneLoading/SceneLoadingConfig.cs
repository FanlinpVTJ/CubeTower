using UnityEngine;

namespace CubeGame.SceneLoading
{
    [CreateAssetMenu(menuName = "CubeGame/Scene Loading/Config", fileName = "SceneLoadingConfig")]
    public sealed class SceneLoadingConfig : ScriptableObject
    {
        [SerializeField] private float minimumLoadingDuration = 1f;
        [SerializeField] private float fadeDuration = 0.2f;

        public float MinimumLoadingDuration => minimumLoadingDuration;
        public float FadeDuration => fadeDuration;
    }
}

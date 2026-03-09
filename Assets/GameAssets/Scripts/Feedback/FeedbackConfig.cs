using UnityEngine;

namespace CubeGame.Feedback
{
    [CreateAssetMenu(menuName = "CubeGame/Feedback/Config", fileName = "FeedbackConfig")]
    public sealed class FeedbackConfig : ScriptableObject
    {
        [SerializeField] private float visibleDuration = 1f;
        [SerializeField] private float fadeDuration = 0.15f;

        public float VisibleDuration => visibleDuration;
        public float FadeDuration => fadeDuration;
    }
}

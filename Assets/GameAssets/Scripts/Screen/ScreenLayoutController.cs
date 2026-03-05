using UnityEngine;

namespace CubeGame.Screen
{
    public sealed class ScreenLayoutController : MonoBehaviour
    {
        [Header("Zone roots")]
        [SerializeField] private RectTransform leftZoneRoot;
        [SerializeField] private RectTransform rightZoneRoot;
        [SerializeField] private RectTransform scrollZoneRoot;

        [Header("Normalized layout")]
        [SerializeField, Range(0.15f, 0.5f)] private float scrollHeight = 0.3f;
        [SerializeField, Range(0.2f, 0.8f)] private float topSplit = 0.5f;

        private void Awake()
        {
            ApplyLayout();
        }

        [ContextMenu("Apply Layout")]
        public void ApplyLayout()
        {
            if (leftZoneRoot == null || rightZoneRoot == null || scrollZoneRoot == null)
            {
                Debug.LogWarning("[ScreenLayoutController] Cannot apply layout: zone roots are not assigned.");
                return;
            }

            SetAnchors(scrollZoneRoot, new Vector2(0f, 0f), new Vector2(1f, scrollHeight));
            SetAnchors(leftZoneRoot, new Vector2(0f, scrollHeight), new Vector2(topSplit, 1f));
            SetAnchors(rightZoneRoot, new Vector2(topSplit, scrollHeight), new Vector2(1f, 1f));
        }

        private static void SetAnchors(RectTransform target, Vector2 min, Vector2 max)
        {
            target.anchorMin = min;
            target.anchorMax = max;
            target.offsetMin = Vector2.zero;
            target.offsetMax = Vector2.zero;
        }
    }
}

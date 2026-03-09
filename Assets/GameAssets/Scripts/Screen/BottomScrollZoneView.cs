using UnityEngine;
using UnityEngine.UI;

namespace CubeGame.Screen
{
    public sealed class BottomScrollZoneView : ScreenZoneBase, IScrollZone, IScrollView
    {
        [SerializeField] private RectTransform contentRoot;
        [SerializeField] private ScrollRect scrollRect;

        public RectTransform ContentRoot => contentRoot != null ? contentRoot : Root;
        public ScrollRect Scroll => scrollRect;

        protected override void OnValidate()
        {
            base.OnValidate();

            if (contentRoot == null)
            {
                contentRoot = Root;
            }

            if (scrollRect == null)
            {
                scrollRect = GetComponentInChildren<ScrollRect>(true);
            }
        }
    }
}

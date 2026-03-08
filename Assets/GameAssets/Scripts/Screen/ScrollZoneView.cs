using CubeGame.Screen;
using UnityEngine;
using UnityEngine.UI;

namespace CubeGame
{
    public class ScrollZoneView : ScreenZoneBase, IScrollZone, IScrollView
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

using CubeGame.Screen;
using UnityEngine;

namespace CubeGame
{
    public class ScrollZoneView : ScreenZoneBase, IScrollZone
    {
        [SerializeField] private RectTransform contentRoot;

        public RectTransform ContentRoot => contentRoot != null ? contentRoot : Root;

        protected override void OnValidate()
        {
            base.OnValidate();
            if (contentRoot == null)
            {
                contentRoot = Root;
            }
        }
    }
}

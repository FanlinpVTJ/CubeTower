using UnityEngine;

namespace CubeGame.Screen
{
    public sealed class BottomScrollZoneView : ScreenZoneBase, IScrollZone
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

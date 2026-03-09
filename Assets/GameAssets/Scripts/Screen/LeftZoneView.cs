using UnityEngine;

namespace CubeGame.Screen
{
    public sealed class LeftZoneView : ScreenZoneBase, ILeftZone, IHoleView
    {
        [SerializeField] private RectTransform holeRoot;

        public RectTransform HoleRoot => holeRoot != null ? holeRoot : Root;

        protected override void Reset()
        {
            base.Reset();
            holeRoot = Root;
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (holeRoot == null)
            {
                holeRoot = Root;
            }
        }
    }
}

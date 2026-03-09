using UnityEngine;

namespace CubeGame.Screen
{
    public sealed class LeftZoneView : ScreenZoneBase, ILeftZone, IHoleView
    {
        private const int GIZMO_SEGMENTS = 40;

        [SerializeField] private RectTransform holeRoot;
        [SerializeField] private bool drawHoleGizmo = true;
        [SerializeField] private Color holeGizmoColor = Color.cyan;

        public RectTransform HoleRoot => holeRoot != null ? holeRoot : Root;

        private void OnDrawGizmosSelected()
        {
            if (!drawHoleGizmo)
            {
                return;
            }

            RectTransform currentHoleRoot = HoleRoot;

            if (currentHoleRoot == null)
            {
                return;
            }

            Rect holeRect = currentHoleRoot.rect;
            Vector2 center = holeRect.center;
            float radiusX = holeRect.width * 0.5f;
            float radiusY = holeRect.height * 0.5f;

            if (radiusX <= 0f || radiusY <= 0f)
            {
                return;
            }

            Gizmos.color = holeGizmoColor;
            Vector3 previousPoint = ResolveEllipsePoint(currentHoleRoot, center, radiusX, radiusY, 0f);

            for (int i = 1; i <= GIZMO_SEGMENTS; i++)
            {
                float progress = (float)i / GIZMO_SEGMENTS;
                float angle = progress * Mathf.PI * 2f;
                Vector3 currentPoint = ResolveEllipsePoint(currentHoleRoot, center, radiusX, radiusY, angle);
                Gizmos.DrawLine(previousPoint, currentPoint);
                previousPoint = currentPoint;
            }
        }

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

        private Vector3 ResolveEllipsePoint(
            RectTransform currentHoleRoot,
            Vector2 center,
            float radiusX,
            float radiusY,
            float angle)
        {
            float pointX = center.x + Mathf.Cos(angle) * radiusX;
            float pointY = center.y + Mathf.Sin(angle) * radiusY;
            Vector3 localPoint = new Vector3(pointX, pointY, 0f);
            Vector3 worldPoint = currentHoleRoot.TransformPoint(localPoint);

            return worldPoint;
        }
    }
}

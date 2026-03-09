using CubeGame.Screen;
using UnityEngine;

namespace CubeGame.Hole
{
    public sealed class OvalHoleHitRule : IHoleDisposalRule
    {
        private readonly IHoleView holeView;

        public OvalHoleHitRule(IHoleView holeView)
        {
            this.holeView = holeView;
        }

        public HoleDisposalFailureReasonType Validate(HoleDisposalContext context)
        {
            if (context == null || context.DragElement == null)
            {
                return HoleDisposalFailureReasonType.InvalidElement;
            }

            if (holeView == null || holeView.HoleRoot == null)
            {
                return HoleDisposalFailureReasonType.OutsideHole;
            }

            RectTransform holeRoot = holeView.HoleRoot;
            Camera eventCamera = ResolveEventCamera(holeRoot);
            Vector2 localPoint;
            bool isConverted = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                holeRoot,
                context.DragPosition,
                eventCamera,
                out localPoint);

            if (!isConverted)
            {
                return HoleDisposalFailureReasonType.OutsideHole;
            }

            Rect holeRect = holeRoot.rect;
            Vector2 center = holeRect.center;
            float radiusX = holeRect.width * 0.5f;
            float radiusY = holeRect.height * 0.5f;

            if (radiusX <= 0f || radiusY <= 0f)
            {
                return HoleDisposalFailureReasonType.OutsideHole;
            }

            float normalizedX = (localPoint.x - center.x) / radiusX;
            float normalizedY = (localPoint.y - center.y) / radiusY;
            float ellipseValue = normalizedX * normalizedX + normalizedY * normalizedY;

            if (ellipseValue > 1f)
            {
                return HoleDisposalFailureReasonType.OutsideHole;
            }

            return HoleDisposalFailureReasonType.None;
        }

        private Camera ResolveEventCamera(RectTransform zoneRoot)
        {
            Canvas canvas = zoneRoot.GetComponentInParent<Canvas>();

            if (canvas == null)
            {
                return null;
            }

            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return null;
            }

            return canvas.worldCamera;
        }
    }
}

using CubeGame.Screen;
using UnityEngine;

namespace CubeGame.Tower
{
    public sealed class FirstBlockInsideRightZoneRule : ITowerPlacementRule
    {
        private readonly IRightZone rightZone;

        public FirstBlockInsideRightZoneRule(IRightZone rightZone)
        {
            this.rightZone = rightZone;
        }

        public TowerPlacementFailureReasonType Validate(TowerPlacementContext context, TowerState towerState)
        {
            if (towerState != null && towerState.HasBlocks)
            {
                return TowerPlacementFailureReasonType.None;
            }

            if (rightZone == null || rightZone.Root == null)
            {
                return TowerPlacementFailureReasonType.NotInRightZone;
            }

            Camera eventCamera = ResolveEventCamera(rightZone.Root);
            bool isInside = IsInsideRightZoneWithFullElement(context, rightZone.Root, eventCamera);

            if (isInside)
            {
                return TowerPlacementFailureReasonType.None;
            }

            return TowerPlacementFailureReasonType.NotInRightZone;
        }

        private bool IsInsideRightZoneWithFullElement(
            TowerPlacementContext context,
            RectTransform zoneRoot,
            Camera eventCamera)
        {
            Vector2 localPoint;
            bool isConverted = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                zoneRoot,
                context.CandidatePosition,
                eventCamera,
                out localPoint);

            if (!isConverted)
            {
                return false;
            }

            Rect rect = zoneRoot.rect;
            float halfWidth = context.ElementSize.x * 0.5f;
            float halfHeight = context.ElementSize.y * 0.5f;
            float minX = rect.xMin + halfWidth;
            float maxX = rect.xMax - halfWidth;
            float minY = rect.yMin + halfHeight;
            float maxY = rect.yMax - halfHeight;
            bool isInsideX = localPoint.x >= minX && localPoint.x <= maxX;
            bool isInsideY = localPoint.y >= minY && localPoint.y <= maxY;

            return isInsideX && isInsideY;
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

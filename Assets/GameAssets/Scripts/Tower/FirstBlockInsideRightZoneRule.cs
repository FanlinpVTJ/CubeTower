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
            bool isInside = RectTransformUtility.RectangleContainsScreenPoint(
                rightZone.Root,
                context.CandidatePosition,
                eventCamera);

            if (isInside)
            {
                return TowerPlacementFailureReasonType.None;
            }

            return TowerPlacementFailureReasonType.NotInRightZone;
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

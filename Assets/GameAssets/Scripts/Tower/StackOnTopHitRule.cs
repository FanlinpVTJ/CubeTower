using UnityEngine;

namespace CubeGame.Tower
{
    public sealed class StackOnTopHitRule : ITowerPlacementRule
    {
        public TowerPlacementFailureReasonType Validate(TowerPlacementContext context, TowerState towerState)
        {
            if (towerState == null || !towerState.HasBlocks)
            {
                return TowerPlacementFailureReasonType.None;
            }

            TowerBlockEntry topBlock = towerState.GetTopBlock();

            if (topBlock == null)
            {
                return TowerPlacementFailureReasonType.None;
            }

            bool isPointerOverTopBlock = IsPointerInsideBlock(context.PointerScreenPosition, topBlock);

            if (isPointerOverTopBlock)
            {
                return TowerPlacementFailureReasonType.None;
            }

            return TowerPlacementFailureReasonType.MustPlaceOnTopBlock;
        }

        private bool IsPointerInsideBlock(Vector2 pointerPosition, TowerBlockEntry blockEntry)
        {
            float halfWidth = blockEntry.Size.x * 0.5f;
            float halfHeight = blockEntry.Size.y * 0.5f;
            float minX = blockEntry.Position.x - halfWidth;
            float maxX = blockEntry.Position.x + halfWidth;
            float minY = blockEntry.Position.y - halfHeight;
            float maxY = blockEntry.Position.y + halfHeight;
            bool isInsideX = pointerPosition.x >= minX && pointerPosition.x <= maxX;
            bool isInsideY = pointerPosition.y >= minY && pointerPosition.y <= maxY;

            return isInsideX && isInsideY;
        }
    }
}

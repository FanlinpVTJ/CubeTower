using UnityEngine;

namespace CubeGame.Tower
{
    public sealed class TowerPositionResolver : ITowerPositionResolver
    {
        private readonly TowerConfig towerConfig;

        public TowerPositionResolver(TowerConfig towerConfig)
        {
            this.towerConfig = towerConfig;
        }

        public Vector2 Resolve(TowerState towerState, Vector2 pointerScreenPosition, Vector2 elementSize)
        {
            TowerBlockEntry topBlock = towerState.GetTopBlock();

            if (topBlock == null)
            {
                return pointerScreenPosition;
            }

            float horizontalRange = elementSize.x * GetMaxHorizontalOffsetFactor();
            float randomOffset = Random.Range(-horizontalRange, horizontalRange);
            float targetX = topBlock.Position.x + randomOffset;
            float targetY = topBlock.Position.y + topBlock.Size.y;
            Vector2 targetPosition = new Vector2(targetX, targetY);

            return targetPosition;
        }

        private float GetMaxHorizontalOffsetFactor()
        {
            if (towerConfig == null)
            {
                return 0.5f;
            }

            return Mathf.Clamp01(towerConfig.MaxHorizontalOffsetFactor);
        }
    }
}

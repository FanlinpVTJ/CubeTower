using CubeGame.Screen;
using UnityEngine;

namespace CubeGame.Tower
{
    public sealed class TowerPositionResolver : ITowerPositionResolver
    {
        private readonly IRightZone rightZone;
        private readonly TowerConfig towerConfig;

        public TowerPositionResolver(IRightZone rightZone, TowerConfig towerConfig)
        {
            this.rightZone = rightZone;
            this.towerConfig = towerConfig;
        }

        public Vector2 Resolve(TowerState towerState, Vector2 dragPosition, Vector2 pointerScreenPosition, Vector2 elementSize)
        {
            TowerBlockEntry topBlock = towerState.GetTopBlock();

            if (topBlock == null)
            {
                return ResolveFirstBlockPosition(dragPosition, elementSize);
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

        private Vector2 ResolveFirstBlockPosition(Vector2 currentPosition, Vector2 elementSize)
        {
            if (rightZone == null || rightZone.Root == null)
            {
                return currentPosition;
            }

            RectTransform zoneRoot = rightZone.Root;
            Camera eventCamera = ResolveEventCamera(zoneRoot);
            Vector2 localPoint;
            bool isConverted = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                zoneRoot,
                currentPosition,
                eventCamera,
                out localPoint);

            if (!isConverted)
            {
                return currentPosition;
            }

            Rect rect = zoneRoot.rect;
            float halfWidth = elementSize.x * 0.5f;
            float halfHeight = elementSize.y * 0.5f;
            float minX = rect.xMin + halfWidth;
            float maxX = rect.xMax - halfWidth;
            float minY = rect.yMin + halfHeight;
            float maxY = rect.yMax - halfHeight;
            float clampedX = Mathf.Clamp(localPoint.x, minX, maxX);
            float clampedY = Mathf.Clamp(localPoint.y, minY, maxY);
            bool isClamped = !Mathf.Approximately(clampedX, localPoint.x)
                || !Mathf.Approximately(clampedY, localPoint.y);
            Vector2 correctedLocalPoint = new Vector2(clampedX, clampedY);

            if (isClamped)
            {
                Vector2 centerPoint = rect.center;
                float bias = GetFirstBlockCenterBias();
                correctedLocalPoint = Vector2.Lerp(correctedLocalPoint, centerPoint, bias);
            }

            Vector3 worldPoint = zoneRoot.TransformPoint(correctedLocalPoint);
            Vector2 resultPosition = worldPoint;

            return resultPosition;
        }

        private float GetFirstBlockCenterBias()
        {
            if (towerConfig == null)
            {
                return 0.2f;
            }

            return Mathf.Clamp01(towerConfig.FirstBlockCenterBias);
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

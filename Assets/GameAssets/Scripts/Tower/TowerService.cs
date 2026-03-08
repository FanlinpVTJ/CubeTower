using System.Collections.Generic;
using CubeGame.Drag;
using CubeGame.Screen;
using UnityEngine;

namespace CubeGame.Tower
{
    public sealed class TowerService : ITowerService
    {
        private readonly TowerState towerState;
        private readonly ITowerPlacementRuleValidator ruleValidator;
        private readonly ITowerPositionResolver towerPositionResolver;
        private readonly IRightZone rightZone;
        private readonly TowerConfig towerConfig;

        public TowerService(
            TowerState towerState,
            ITowerPlacementRuleValidator ruleValidator,
            ITowerPositionResolver towerPositionResolver,
            IRightZone rightZone,
            TowerConfig towerConfig)
        {
            this.towerState = towerState;
            this.ruleValidator = ruleValidator;
            this.towerPositionResolver = towerPositionResolver;
            this.rightZone = rightZone;
            this.towerConfig = towerConfig;
        }

        public TowerPlacementResult TryPlace(IDragElement dragElement, Vector2 pointerScreenPosition)
        {
            if (dragElement == null || dragElement.Root == null)
            {
                TowerPlacementResult invalidResult = new TowerPlacementResult(
                    false,
                    TowerPlacementFailureReasonType.InvalidElement,
                    Vector2.zero);

                return invalidResult;
            }

            Vector2 elementSize = ResolveElementSize(dragElement.Root);
            Vector2 candidatePosition = ResolveCandidatePosition(dragElement, pointerScreenPosition, elementSize);
            TowerPlacementContext context = new TowerPlacementContext(
                dragElement,
                pointerScreenPosition,
                candidatePosition,
                elementSize);
            TowerPlacementFailureReasonType failureReason = ruleValidator.Validate(context, towerState);

            if (failureReason != TowerPlacementFailureReasonType.None)
            {
                TowerPlacementResult failureResult = new TowerPlacementResult(false, failureReason, candidatePosition);

                return failureResult;
            }

            if (rightZone != null && rightZone.Root != null)
            {
                dragElement.Root.SetParent(rightZone.Root, true);
            }

            dragElement.Root.position = candidatePosition;

            TowerBlockEntry blockEntry = new TowerBlockEntry(
                dragElement,
                ResolveElementIdentifier(dragElement),
                candidatePosition,
                elementSize);
            towerState.AddBlock(blockEntry);

            TowerPlacementResult successResult = new TowerPlacementResult(
                true,
                TowerPlacementFailureReasonType.None,
                candidatePosition);

            return successResult;
        }

        public List<TowerBlockEntry> GetBlocks()
        {
            return towerState.Blocks;
        }

        private Vector2 ResolveElementSize(RectTransform rectTransform)
        {
            Vector2 size = rectTransform.rect.size;
            Vector3 scale = rectTransform.lossyScale;
            Vector2 result = new Vector2(size.x * scale.x, size.y * scale.y);

            return result;
        }

        private string ResolveElementIdentifier(IDragElement dragElement)
        {
            if (dragElement.Data == null)
            {
                return string.Empty;
            }

            return dragElement.Data.ElementId;
        }

        private Vector2 ResolveCandidatePosition(IDragElement dragElement, Vector2 pointerScreenPosition, Vector2 elementSize)
        {
            if (towerState == null || !towerState.HasBlocks)
            {
                return ResolveFirstBlockPosition(dragElement.Root.position, elementSize);
            }

            return towerPositionResolver.Resolve(towerState, pointerScreenPosition, elementSize);
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

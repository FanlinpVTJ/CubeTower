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

        public TowerService(
            TowerState towerState,
            ITowerPlacementRuleValidator ruleValidator,
            ITowerPositionResolver towerPositionResolver,
            IRightZone rightZone)
        {
            this.towerState = towerState;
            this.ruleValidator = ruleValidator;
            this.towerPositionResolver = towerPositionResolver;
            this.rightZone = rightZone;
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
                return dragElement.Root.position;
            }

            return towerPositionResolver.Resolve(towerState, pointerScreenPosition, elementSize);
        }
    }
}

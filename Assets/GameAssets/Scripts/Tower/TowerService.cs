using System.Collections.Generic;
using CubeGame.Drag;
using CubeGame.ObjectPoolManager;
using CubeGame.Screen;
using CubeGame.Scroll;
using UnityEngine;

namespace CubeGame.Tower
{
    public sealed class TowerService : ITowerService
    {
        private readonly TowerState towerState;
        private readonly ITowerPlacementRuleValidator ruleValidator;
        private readonly ITowerPositionResolver towerPositionResolver;
        private readonly IRightZone rightZone;
        private readonly IDragElementFactory dragElementFactory;
        private readonly IScrollElementDataRepository scrollElementDataRepository;

        public TowerService(
            TowerState towerState,
            ITowerPlacementRuleValidator ruleValidator,
            ITowerPositionResolver towerPositionResolver,
            IRightZone rightZone,
            IDragElementFactory dragElementFactory,
            IScrollElementDataRepository scrollElementDataRepository)
        {
            this.towerState = towerState;
            this.ruleValidator = ruleValidator;
            this.towerPositionResolver = towerPositionResolver;
            this.rightZone = rightZone;
            this.dragElementFactory = dragElementFactory;
            this.scrollElementDataRepository = scrollElementDataRepository;
        }

        public TowerPlacementResult TryPlace(IDragElement dragElement, Vector2 pointerScreenPosition)
        {
            if (dragElement == null || dragElement.Root == null)
            {
                TowerPlacementResult invalidResult = new TowerPlacementResult(
                    false,
                    TowerPlacementFailureReasonType.InvalidElement,
                    Vector2.zero,
                    false);

                return invalidResult;
            }

            Vector2 elementSize = ResolveElementSize(dragElement.Root);
            Vector2 dragPosition = dragElement.Root.position;
            Vector2 candidatePosition = ResolveCandidatePosition(dragElement, pointerScreenPosition, elementSize);
            TowerPlacementContext context = new TowerPlacementContext(
                dragElement,
                pointerScreenPosition,
                dragPosition,
                candidatePosition,
                elementSize);
            TowerPlacementFailureReasonType failureReason = ruleValidator.Validate(context, towerState);

            if (failureReason != TowerPlacementFailureReasonType.None)
            {
                TowerPlacementResult failureResult = new TowerPlacementResult(
                    false,
                    failureReason,
                    candidatePosition,
                    false);
                
                return failureResult;
            }

            bool hasReachedHeightLimit = IsPlacementOverHeight(candidatePosition, elementSize);

            if (hasReachedHeightLimit)
            {
                towerState.MarkHeightLimitReached();
            }

            if (rightZone != null && rightZone.Root != null)
            {
                dragElement.Root.SetParent(rightZone.Root, true);
            }

            TowerBlockEntry blockEntry = new TowerBlockEntry(
                dragElement,
                ResolveElementIdentifier(dragElement),
                candidatePosition,
                elementSize);
            towerState.AddBlock(blockEntry);

            TowerPlacementResult successResult = new TowerPlacementResult(
                true,
                TowerPlacementFailureReasonType.None,
                candidatePosition,
                hasReachedHeightLimit);

            return successResult;
        }

        public List<TowerBlockEntry> GetBlocks()
        {
            return towerState.Blocks;
        }

        public void Clear()
        {
            List<TowerBlockEntry> blocks = towerState.Blocks;

            for (int i = 0; i < blocks.Count; i++)
            {
                TowerBlockEntry block = blocks[i];

                if (block == null || block.DragElement == null || block.DragElement.Root == null)
                {
                    continue;
                }

                PooledObject.Despawn(block.DragElement.Root.gameObject);
            }

            towerState.Clear();
        }

        public void Restore(TowerSnapshot snapshot)
        {
            Clear();

            if (snapshot == null || snapshot.Blocks == null || snapshot.Blocks.Count == 0)
            {
                return;
            }

            for (int i = 0; i < snapshot.Blocks.Count; i++)
            {
                TowerSnapshotBlock snapshotBlock = snapshot.Blocks[i];

                if (snapshotBlock == null)
                {
                    continue;
                }

                ScrollElementData data = scrollElementDataRepository.FindById(snapshotBlock.ElementId);

                if (data == null)
                {
                    continue;
                }

                IDragElement dragElement = dragElementFactory.Create(data, snapshotBlock.Position);

                if (rightZone != null && rightZone.Root != null)
                {
                    dragElement.Root.SetParent(rightZone.Root, true);
                }

                dragElement.Root.position = snapshotBlock.Position;
                Vector2 elementSize = ResolveElementSize(dragElement.Root);
                TowerBlockEntry blockEntry = new TowerBlockEntry(
                    dragElement,
                    snapshotBlock.ElementId,
                    snapshotBlock.Position,
                    elementSize);
                towerState.AddBlock(blockEntry);
            }

            towerState.SetHeightLimitReached(snapshot.IsHeightLimitReached);
        }

        public TowerSnapshot GetSnapshot()
        {
            List<TowerSnapshotBlock> snapshotBlocks = new List<TowerSnapshotBlock>();
            List<TowerBlockEntry> blocks = towerState.Blocks;

            for (int i = 0; i < blocks.Count; i++)
            {
                TowerBlockEntry block = blocks[i];

                if (block == null)
                {
                    continue;
                }

                TowerSnapshotBlock snapshotBlock = new TowerSnapshotBlock(block.ElementId, block.Position);
                snapshotBlocks.Add(snapshotBlock);
            }

            TowerSnapshot snapshot = new TowerSnapshot(snapshotBlocks, towerState.IsHeightLimitReached);

            return snapshot;
        }

        public TowerRemovalResult TryRemove(IDragElement dragElement)
        {
            if (dragElement == null)
            {
                TowerRemovalResult invalidResult = new TowerRemovalResult(
                    false,
                    null,
                    new List<TowerShiftEntry>());

                return invalidResult;
            }

            List<TowerBlockEntry> blocks = towerState.Blocks;
            int removedIndex = FindBlockIndex(blocks, dragElement);

            if (removedIndex < 0)
            {
                TowerRemovalResult missedResult = new TowerRemovalResult(
                    false,
                    null,
                    new List<TowerShiftEntry>());

                return missedResult;
            }

            TowerBlockEntry removedBlock = blocks[removedIndex];
            blocks.RemoveAt(removedIndex);
            List<TowerShiftEntry> shiftedBlocks = new List<TowerShiftEntry>();
            float shiftDistance = removedBlock.Size.y;

            for (int i = removedIndex; i < blocks.Count; i++)
            {
                TowerBlockEntry block = blocks[i];
                Vector2 targetPosition = new Vector2(block.Position.x, block.Position.y - shiftDistance);
                block.Position = targetPosition;
                TowerShiftEntry shiftEntry = new TowerShiftEntry(block.DragElement, targetPosition);
                shiftedBlocks.Add(shiftEntry);
            }

            towerState.SetHeightLimitReached(IsTowerOverHeight());

            TowerRemovalResult result = new TowerRemovalResult(
                true,
                removedBlock,
                shiftedBlocks);

            return result;
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
            return towerPositionResolver.Resolve(towerState, dragElement.Root.position, pointerScreenPosition, elementSize);
        }

        private bool IsPlacementOverHeight(Vector2 candidatePosition, Vector2 elementSize)
        {
            float topEdge = candidatePosition.y + elementSize.y * 0.5f;

            return topEdge > UnityEngine.Screen.height;
        }

        private int FindBlockIndex(List<TowerBlockEntry> blocks, IDragElement dragElement)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                TowerBlockEntry block = blocks[i];

                if (block.DragElement == dragElement)
                {
                    return i;
                }
            }

            return -1;
        }

        private bool IsTowerOverHeight()
        {
            TowerBlockEntry topBlock = towerState.GetTopBlock();

            if (topBlock == null)
            {
                return false;
            }

            return IsPlacementOverHeight(topBlock.Position, topBlock.Size);
        }
    }
}

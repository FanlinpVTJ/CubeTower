using System;
using System.Collections.Generic;
using CubeGame.Input;
using CubeGame.Hole;
using CubeGame.Scroll;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace CubeGame.Tower
{
    public sealed class TowerDragDropHandler : IInitializable, IDisposable
    {
        private readonly ISubscriber<DragSessionEndedMessage> dragSessionEndedSubscriber;
        private readonly ITowerService towerService;
        private readonly IHoleService holeService;
        private readonly IPublisher<TowerActionMessage> towerActionPublisher;
        private readonly IPublisher<DragSessionCancelledMessage> dragSessionCancelledPublisher;
        private readonly IPublisher<DragSessionPlacedMessage> dragSessionPlacedPublisher;
        private readonly IPublisher<DragSessionDisposalStartedMessage> dragSessionDisposalStartedPublisher;
        private readonly IPublisher<TowerBlockShiftedMessage> towerBlockShiftedPublisher;
        private readonly TowerConfig towerConfig;
        private readonly ScrollRuntimeConfig runtimeConfig;
        private readonly HoleConfig holeConfig;

        private IDisposable dragSessionEndedSubscription;

        public TowerDragDropHandler(
            ISubscriber<DragSessionEndedMessage> dragSessionEndedSubscriber,
            ITowerService towerService,
            IHoleService holeService,
            IPublisher<TowerActionMessage> towerActionPublisher,
            IPublisher<DragSessionCancelledMessage> dragSessionCancelledPublisher,
            IPublisher<DragSessionPlacedMessage> dragSessionPlacedPublisher,
            IPublisher<DragSessionDisposalStartedMessage> dragSessionDisposalStartedPublisher,
            IPublisher<TowerBlockShiftedMessage> towerBlockShiftedPublisher,
            TowerConfig towerConfig,
            ScrollRuntimeConfig runtimeConfig,
            HoleConfig holeConfig)
        {
            this.dragSessionEndedSubscriber = dragSessionEndedSubscriber;
            this.towerService = towerService;
            this.holeService = holeService;
            this.towerActionPublisher = towerActionPublisher;
            this.dragSessionCancelledPublisher = dragSessionCancelledPublisher;
            this.dragSessionPlacedPublisher = dragSessionPlacedPublisher;
            this.dragSessionDisposalStartedPublisher = dragSessionDisposalStartedPublisher;
            this.towerBlockShiftedPublisher = towerBlockShiftedPublisher;
            this.towerConfig = towerConfig;
            this.runtimeConfig = runtimeConfig;
            this.holeConfig = holeConfig;
        }

        public void Initialize()
        {
            dragSessionEndedSubscription = dragSessionEndedSubscriber.Subscribe(OnDragSessionEnded);
        }

        public void Dispose()
        {
            dragSessionEndedSubscription?.Dispose();
            dragSessionEndedSubscription = null;
        }

        private void OnDragSessionEnded(DragSessionEndedMessage message)
        {
            if (message.DragElement == null)
            {
                return;
            }

            HoleDisposalResult disposalResult = holeService.TryDispose(message.DragElement, message.PointerScreenPosition);

            if (disposalResult.IsSuccess)
            {
                TowerRemovalResult removalResult = towerService.TryRemove(message.DragElement);
                PublishShiftMessages(removalResult.ShiftedBlocks);
                PublishDisposeMessage(message, disposalResult);
                PublishAction(TowerActionType.BlockRemoved, ResolveRemovedText());

                return;
            }

            if (message.ScrollElement == null)
            {
                PublishAction(TowerActionType.BlockReturned, ResolveReturnedText());

                DragSessionCancelledMessage towerCancelledMessage = new DragSessionCancelledMessage(
                    null,
                    message.DragElement,
                    message.ReturnPosition,
                    ResolveCancelAnimationDuration(),
                    false);
                dragSessionCancelledPublisher.Publish(towerCancelledMessage);

                return;
            }

            TowerPlacementResult placementResult = towerService.TryPlace(message.DragElement, message.PointerScreenPosition);

            if (placementResult.IsSuccess)
            {
                DragSessionPlacedMessage placedMessage = new DragSessionPlacedMessage(message.ScrollElement, message.DragElement);
                dragSessionPlacedPublisher.Publish(placedMessage);
                PublishAction(TowerActionType.BlockPlaced, ResolvePlacedText());

                if (placementResult.HasReachedHeightLimit)
                {
                    PublishAction(TowerActionType.HeightLimitReached, ResolveHeightLimitText());
                }

                return;
            }

            if (placementResult.FailureReason == TowerPlacementFailureReasonType.HeightLimitReached)
            {
                PublishAction(TowerActionType.HeightLimitReached, ResolveHeightLimitText());
            }
            else
            {
                PublishAction(TowerActionType.BlockMissed, ResolveMissedText());
            }

            Vector2 returnPosition = ResolveReturnPosition(message);
            DragSessionCancelledMessage cancelledMessage = new DragSessionCancelledMessage(
                message.ScrollElement,
                message.DragElement,
                returnPosition,
                ResolveCancelAnimationDuration(),
                true);
            dragSessionCancelledPublisher.Publish(cancelledMessage);
        }

        private void PublishDisposeMessage(DragSessionEndedMessage message, HoleDisposalResult disposalResult)
        {
            DragSessionDisposalStartedMessage disposalMessage = new DragSessionDisposalStartedMessage(
                message.ScrollElement,
                message.DragElement,
                disposalResult.TargetPosition,
                ResolveDisposeAnimationDuration());
            dragSessionDisposalStartedPublisher.Publish(disposalMessage);
        }

        private void PublishShiftMessages(List<TowerShiftEntry> shiftedBlocks)
        {
            if (shiftedBlocks == null || shiftedBlocks.Count == 0)
            {
                return;
            }

            float animationDuration = ResolveTowerShiftAnimationDuration();

            for (int i = 0; i < shiftedBlocks.Count; i++)
            {
                TowerShiftEntry shiftedBlock = shiftedBlocks[i];

                if (shiftedBlock == null || shiftedBlock.DragElement == null)
                {
                    continue;
                }

                TowerBlockShiftedMessage shiftedMessage = new TowerBlockShiftedMessage(
                    shiftedBlock.DragElement,
                    shiftedBlock.TargetPosition,
                    animationDuration);
                towerBlockShiftedPublisher.Publish(shiftedMessage);
            }
        }

        private void PublishAction(TowerActionType actionType, string text)
        {
            TowerActionMessage actionMessage = new TowerActionMessage(actionType, text);
            towerActionPublisher.Publish(actionMessage);
        }

        private string ResolvePlacedText()
        {
            if (towerConfig == null)
            {
                return "Block placed";
            }

            return towerConfig.BlockPlacedText;
        }

        private string ResolveRemovedText()
        {
            if (towerConfig == null)
            {
                return "Block removed";
            }

            return towerConfig.BlockRemovedText;
        }

        private string ResolveReturnedText()
        {
            if (towerConfig == null)
            {
                return "Block returned";
            }

            return towerConfig.BlockReturnedText;
        }

        private string ResolveMissedText()
        {
            if (towerConfig == null)
            {
                return "Missed";
            }

            return towerConfig.BlockMissedText;
        }

        private string ResolveHeightLimitText()
        {
            if (towerConfig == null)
            {
                return "Height limit reached";
            }

            return towerConfig.HeightLimitReachedText;
        }

        private Vector2 ResolveReturnPosition(DragSessionEndedMessage message)
        {
            return message.ReturnPosition;
        }

        private float ResolveCancelAnimationDuration()
        {
            if (runtimeConfig == null)
            {
                return 0.18f;
            }

            return runtimeConfig.DragCancelAnimationDuration;
        }

        private float ResolveDisposeAnimationDuration()
        {
            if (holeConfig == null)
            {
                return 0.22f;
            }

            return holeConfig.DisposeAnimationDuration;
        }

        private float ResolveTowerShiftAnimationDuration()
        {
            if (holeConfig == null)
            {
                return 0.2f;
            }

            return holeConfig.TowerShiftAnimationDuration;
        }
    }
}

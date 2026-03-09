using System;
using CubeGame.Input;
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
        private readonly IPublisher<TowerActionMessage> towerActionPublisher;
        private readonly IPublisher<DragSessionCancelledMessage> dragSessionCancelledPublisher;
        private readonly IPublisher<DragSessionPlacedMessage> dragSessionPlacedPublisher;
        private readonly TowerConfig towerConfig;
        private readonly ScrollRuntimeConfig runtimeConfig;

        private IDisposable dragSessionEndedSubscription;

        public TowerDragDropHandler(
            ISubscriber<DragSessionEndedMessage> dragSessionEndedSubscriber,
            ITowerService towerService,
            IPublisher<TowerActionMessage> towerActionPublisher,
            IPublisher<DragSessionCancelledMessage> dragSessionCancelledPublisher,
            IPublisher<DragSessionPlacedMessage> dragSessionPlacedPublisher,
            TowerConfig towerConfig,
            ScrollRuntimeConfig runtimeConfig)
        {
            this.dragSessionEndedSubscriber = dragSessionEndedSubscriber;
            this.towerService = towerService;
            this.towerActionPublisher = towerActionPublisher;
            this.dragSessionCancelledPublisher = dragSessionCancelledPublisher;
            this.dragSessionPlacedPublisher = dragSessionPlacedPublisher;
            this.towerConfig = towerConfig;
            this.runtimeConfig = runtimeConfig;
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
                ResolveCancelAnimationDuration());
            dragSessionCancelledPublisher.Publish(cancelledMessage);
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
            if (message.ScrollElement == null || message.ScrollElement.Root == null)
            {
                return message.DragElement.Root.position;
            }

            return message.ScrollElement.Root.position;
        }

        private float ResolveCancelAnimationDuration()
        {
            if (runtimeConfig == null)
            {
                return 0.18f;
            }

            return runtimeConfig.DragCancelAnimationDuration;
        }
    }
}

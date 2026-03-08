using System;
using CubeGame.Input;
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
        private readonly TowerConfig towerConfig;

        private IDisposable dragSessionEndedSubscription;

        public TowerDragDropHandler(
            ISubscriber<DragSessionEndedMessage> dragSessionEndedSubscriber,
            ITowerService towerService,
            IPublisher<TowerActionMessage> towerActionPublisher,
            TowerConfig towerConfig)
        {
            this.dragSessionEndedSubscriber = dragSessionEndedSubscriber;
            this.towerService = towerService;
            this.towerActionPublisher = towerActionPublisher;
            this.towerConfig = towerConfig;
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
                PublishAction(TowerActionType.BlockPlaced, ResolvePlacedText());
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

            UnityEngine.Object.Destroy(message.DragElement.Root.gameObject);
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
    }
}

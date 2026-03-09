using System;
using CubeGame.Tower;
using MessagePipe;
using Zenject;

namespace CubeGame.Feedback
{
    public sealed class FeedbackController : IInitializable, IDisposable
    {
        private readonly ISubscriber<TowerActionMessage> towerActionSubscriber;
        private readonly IFeedbackFactory feedbackFactory;

        private IDisposable towerActionSubscription;

        public FeedbackController(
            ISubscriber<TowerActionMessage> towerActionSubscriber,
            IFeedbackFactory feedbackFactory)
        {
            this.towerActionSubscriber = towerActionSubscriber;
            this.feedbackFactory = feedbackFactory;
        }

        public void Initialize()
        {
            towerActionSubscription = towerActionSubscriber.Subscribe(OnTowerAction);
        }

        public void Dispose()
        {
            towerActionSubscription?.Dispose();
            towerActionSubscription = null;
        }

        private void OnTowerAction(TowerActionMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.Text))
            {
                return;
            }

            feedbackFactory.Create(message.Text);
        }
    }
}

using System;
using CubeGame.Localization;
using CubeGame.Tower;
using MessagePipe;
using Zenject;

namespace CubeGame.Feedback
{
    public sealed class FeedbackController : IInitializable, IDisposable
    {
        private readonly ISubscriber<TowerActionMessage> towerActionSubscriber;
        private readonly IFeedbackFactory feedbackFactory;
        private readonly ILocalizationManager localizationManager;

        private IDisposable towerActionSubscription;

        public FeedbackController(
            ISubscriber<TowerActionMessage> towerActionSubscriber,
            IFeedbackFactory feedbackFactory,
            ILocalizationManager localizationManager)
        {
            this.towerActionSubscriber = towerActionSubscriber;
            this.feedbackFactory = feedbackFactory;
            this.localizationManager = localizationManager;
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

            string localizedText = ResolveLocalizedText(message.Text);
            feedbackFactory.Create(localizedText);
        }

        private string ResolveLocalizedText(string key)
        {
            if (localizationManager == null)
            {
                return key;
            }

            return localizationManager.GetString(key);
        }
    }
}

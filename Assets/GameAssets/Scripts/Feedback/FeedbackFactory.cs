using CubeGame.ObjectPoolManager;
using UnityEngine;
using Zenject;

namespace CubeGame.Feedback
{
    public sealed class FeedbackFactory : IFeedbackFactory
    {
        private readonly PoolManager poolManager;
        private readonly PoolGroup feedbackItemPoolGroup;
        private readonly RectTransform feedbackItemsRoot;
        private readonly FeedbackConfig feedbackConfig;

        public FeedbackFactory(
            PoolManager poolManager,
            PoolGroup feedbackItemPoolGroup,
            RectTransform feedbackItemsRoot,
            FeedbackConfig feedbackConfig)
        {
            this.poolManager = poolManager;
            this.feedbackItemPoolGroup = feedbackItemPoolGroup;
            this.feedbackItemsRoot = feedbackItemsRoot;
            this.feedbackConfig = feedbackConfig;

            if (this.feedbackItemPoolGroup == null)
            {
                throw new ZenjectException("[FeedbackFactory] Feedback item pool group is not assigned.");
            }

            if (this.feedbackItemsRoot == null)
            {
                throw new ZenjectException("[FeedbackFactory] Feedback items root is not assigned.");
            }
        }

        public void Create(string text)
        {
            PooledObject pooledObject = poolManager.InstantiateFromGroup(feedbackItemPoolGroup, feedbackItemsRoot);
            FeedbackItemView feedbackItemView = pooledObject.GetComponent<FeedbackItemView>();

            if (feedbackItemView == null)
            {
                throw new ZenjectException("[FeedbackFactory] Created object has no FeedbackItemView.");
            }

            feedbackItemView.Show(text, feedbackConfig);
            feedbackItemView.Root.SetAsLastSibling();
        }
    }
}

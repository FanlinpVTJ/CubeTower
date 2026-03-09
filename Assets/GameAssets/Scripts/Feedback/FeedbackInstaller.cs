using CubeGame.ObjectPoolManager;
using UnityEngine;
using Zenject;

namespace CubeGame.Feedback
{
    public sealed class FeedbackInstaller : MonoInstaller<FeedbackInstaller>
    {
        [SerializeField] private FeedbackConfig feedbackConfig;
        [SerializeField] private PoolGroup feedbackItemPoolGroup;
        [SerializeField] private RectTransform feedbackItemsRoot;

        public override void InstallBindings()
        {
            if (feedbackConfig == null)
            {
                throw new ZenjectException("[FeedbackInstaller] Feedback config is not assigned.");
            }

            if (feedbackItemPoolGroup == null)
            {
                throw new ZenjectException("[FeedbackInstaller] Feedback item pool group is not assigned.");
            }

            if (feedbackItemsRoot == null)
            {
                throw new ZenjectException("[FeedbackInstaller] Feedback items root is not assigned.");
            }

            Container.Bind<FeedbackConfig>().FromInstance(feedbackConfig).AsSingle();
            Container.Bind<IFeedbackFactory>().To<FeedbackFactory>().AsSingle()
                .WithArguments(feedbackItemPoolGroup, feedbackItemsRoot, feedbackConfig);
            Container.BindInterfacesAndSelfTo<FeedbackController>().AsSingle();
        }
    }
}

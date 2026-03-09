using MessagePipe;
using UnityEngine;
using Zenject;
using CubeGame.Drag;
using CubeGame.Input;
using CubeGame.Tower;

namespace CubeGame.Scroll
{
    public sealed class ScrollRuntimeInstaller : MonoInstaller<ScrollRuntimeInstaller>
    {
        [SerializeField] private ScrollFeedConfig feedConfig;
        [SerializeField] private ScrollRuntimeConfig runtimeConfig;

        public override void InstallBindings()
        {
            if (feedConfig == null)
            {
                throw new ZenjectException("[ScrollRuntimeInstaller] Feed config is not assigned.");
            }

            if (runtimeConfig == null)
            {
                throw new ZenjectException("[ScrollRuntimeInstaller] Runtime config is not assigned.");
            }

            MessagePipeOptions options = Container.BindMessagePipe();
            Container.BindMessageBroker<ScrollElementPressedMessage>(options);
            Container.BindMessageBroker<DragElementPressedMessage>(options);
            Container.BindMessageBroker<ScrollElementSpawnedMessage>(options);
            Container.BindMessageBroker<ScrollElementRemovedMessage>(options);
            Container.BindMessageBroker<DragSessionStartedMessage>(options);
            Container.BindMessageBroker<DragSessionMovedMessage>(options);
            Container.BindMessageBroker<DragSessionEndedMessage>(options);
            Container.BindMessageBroker<DragSessionCancelledMessage>(options);
            Container.BindMessageBroker<DragSessionPlacedMessage>(options);
            Container.BindMessageBroker<DragSessionReturnedMessage>(options);
            Container.BindMessageBroker<DragSessionDisposalStartedMessage>(options);
            Container.BindMessageBroker<DragSessionDisposedMessage>(options);
            Container.BindMessageBroker<TowerActionMessage>(options);
            Container.BindMessageBroker<TowerBlockShiftedMessage>(options);

            Container.Bind<ScrollRuntimeConfig>().FromInstance(runtimeConfig).AsSingle();
            Container.Bind<ScrollFeedConfig>().FromInstance(feedConfig).AsSingle();
            Container.Bind<IScrollElementDataRepository>().To<ScrollElementDataRepository>().AsSingle()
                .WithArguments(feedConfig);

            Container.BindInterfacesAndSelfTo<ScrollElementRegistry>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScrollRectMovementController>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScrollDragSessionController>().AsSingle();
            Container.Bind<IScrollInputHandler>().To<ScrollInputHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScrollElementSpawner>().AsSingle().NonLazy();
        }
    }
}

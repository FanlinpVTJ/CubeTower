using MessagePipe;
using UnityEngine;
using Zenject;

namespace CubeGame.Scroll
{
    public sealed class ScrollRuntimeInstaller : MonoInstaller<ScrollRuntimeInstaller>
    {
        [SerializeField] private ScrollFeedConfig feedConfig;

        public override void InstallBindings()
        {
            var options = Container.BindMessagePipe();
            Container.BindMessageBroker<ScrollElementPressedMessage>(options);
            Container.BindMessageBroker<ScrollElementSpawnedMessage>(options);
            Container.BindMessageBroker<ScrollElementRemovedMessage>(options);

            Container.Bind<IScrollElementDataRepository>().To<ScrollElementDataRepository>().AsSingle()
                .WithArguments(feedConfig);

            Container.BindInterfacesAndSelfTo<ScrollElementRegistry>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScrollInputManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScrollElementSpawner>().AsSingle().NonLazy();
        }
    }
}

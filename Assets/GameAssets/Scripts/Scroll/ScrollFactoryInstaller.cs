using UnityEngine;
using Zenject;

namespace CubeGame.Scroll
{
    public sealed class ScrollFactoryInstaller : MonoInstaller<ScrollFactoryInstaller>
    {
        [SerializeField] private ScrollElementBase scrollElementPrefab;

        public override void InstallBindings()
        {
            Container.Bind<IScrollElementFactory>().To<ScrollElementFactory>().AsSingle()
                .WithArguments(scrollElementPrefab);
        }
    }
}

using UnityEngine;
using Zenject;
using CubeGame.ObjectPoolManager;

namespace CubeGame.Drag
{
    public sealed class DragFactoryInstaller : MonoInstaller<DragFactoryInstaller>
    {
        [SerializeField] private PoolGroup dragElementPoolGroup;
        [SerializeField] private RectTransform dragElementsRoot;

        public override void InstallBindings()
        {
            Container.Bind<IDragElementFactory>().To<DragElementFactory>().AsSingle()
                .WithArguments(dragElementPoolGroup, dragElementsRoot);
        }
    }
}

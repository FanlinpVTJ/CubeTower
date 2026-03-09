using UnityEngine;
using Zenject;

namespace CubeGame.Drag
{
    public sealed class DragFactoryInstaller : MonoInstaller<DragFactoryInstaller>
    {
        [SerializeField] private DragElementBase dragElementPrefab;
        [SerializeField] private RectTransform dragElementsRoot;

        public override void InstallBindings()
        {
            Container.Bind<IDragElementFactory>().To<DragElementFactory>().AsSingle()
                .WithArguments(dragElementPrefab, dragElementsRoot);
        }
    }
}

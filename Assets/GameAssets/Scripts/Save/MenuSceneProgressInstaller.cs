using UnityEngine;
using Zenject;

namespace CubeGame.Save
{
    public sealed class MenuSceneProgressInstaller : MonoInstaller<MenuSceneProgressInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IMenuSceneProgressHandler>().To<MenuSceneProgressHandler>().AsSingle();
        }
    }
}

using UnityEngine;
using Zenject;

namespace CubeGame.Save
{
    public sealed class GameSceneProgressInstaller : MonoInstaller<GameSceneProgressInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameSceneProgressHandler>().AsSingle();
        }
    }
}

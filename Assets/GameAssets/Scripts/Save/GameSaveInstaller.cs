using UnityEngine;
using Zenject;

namespace CubeGame.Save
{
    public sealed class GameSaveInstaller : MonoInstaller<GameSaveInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameSaver>().AsSingle();
        }
    }
}

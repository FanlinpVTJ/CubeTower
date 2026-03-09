using UnityEngine;
using Zenject;

namespace CubeGame.ObjectPoolManager
{
    public class PoolManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PoolManager>().AsSingle();
        }
    } 
}

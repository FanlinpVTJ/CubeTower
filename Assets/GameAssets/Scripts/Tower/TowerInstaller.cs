using UnityEngine;
using Zenject;

namespace CubeGame.Tower
{
    public sealed class TowerInstaller : MonoInstaller<TowerInstaller>
    {
        [SerializeField] private TowerConfig towerConfig;

        public override void InstallBindings()
        {
            if (towerConfig == null)
            {
                throw new ZenjectException("[TowerInstaller] Tower config is not assigned.");
            }

            Container.Bind<TowerConfig>().FromInstance(towerConfig).AsSingle();
            Container.Bind<TowerState>().AsSingle();
            Container.Bind<ITowerPositionResolver>().To<TowerPositionResolver>().AsSingle();
            Container.Bind<ITowerPlacementRuleValidator>().To<TowerPlacementRuleValidator>().AsSingle();
            Container.Bind<ITowerPlacementRule>().To<FirstBlockInsideRightZoneRule>().AsSingle();
            Container.Bind<ITowerPlacementRule>().To<StackOnTopHitRule>().AsSingle();
            Container.Bind<ITowerPlacementRule>().To<HeightLimitRule>().AsSingle();
            Container.Bind<ITowerService>().To<TowerService>().AsSingle();
            Container.BindInterfacesAndSelfTo<TowerDragDropHandler>().AsSingle();
        }
    }
}

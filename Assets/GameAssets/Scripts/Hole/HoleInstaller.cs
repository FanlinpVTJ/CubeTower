using UnityEngine;
using Zenject;

namespace CubeGame.Hole
{
    public sealed class HoleInstaller : MonoInstaller<HoleInstaller>
    {
        [SerializeField] private HoleConfig holeConfig;

        public override void InstallBindings()
        {
            if (holeConfig == null)
            {
                throw new ZenjectException("[HoleInstaller] Hole config is not assigned.");
            }

            Container.Bind<HoleConfig>().FromInstance(holeConfig).AsSingle();
            Container.Bind<IHoleDisposalRuleValidator>().To<HoleDisposalRuleValidator>().AsSingle();
            Container.Bind<IHoleDisposalRule>().To<OvalHoleHitRule>().AsSingle();
            Container.Bind<IHoleService>().To<HoleService>().AsSingle();
        }
    }
}

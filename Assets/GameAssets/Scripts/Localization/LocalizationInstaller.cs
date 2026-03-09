using UnityEngine;
using Zenject;

namespace CubeGame.Localization
{
    public sealed class LocalizationInstaller : MonoInstaller<LocalizationInstaller>
    {
        [SerializeField] private LocalizationConfig localizationConfig;

        public override void InstallBindings()
        {
            if (localizationConfig == null)
            {
                throw new ZenjectException("[LocalizationInstaller] Localization config is not assigned.");
            }

            Container.Bind<LocalizationConfig>().FromInstance(localizationConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<LocalizationManager>().AsSingle();
        }
    }
}

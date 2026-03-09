using UnityEngine;
using Zenject;

namespace CubeGame.SceneLoading
{
    public sealed class SceneLoadingInstaller : MonoInstaller<SceneLoadingInstaller>
    {
        [SerializeField] private SceneLoadingConfig sceneLoadingConfig;
        [SerializeField] private LoadingScreenView loadingScreenPrefab;

        public override void InstallBindings()
        {
            if (sceneLoadingConfig == null)
            {
                throw new ZenjectException("[SceneLoadingInstaller] Scene loading config is not assigned.");
            }

            if (loadingScreenPrefab == null)
            {
                throw new ZenjectException("[SceneLoadingInstaller] Loading screen prefab is not assigned.");
            }

            LoadingScreenView loadingScreenView = ResolveLoadingScreenView();
            Container.Bind<SceneLoadingConfig>().FromInstance(sceneLoadingConfig).AsSingle();
            Container.Bind<LoadingScreenView>().FromInstance(loadingScreenView).AsSingle();
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
        }

        private LoadingScreenView ResolveLoadingScreenView()
        {
            LoadingScreenView existingLoadingScreenView = Object.FindFirstObjectByType<LoadingScreenView>(FindObjectsInactive.Include);

            if (existingLoadingScreenView != null)
            {
                return existingLoadingScreenView;
            }

            LoadingScreenView createdLoadingScreenView = Container.InstantiatePrefabForComponent<LoadingScreenView>(loadingScreenPrefab);

            return createdLoadingScreenView;
        }
    }
}

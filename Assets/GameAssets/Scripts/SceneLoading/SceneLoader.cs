using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CubeGame.SceneLoading
{
    public sealed class SceneLoader : ISceneLoader
    {
        private readonly SceneLoadingConfig sceneLoadingConfig;
        private readonly LoadingScreenView loadingScreenView;

        public SceneLoader(SceneLoadingConfig sceneLoadingConfig, LoadingScreenView loadingScreenView)
        {
            this.sceneLoadingConfig = sceneLoadingConfig;
            this.loadingScreenView = loadingScreenView;
        }

        public bool IsLoading { get; private set; }

        public void LoadScene(string sceneName)
        {
            if (IsLoading || string.IsNullOrEmpty(sceneName))
            {
                return;
            }

            LoadSceneInternalAsync(sceneName).Forget();
        }

        private async UniTaskVoid LoadSceneInternalAsync(string sceneName)
        {
            IsLoading = true;

            if (loadingScreenView != null)
            {
                await loadingScreenView.ShowAsync(GetFadeDuration());
            }

            float startTime = Time.realtimeSinceStartup;
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);

            if (loadOperation != null)
            {
                await UniTask.WaitUntil(IsSceneLoaded, cancellationToken: default);
            }

            float elapsedTime = Time.realtimeSinceStartup - startTime;
            float remainingTime = GetMinimumLoadingDuration() - elapsedTime;

            if (remainingTime > 0f)
            {
                int remainingMilliseconds = Mathf.CeilToInt(remainingTime * 1000f);
                await UniTask.Delay(remainingMilliseconds, DelayType.UnscaledDeltaTime);
            }

            if (loadingScreenView != null)
            {
                await loadingScreenView.HideAsync(GetFadeDuration());
            }

            IsLoading = false;

            bool IsSceneLoaded()
            {
                return loadOperation == null || loadOperation.isDone;
            }
        }

        private float GetMinimumLoadingDuration()
        {
            if (sceneLoadingConfig == null)
            {
                return 1f;
            }

            return Mathf.Max(0f, sceneLoadingConfig.MinimumLoadingDuration);
        }

        private float GetFadeDuration()
        {
            if (sceneLoadingConfig == null)
            {
                return 0.2f;
            }

            return Mathf.Max(0f, sceneLoadingConfig.FadeDuration);
        }
    }
}

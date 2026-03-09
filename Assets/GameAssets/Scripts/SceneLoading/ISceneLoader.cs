namespace CubeGame.SceneLoading
{
    public interface ISceneLoader
    {
        bool IsLoading { get; }
        void LoadScene(string sceneName);
    }
}

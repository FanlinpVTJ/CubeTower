namespace CubeGame.Save
{
    public interface IMenuSceneProgressHandler
    {
        bool HasSave { get; }
        void StartNewGame();
    }
}

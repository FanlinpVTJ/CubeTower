using Zenject;

namespace CubeGame.Save
{
    public interface IGameSaver : IInitializable
    {
        bool HasSave { get; }
        void SaveProgress();
        bool TryRestoreProgress();
        void ClearProgress();
    }
}

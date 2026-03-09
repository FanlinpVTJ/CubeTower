using System;
using Zenject;

namespace CubeGame.Save
{
    public interface IGameSaver : IInitializable
    {
        event Action OnInitialized;
        bool IsInitialized { get; }
        bool HasSave { get; }
        void SaveProgress(GameSaveData saveData);
        bool TryLoadProgress(out GameSaveData saveData);
        void ClearProgress();
    }
}

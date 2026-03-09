using CubeGame.Tower;
using UnityEngine;
using Zenject;

namespace CubeGame.Save
{
    public sealed class GameSaver : IGameSaver
    {
        private const string SAVE_KEY = "cube_game_progress";

        [Inject(Optional = true)] private ITowerService towerService;

        public bool HasSave { get; private set; }

        public void Initialize()
        {
            HasSave = PlayerPrefs.HasKey(SAVE_KEY);
        }

        public void SaveProgress()
        {
            if (towerService == null)
            {
                HasSave = PlayerPrefs.HasKey(SAVE_KEY);

                return;
            }

            TowerSnapshot towerSnapshot = towerService.GetSnapshot();
            GameSaveData saveData = new GameSaveData(towerSnapshot);
            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(SAVE_KEY, json);
            PlayerPrefs.Save();
            HasSave = true;
        }

        public bool TryRestoreProgress()
        {
            if (!PlayerPrefs.HasKey(SAVE_KEY))
            {
                HasSave = false;

                return false;
            }

            if (towerService == null)
            {
                HasSave = true;

                return false;
            }

            string json = PlayerPrefs.GetString(SAVE_KEY, string.Empty);

            if (string.IsNullOrEmpty(json))
            {
                HasSave = false;

                return false;
            }

            GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);

            if (saveData == null)
            {
                HasSave = false;

                return false;
            }

            towerService.Restore(saveData.TowerSnapshot);
            HasSave = true;

            return true;
        }

        public void ClearProgress()
        {
            if (towerService != null)
            {
                towerService.Clear();
            }

            PlayerPrefs.DeleteKey(SAVE_KEY);
            PlayerPrefs.Save();
            HasSave = false;
        }
    }
}

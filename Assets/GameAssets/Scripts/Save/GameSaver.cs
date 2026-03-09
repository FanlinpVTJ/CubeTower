using UnityEngine;

namespace CubeGame.Save
{
    public sealed class GameSaver : IGameSaver
    {
        private const string SAVE_KEY = "cube_game_progress";

        public bool HasSave { get; private set; }

        public void Initialize()
        {
            HasSave = PlayerPrefs.HasKey(SAVE_KEY);
        }

        public void SaveProgress(GameSaveData saveData)
        {
            if (saveData == null)
            {
                HasSave = PlayerPrefs.HasKey(SAVE_KEY);

                return;
            }

            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(SAVE_KEY, json);
            PlayerPrefs.Save();
            HasSave = true;
        }

        public bool TryLoadProgress(out GameSaveData saveData)
        {
            saveData = null;

            if (!PlayerPrefs.HasKey(SAVE_KEY))
            {
                HasSave = false;

                return false;
            }

            string json = PlayerPrefs.GetString(SAVE_KEY, string.Empty);

            if (string.IsNullOrEmpty(json))
            {
                HasSave = false;

                return false;
            }

            saveData = JsonUtility.FromJson<GameSaveData>(json);

            if (saveData == null)
            {
                HasSave = false;

                return false;
            }
            
            HasSave = true;

            return true;
        }

        public void ClearProgress()
        {
            PlayerPrefs.DeleteKey(SAVE_KEY);
            PlayerPrefs.Save();
            HasSave = false;
        }
    }
}

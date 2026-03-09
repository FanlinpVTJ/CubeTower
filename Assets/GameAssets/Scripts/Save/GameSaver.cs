using System;
using UnityEngine;

namespace CubeGame.Save
{
    public sealed class GameSaver : IGameSaver
    {
        private const string SAVE_KEY = "cube_game_progress";

        public event Action OnInitialized;

        public bool IsInitialized { get; private set; }
        public bool HasSave => PlayerPrefs.HasKey(SAVE_KEY);

        public void Initialize()
        {
            IsInitialized = true;
            OnInitialized?.Invoke();
        }

        public void SaveProgress(GameSaveData saveData)
        {
            if (saveData == null)
            {
                return;
            }

            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(SAVE_KEY, json);
            PlayerPrefs.Save();
        }

        public bool TryLoadProgress(out GameSaveData saveData)
        {
            saveData = null;

            if (!PlayerPrefs.HasKey(SAVE_KEY))
            {
                return false;
            }

            string json = PlayerPrefs.GetString(SAVE_KEY, string.Empty);

            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            saveData = JsonUtility.FromJson<GameSaveData>(json);

            if (saveData == null)
            {
                return false;
            }

            return true;
        }

        public void ClearProgress()
        {
            PlayerPrefs.DeleteKey(SAVE_KEY);
            PlayerPrefs.Save();
        }
    }
}

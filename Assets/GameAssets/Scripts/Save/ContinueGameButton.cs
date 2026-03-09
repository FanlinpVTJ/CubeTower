using CubeGame.SceneLoading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CubeGame.Save
{
    [RequireComponent(typeof(Button))]
    public sealed class ContinueGameButton : MonoBehaviour
    {
        [SerializeField] private string gameSceneName;

        private Button button;

        [Inject] private IGameSaver gameSaver;
        [Inject] private IMenuSceneProgressHandler menuSceneProgressHandler;
        [Inject] private ISceneLoader sceneLoader;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void Start()
        {
            RefreshState();
        }

        private void OnEnable()
        {
            if (gameSaver != null)
            {
                gameSaver.OnInitialized += OnGameSaverInitialized;
            }

            if (button != null)
            {
                button.onClick.AddListener(OnClick);
            }

            RefreshState();
        }

        private void OnDisable()
        {
            if (gameSaver != null)
            {
                gameSaver.OnInitialized -= OnGameSaverInitialized;
            }

            if (button == null)
            {
                return;
            }

            button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (menuSceneProgressHandler == null || sceneLoader == null)
            {
                return;
            }

            if (!menuSceneProgressHandler.HasSave)
            {
                return;
            }

            sceneLoader.LoadScene(gameSceneName);
        }

        private void OnGameSaverInitialized()
        {
            RefreshState();
        }

        private void RefreshState()
        {
            if (button == null)
            {
                return;
            }

            bool isInteractable = menuSceneProgressHandler != null && menuSceneProgressHandler.HasSave;
            button.interactable = isInteractable;
        }
    }
}

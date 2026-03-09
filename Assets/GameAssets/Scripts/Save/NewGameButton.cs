using CubeGame.SceneLoading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CubeGame.Save
{
    [RequireComponent(typeof(Button))]
    public sealed class NewGameButton : MonoBehaviour
    {
        [SerializeField] private string gameSceneName;

        private Button button;

        [Inject] private IMenuSceneProgressHandler menuSceneProgressHandler;
        [Inject] private ISceneLoader sceneLoader;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            if (button != null)
            {
                button.onClick.AddListener(OnClick);
            }
        }

        private void OnDisable()
        {
            if (button == null)
            {
                return;
            }

            button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (menuSceneProgressHandler != null)
            {
                menuSceneProgressHandler.StartNewGame();
            }

            if (sceneLoader == null)
            {
                return;
            }

            sceneLoader.LoadScene(gameSceneName);
        }
    }
}

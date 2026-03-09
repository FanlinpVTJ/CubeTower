using CubeGame.SceneLoading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CubeGame.Save
{
    [RequireComponent(typeof(Button))]
    public sealed class ExitToMenuButton : MonoBehaviour
    {
        [SerializeField] private string menuSceneName;

        private Button button;

        [Inject] private IGameSceneProgressHandler gameSceneProgressHandler;
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
            if (gameSceneProgressHandler != null)
            {
                gameSceneProgressHandler.SaveProgress();
            }

            if (sceneLoader == null)
            {
                return;
            }

            sceneLoader.LoadScene(menuSceneName);
        }
    }
}

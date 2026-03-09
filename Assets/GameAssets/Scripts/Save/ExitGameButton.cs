using UnityEngine;
using UnityEngine.UI;

namespace CubeGame.Save
{
    [RequireComponent(typeof(Button))]
    public sealed class ExitGameButton : MonoBehaviour
    {
        private Button button;

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
            Application.Quit();
        }
    }
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ToolsAndMechanics.Misc.UI
{
    public class ButtonPressAnimation : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        private Tween _animation;

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void OnButtonClick()
        {
            if (_animation.IsActive())
                _animation.Kill(true);

            _animation = transform.DOPunchScale(-Vector3.one * 0.15f, 0.15f)
                .SetUpdate(true)
                .SetEase(Ease.InOutSine)
                .SetLink(gameObject);
        }
    }
}


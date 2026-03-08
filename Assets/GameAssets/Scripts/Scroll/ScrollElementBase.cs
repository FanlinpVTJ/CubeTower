using UnityEngine;
using UnityEngine.UI;

namespace CubeGame.Scroll
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class ScrollElementBase : MonoBehaviour, IScrollElement
    {
        [SerializeField] private RectTransform root;
        [SerializeField] private Image viewTarget;

        public RectTransform Root => root != null ? root : (RectTransform)transform;
        public ScrollElementData Data { get; private set; }
        public string ElementId => Data != null ? Data.ElementId : string.Empty;

        public virtual void Initialize(ScrollElementData data)
        {
            Data = data;

            if (data == null)
            {
                gameObject.name = "ScrollElement_Empty";

                if (viewTarget != null)
                {
                    viewTarget.sprite = null;
                }

                return;
            }

            gameObject.name = $"ScrollElement_{data.ElementId}";

            if (viewTarget != null)
            {
                viewTarget.sprite = data.ElementView;
            }
        }

        protected virtual void Reset()
        {
            root = (RectTransform)transform;
            viewTarget = GetComponent<Image>();
        }

        protected virtual void OnValidate()
        {
            if (root == null)
            {
                root = (RectTransform)transform;
            }

            if (viewTarget == null)
            {
                viewTarget = GetComponent<Image>();
            }
        }
    }
}

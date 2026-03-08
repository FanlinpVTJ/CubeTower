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
        public string ElementId { get; private set; }

        public virtual void Initialize(string elementId, Sprite elementView)
        {
            ElementId = elementId;
            gameObject.name = $"ScrollElement_{elementId}";

            if (viewTarget != null)
            {
                viewTarget.sprite = elementView;
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

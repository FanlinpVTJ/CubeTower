using UnityEngine;

namespace CubeGame.Screen
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class ScreenZoneBase : MonoBehaviour, IScreenZone
    {
        [SerializeField] private RectTransform root;

        public RectTransform Root => root != null ? root : (RectTransform)transform;

        public virtual void SetVisible(bool isVisible)
        {
            Root.gameObject.SetActive(isVisible);
        }

        public virtual void Initialize()
        {
        }

        protected virtual void Reset()
        {
            root = (RectTransform)transform;
        }

        protected virtual void OnValidate()
        {
            if (root == null)
            {
                root = (RectTransform)transform;
            }
        }
    }
}

using CubeGame.Scroll;
using UnityEngine;
using Zenject;

namespace CubeGame.Drag
{
    public sealed class DragElementFactory : IDragElementFactory
    {
        private readonly DiContainer container;
        private readonly DragElementBase elementPrefab;
        private readonly RectTransform dragElementsRoot;

        public DragElementFactory(DiContainer container, DragElementBase elementPrefab, RectTransform dragElementsRoot)
        {
            this.container = container;
            this.elementPrefab = elementPrefab;
            this.dragElementsRoot = dragElementsRoot;

            if (this.elementPrefab == null)
            {
                throw new ZenjectException("[DragElementFactory] Drag element prefab is not assigned.");
            }
        }

        public IDragElement Create(ScrollElementData data, Vector3 worldPosition)
        {
            var instance = container.InstantiatePrefab(elementPrefab.gameObject, dragElementsRoot);
            var element = instance.GetComponent<DragElementBase>();

            if (element == null)
            {
                throw new ZenjectException("[DragElementFactory] Created object has no DragElementBase.");
            }

            element.Initialize(data);
            element.Root.position = worldPosition;
            return element;
        }
    }
}

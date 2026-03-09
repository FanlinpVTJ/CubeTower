using CubeGame.Scroll;
using CubeGame.ObjectPoolManager;
using UnityEngine;
using Zenject;

namespace CubeGame.Drag
{
    public sealed class DragElementFactory : IDragElementFactory
    {
        private readonly PoolManager poolManager;
        private readonly PoolGroup dragElementPoolGroup;
        private readonly RectTransform dragElementsRoot;

        public DragElementFactory(PoolManager poolManager, PoolGroup dragElementPoolGroup, RectTransform dragElementsRoot)
        {
            this.poolManager = poolManager;
            this.dragElementPoolGroup = dragElementPoolGroup;
            this.dragElementsRoot = dragElementsRoot;

            if (this.dragElementPoolGroup == null)
            {
                throw new ZenjectException("[DragElementFactory] Drag element pool group is not assigned.");
            }
        }

        public IDragElement Create(ScrollElementData data, Vector3 worldPosition)
        {
            PooledObject pooledObject = poolManager.InstantiateFromGroup(dragElementPoolGroup, dragElementsRoot);
            DragElementBase element = pooledObject.GetComponent<DragElementBase>();

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

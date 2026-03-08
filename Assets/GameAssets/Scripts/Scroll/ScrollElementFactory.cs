using CubeGame.Screen;
using UnityEngine;
using Zenject;

namespace CubeGame.Scroll
{
    public sealed class ScrollElementFactory : IScrollElementFactory
    {
        private readonly DiContainer _container;
        private readonly IScrollZone _scrollZone;
        private readonly ScrollElementBase _elementPrefab;

        public ScrollElementFactory(DiContainer container, IScrollZone scrollZone, ScrollElementBase elementPrefab)
        {
            _container = container;
            _scrollZone = scrollZone;
            _elementPrefab = elementPrefab;

            if (_elementPrefab == null)
            {
                throw new ZenjectException("[ScrollElementFactory] Scroll element prefab is not assigned.");
            }
        }

        public IScrollElement Create(ScrollElementData data)
        {
            var instance = _container.InstantiatePrefab(_elementPrefab.gameObject, _scrollZone.ContentRoot);
            var element = instance.GetComponent<ScrollElementBase>();

            if (element == null)
            {
                throw new ZenjectException("[ScrollElementFactory] Created object has no ScrollElementBase.");
            }

            element.Initialize(data);
            return element;
        }
    }
}

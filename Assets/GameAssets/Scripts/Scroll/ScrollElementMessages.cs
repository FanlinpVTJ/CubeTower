using UnityEngine;

namespace CubeGame.Scroll
{
    public readonly struct ScrollElementPressedMessage
    {
        public ScrollElementPressedMessage(IScrollElement element, Vector2 pointerScreenPosition)
        {
            Element = element;
            PointerScreenPosition = pointerScreenPosition;
        }

        public IScrollElement Element { get; }
        public Vector2 PointerScreenPosition { get; }
        public ScrollElementData Data => Element != null ? Element.Data : null;
    }

    public readonly struct ScrollElementSpawnedMessage
    {
        public ScrollElementSpawnedMessage(IScrollElement element)
        {
            Element = element;
        }

        public IScrollElement Element { get; }
    }

    public readonly struct ScrollElementRemovedMessage
    {
        public ScrollElementRemovedMessage(IScrollElement element)
        {
            Element = element;
        }

        public IScrollElement Element { get; }
    }
}

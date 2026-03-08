using CubeGame.Drag;
using CubeGame.Scroll;
using UnityEngine;

namespace CubeGame.Input
{
    public readonly struct DragSessionStartedMessage
    {
        public DragSessionStartedMessage(IScrollElement scrollElement, IDragElement dragElement, Vector2 pointerScreenPosition)
        {
            ScrollElement = scrollElement;
            DragElement = dragElement;
            PointerScreenPosition = pointerScreenPosition;
        }

        public IScrollElement ScrollElement { get; }
        public IDragElement DragElement { get; }
        public Vector2 PointerScreenPosition { get; }
    }
}

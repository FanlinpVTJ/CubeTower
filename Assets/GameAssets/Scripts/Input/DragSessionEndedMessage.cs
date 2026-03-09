using CubeGame.Drag;
using CubeGame.Scroll;
using UnityEngine;

namespace CubeGame.Input
{
    public readonly struct DragSessionEndedMessage
    {
        public DragSessionEndedMessage(
            IScrollElement scrollElement,
            IDragElement dragElement,
            Vector2 pointerScreenPosition,
            Vector2 returnPosition)
        {
            ScrollElement = scrollElement;
            DragElement = dragElement;
            PointerScreenPosition = pointerScreenPosition;
            ReturnPosition = returnPosition;
        }

        public IScrollElement ScrollElement { get; }
        public IDragElement DragElement { get; }
        public Vector2 PointerScreenPosition { get; }
        public Vector2 ReturnPosition { get; }
    }
}

using CubeGame.Drag;
using CubeGame.Scroll;
using UnityEngine;

namespace CubeGame.Input
{
    public readonly struct DragSessionMovedMessage
    {
        public DragSessionMovedMessage(
            IScrollElement scrollElement,
            IDragElement dragElement,
            Vector2 pointerScreenPosition,
            Vector2 targetPosition)
        {
            ScrollElement = scrollElement;
            DragElement = dragElement;
            PointerScreenPosition = pointerScreenPosition;
            TargetPosition = targetPosition;
        }

        public IScrollElement ScrollElement { get; }
        public IDragElement DragElement { get; }
        public Vector2 PointerScreenPosition { get; }
        public Vector2 TargetPosition { get; }
    }
}

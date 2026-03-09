using CubeGame.Drag;
using CubeGame.Scroll;
using UnityEngine;

namespace CubeGame.Input
{
    public readonly struct DragSessionCancelledMessage
    {
        public DragSessionCancelledMessage(
            IScrollElement scrollElement,
            IDragElement dragElement,
            Vector2 returnPosition,
            float animationDuration)
        {
            ScrollElement = scrollElement;
            DragElement = dragElement;
            ReturnPosition = returnPosition;
            AnimationDuration = animationDuration;
        }

        public IScrollElement ScrollElement { get; }
        public IDragElement DragElement { get; }
        public Vector2 ReturnPosition { get; }
        public float AnimationDuration { get; }
    }
}

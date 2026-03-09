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
            float animationDuration,
            bool shouldDespawn)
        {
            ScrollElement = scrollElement;
            DragElement = dragElement;
            ReturnPosition = returnPosition;
            AnimationDuration = animationDuration;
            ShouldDespawn = shouldDespawn;
        }

        public IScrollElement ScrollElement { get; }
        public IDragElement DragElement { get; }
        public Vector2 ReturnPosition { get; }
        public float AnimationDuration { get; }
        public bool ShouldDespawn { get; }
    }
}

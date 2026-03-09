using CubeGame.Drag;
using CubeGame.Scroll;
using UnityEngine;

namespace CubeGame.Input
{
    public readonly struct DragSessionStartedMessage
    {
        public DragSessionStartedMessage(
            IScrollElement scrollElement,
            IDragElement dragElement,
            Vector2 pointerScreenPosition,
            Vector2 startPosition,
            Vector2 targetPosition,
            float animationDuration)
        {
            ScrollElement = scrollElement;
            DragElement = dragElement;
            PointerScreenPosition = pointerScreenPosition;
            StartPosition = startPosition;
            TargetPosition = targetPosition;
            AnimationDuration = animationDuration;
        }

        public IScrollElement ScrollElement { get; }
        public IDragElement DragElement { get; }
        public Vector2 PointerScreenPosition { get; }
        public Vector2 StartPosition { get; }
        public Vector2 TargetPosition { get; }
        public float AnimationDuration { get; }
    }
}

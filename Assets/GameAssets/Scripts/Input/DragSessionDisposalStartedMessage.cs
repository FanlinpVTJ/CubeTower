using CubeGame.Drag;
using CubeGame.Scroll;
using UnityEngine;

namespace CubeGame.Input
{
    public readonly struct DragSessionDisposalStartedMessage
    {
        public DragSessionDisposalStartedMessage(
            IScrollElement scrollElement,
            IDragElement dragElement,
            Vector2 targetPosition,
            float animationDuration)
        {
            ScrollElement = scrollElement;
            DragElement = dragElement;
            TargetPosition = targetPosition;
            AnimationDuration = animationDuration;
        }

        public IScrollElement ScrollElement { get; }
        public IDragElement DragElement { get; }
        public Vector2 TargetPosition { get; }
        public float AnimationDuration { get; }
    }
}

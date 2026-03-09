using CubeGame.Drag;
using UnityEngine;

namespace CubeGame.Tower
{
    public readonly struct TowerBlockShiftedMessage
    {
        public TowerBlockShiftedMessage(
            IDragElement dragElement,
            Vector2 targetPosition,
            float animationDuration,
            float startDelay)
        {
            DragElement = dragElement;
            TargetPosition = targetPosition;
            AnimationDuration = animationDuration;
            StartDelay = startDelay;
        }

        public IDragElement DragElement { get; }
        public Vector2 TargetPosition { get; }
        public float AnimationDuration { get; }
        public float StartDelay { get; }
    }
}

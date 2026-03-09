using CubeGame.Drag;
using UnityEngine;

namespace CubeGame.Tower
{
    public sealed class TowerPlacementContext
    {
        public TowerPlacementContext(
            IDragElement dragElement,
            Vector2 pointerScreenPosition,
            Vector2 candidatePosition,
            Vector2 elementSize)
        {
            DragElement = dragElement;
            PointerScreenPosition = pointerScreenPosition;
            CandidatePosition = candidatePosition;
            ElementSize = elementSize;
        }

        public IDragElement DragElement { get; }
        public Vector2 PointerScreenPosition { get; }
        public Vector2 CandidatePosition { get; }
        public Vector2 ElementSize { get; }
    }
}

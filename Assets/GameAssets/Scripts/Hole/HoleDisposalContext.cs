using CubeGame.Drag;
using UnityEngine;

namespace CubeGame.Hole
{
    public sealed class HoleDisposalContext
    {
        public HoleDisposalContext(
            IDragElement dragElement,
            Vector2 pointerScreenPosition,
            Vector2 dragPosition,
            Vector2 elementSize)
        {
            DragElement = dragElement;
            PointerScreenPosition = pointerScreenPosition;
            DragPosition = dragPosition;
            ElementSize = elementSize;
        }

        public IDragElement DragElement { get; }
        public Vector2 PointerScreenPosition { get; }
        public Vector2 DragPosition { get; }
        public Vector2 ElementSize { get; }
    }
}

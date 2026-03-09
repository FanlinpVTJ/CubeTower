using CubeGame.Drag;
using UnityEngine;

namespace CubeGame.Tower
{
    public sealed class TowerShiftEntry
    {
        public TowerShiftEntry(IDragElement dragElement, Vector2 targetPosition)
        {
            DragElement = dragElement;
            TargetPosition = targetPosition;
        }

        public IDragElement DragElement { get; }
        public Vector2 TargetPosition { get; }
    }
}

using CubeGame.Drag;
using UnityEngine;

namespace CubeGame.Tower
{
    public sealed class TowerBlockEntry
    {
        public TowerBlockEntry(IDragElement dragElement, string elementId, Vector2 position, Vector2 size)
        {
            DragElement = dragElement;
            ElementId = elementId;
            Position = position;
            Size = size;
        }

        public IDragElement DragElement { get; }
        public string ElementId { get; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; }
    }
}

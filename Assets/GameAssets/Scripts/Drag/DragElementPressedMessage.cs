using UnityEngine;

namespace CubeGame.Drag
{
    public readonly struct DragElementPressedMessage
    {
        public DragElementPressedMessage(IDragElement element, Vector2 pointerScreenPosition)
        {
            Element = element;
            PointerScreenPosition = pointerScreenPosition;
        }

        public IDragElement Element { get; }
        public Vector2 PointerScreenPosition { get; }
    }
}

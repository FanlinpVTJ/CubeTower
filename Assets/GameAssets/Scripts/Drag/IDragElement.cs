using CubeGame.Scroll;
using UnityEngine;

namespace CubeGame.Drag
{
    public interface IDragElement
    {
        RectTransform Root { get; }
        ScrollElementData Data { get; }
        void Initialize(ScrollElementData data);
        void OnDragStart(Vector2 pointerScreenPosition);
        void OnDrag(Vector2 pointerScreenPosition);
        void OnDragEnd(Vector2 pointerScreenPosition);
    }
}

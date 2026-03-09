using UnityEngine;

namespace CubeGame.Scroll
{
    public interface IScrollElement
    {
        RectTransform Root { get; }
        ScrollElementData Data { get; }
        string ElementId { get; }
        void Initialize(ScrollElementData data);
        void OnDragStart(Vector2 pointerScreenPosition);
        void OnDrag(Vector2 pointerScreenPosition);
        void OnDragEnd(Vector2 pointerScreenPosition);
    }
}

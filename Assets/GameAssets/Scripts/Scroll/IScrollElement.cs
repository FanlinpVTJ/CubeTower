using UnityEngine;

namespace CubeGame.Scroll
{
    public interface IScrollElement
    {
        RectTransform Root { get; }
        ScrollElementData Data { get; }
        string ElementId { get; }
        void Initialize(ScrollElementData data);
    }
}

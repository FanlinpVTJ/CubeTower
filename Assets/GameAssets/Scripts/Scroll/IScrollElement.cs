using UnityEngine;

namespace CubeGame.Scroll
{
    public interface IScrollElement
    {
        RectTransform Root { get; }
        string ElementId { get; }
        void Initialize(string elementId, Sprite elementView);
    }
}

using UnityEngine;

namespace CubeGame.Scroll
{
    public interface IScrollElementFactory
    {
        IScrollElement Create(string elementId, Sprite elementView);
    }
}

using CubeGame.Scroll;
using UnityEngine;

namespace CubeGame.Input
{
    public interface IScrollDragSessionController
    {
        void TryStartFromScroll(IScrollElement scrollElement, Vector2 pointerScreenPosition);
    }
}

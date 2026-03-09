using CubeGame.Drag;
using UnityEngine;

namespace CubeGame.Hole
{
    public interface IHoleService
    {
        HoleDisposalResult TryDispose(IDragElement dragElement, Vector2 pointerScreenPosition);
    }
}

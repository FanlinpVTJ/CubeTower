using CubeGame.Scroll;
using UnityEngine;

namespace CubeGame.Drag
{
    public interface IDragElementFactory
    {
        IDragElement Create(ScrollElementData data, Vector3 worldPosition);
    }
}

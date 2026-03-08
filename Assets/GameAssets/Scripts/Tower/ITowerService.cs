using System.Collections.Generic;
using CubeGame.Drag;
using UnityEngine;

namespace CubeGame.Tower
{
    public interface ITowerService
    {
        TowerPlacementResult TryPlace(IDragElement dragElement, Vector2 pointerScreenPosition);
        List<TowerBlockEntry> GetBlocks();
    }
}

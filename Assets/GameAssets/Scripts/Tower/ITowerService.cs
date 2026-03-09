using System.Collections.Generic;
using CubeGame.Drag;
using UnityEngine;

namespace CubeGame.Tower
{
    public interface ITowerService
    {
        TowerPlacementResult TryPlace(IDragElement dragElement, Vector2 pointerScreenPosition);
        TowerRemovalResult TryRemove(IDragElement dragElement);
        void Clear();
        void Restore(TowerSnapshot snapshot);
        TowerSnapshot GetSnapshot();
        List<TowerBlockEntry> GetBlocks();
    }
}

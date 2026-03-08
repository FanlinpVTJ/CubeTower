using UnityEngine;

namespace CubeGame.Tower
{
    public interface ITowerPositionResolver
    {
        Vector2 Resolve(TowerState towerState, Vector2 pointerScreenPosition, Vector2 elementSize);
    }
}

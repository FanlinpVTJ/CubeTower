namespace CubeGame.Tower
{
    public interface ITowerPlacementRule
    {
        TowerPlacementFailureReasonType Validate(TowerPlacementContext context, TowerState towerState);
    }
}

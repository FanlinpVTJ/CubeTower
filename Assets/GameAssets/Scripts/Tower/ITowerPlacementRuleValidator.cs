namespace CubeGame.Tower
{
    public interface ITowerPlacementRuleValidator
    {
        TowerPlacementFailureReasonType Validate(TowerPlacementContext context, TowerState towerState);
    }
}

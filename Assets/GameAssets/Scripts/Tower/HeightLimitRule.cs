namespace CubeGame.Tower
{
    public sealed class HeightLimitRule : ITowerPlacementRule
    {
        public TowerPlacementFailureReasonType Validate(TowerPlacementContext context, TowerState towerState)
        {
            if (towerState == null || !towerState.IsHeightLimitReached)
            {
                return TowerPlacementFailureReasonType.None;
            }

            return TowerPlacementFailureReasonType.HeightLimitReached;
        }
    }
}

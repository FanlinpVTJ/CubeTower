namespace CubeGame.Tower
{
    public sealed class HeightLimitRule : ITowerPlacementRule
    {
        public TowerPlacementFailureReasonType Validate(TowerPlacementContext context, TowerState towerState)
        {
            float topEdge = context.CandidatePosition.y + context.ElementSize.y * 0.5f;

            if (topEdge <= UnityEngine.Screen.height)
            {
                return TowerPlacementFailureReasonType.None;
            }

            return TowerPlacementFailureReasonType.HeightLimitReached;
        }
    }
}

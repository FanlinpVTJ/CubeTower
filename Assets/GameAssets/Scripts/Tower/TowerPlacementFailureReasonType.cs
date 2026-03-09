namespace CubeGame.Tower
{
    public enum TowerPlacementFailureReasonType
    {
        None = 0,
        InvalidElement = 1,
        NotInRightZone = 2,
        MustPlaceOnTopBlock = 3,
        HeightLimitReached = 4
    }
}

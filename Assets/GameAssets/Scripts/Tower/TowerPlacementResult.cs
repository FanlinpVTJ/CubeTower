using UnityEngine;

namespace CubeGame.Tower
{
    public readonly struct TowerPlacementResult
    {
        public TowerPlacementResult(
            bool isSuccess,
            TowerPlacementFailureReasonType failureReason,
            Vector2 targetPosition,
            bool hasReachedHeightLimit)
        {
            IsSuccess = isSuccess;
            FailureReason = failureReason;
            TargetPosition = targetPosition;
            HasReachedHeightLimit = hasReachedHeightLimit;
        }

        public bool IsSuccess { get; }
        public TowerPlacementFailureReasonType FailureReason { get; }
        public Vector2 TargetPosition { get; }
        public bool HasReachedHeightLimit { get; }
    }
}

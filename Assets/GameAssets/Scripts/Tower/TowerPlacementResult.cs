using UnityEngine;

namespace CubeGame.Tower
{
    public readonly struct TowerPlacementResult
    {
        public TowerPlacementResult(bool isSuccess, TowerPlacementFailureReasonType failureReason, Vector2 targetPosition)
        {
            IsSuccess = isSuccess;
            FailureReason = failureReason;
            TargetPosition = targetPosition;
        }

        public bool IsSuccess { get; }
        public TowerPlacementFailureReasonType FailureReason { get; }
        public Vector2 TargetPosition { get; }
    }
}

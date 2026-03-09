using UnityEngine;

namespace CubeGame.Hole
{
    public readonly struct HoleDisposalResult
    {
        public HoleDisposalResult(bool isSuccess, HoleDisposalFailureReasonType failureReason, Vector2 targetPosition)
        {
            IsSuccess = isSuccess;
            FailureReason = failureReason;
            TargetPosition = targetPosition;
        }

        public bool IsSuccess { get; }
        public HoleDisposalFailureReasonType FailureReason { get; }
        public Vector2 TargetPosition { get; }
    }
}

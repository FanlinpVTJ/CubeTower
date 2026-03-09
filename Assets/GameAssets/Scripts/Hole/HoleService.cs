using CubeGame.Drag;
using CubeGame.Screen;
using UnityEngine;

namespace CubeGame.Hole
{
    public sealed class HoleService : IHoleService
    {
        private readonly IHoleDisposalRuleValidator ruleValidator;
        private readonly IHoleView holeView;

        public HoleService(IHoleDisposalRuleValidator ruleValidator, IHoleView holeView)
        {
            this.ruleValidator = ruleValidator;
            this.holeView = holeView;
        }

        public HoleDisposalResult TryDispose(IDragElement dragElement, Vector2 pointerScreenPosition)
        {
            if (dragElement == null || dragElement.Root == null)
            {
                HoleDisposalResult invalidResult = new HoleDisposalResult(
                    false,
                    HoleDisposalFailureReasonType.InvalidElement,
                    Vector2.zero);

                return invalidResult;
            }

            Vector2 elementSize = ResolveElementSize(dragElement.Root);
            Vector2 dragPosition = dragElement.Root.position;
            HoleDisposalContext context = new HoleDisposalContext(
                dragElement,
                pointerScreenPosition,
                dragPosition,
                elementSize);
            HoleDisposalFailureReasonType failureReason = ruleValidator.Validate(context);

            if (failureReason != HoleDisposalFailureReasonType.None)
            {
                HoleDisposalResult failureResult = new HoleDisposalResult(
                    false,
                    failureReason,
                    dragPosition);

                return failureResult;
            }

            Vector2 targetPosition = ResolveTargetPosition();
            HoleDisposalResult successResult = new HoleDisposalResult(
                true,
                HoleDisposalFailureReasonType.None,
                targetPosition);

            return successResult;
        }

        private Vector2 ResolveElementSize(RectTransform rectTransform)
        {
            Vector2 size = rectTransform.rect.size;
            Vector3 scale = rectTransform.lossyScale;
            Vector2 result = new Vector2(size.x * scale.x, size.y * scale.y);

            return result;
        }

        private Vector2 ResolveTargetPosition()
        {
            if (holeView == null || holeView.HoleRoot == null)
            {
                return Vector2.zero;
            }

            RectTransform holeRoot = holeView.HoleRoot;
            Vector3 targetWorldPosition = holeRoot.TransformPoint(holeRoot.rect.center);
            Vector2 targetPosition = targetWorldPosition;

            return targetPosition;
        }
    }
}

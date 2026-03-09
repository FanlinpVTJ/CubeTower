using CubeGame.Input;
using DG.Tweening;
using CubeGame.ObjectPoolManager;
using CubeGame.Scroll;
using CubeGame.Tower;
using UnityEngine;

namespace CubeGame.Drag
{
    public sealed class BasicDragElement : DragElementBase
    {
        [Zenject.Inject(Optional = true)] private MessagePipe.IPublisher<DragSessionReturnedMessage> dragSessionReturnedPublisher;
        [Zenject.Inject(Optional = true)] private MessagePipe.IPublisher<DragSessionDisposedMessage> dragSessionDisposedPublisher;
        [SerializeField] private RectTransform animationHolder;

        private const float START_SCALE = 0.92f;
        private const float END_SCALE = 1f;
        private const float BOUNCE_DURATION = 0.2f;

        private Tween scaleTween;
        private Tween positionTween;
        private Tween holderTween;

        public override void Initialize(ScrollElementData data)
        {
            base.Initialize(data);
            KillScaleTween();
            KillPositionTween();
            KillHolderTween();
            Root.localScale = Vector3.one;
            ResetAnimationHolder();
        }

        public override void OnDragStart(Vector2 pointerScreenPosition)
        {
            KillScaleTween();
            Root.localScale = Vector3.one * START_SCALE;
            scaleTween = Root.DOScale(END_SCALE, BOUNCE_DURATION).SetEase(Ease.OutBack);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            KillScaleTween();
            KillPositionTween();
            KillHolderTween();
            Root.localScale = Vector3.one;
            ResetAnimationHolder();
        }

        protected override void HandleDragSessionStarted(DragSessionStartedMessage message)
        {
            Root.position = message.TargetPosition;
            AnimateHolderFromStart(message);
        }

        protected override void HandleDragSessionCancelled(DragSessionCancelledMessage message)
        {
            KillScaleTween();
            KillPositionTween();
            KillHolderTween();
            ResetAnimationHolder();
            positionTween = Root.DOMove(message.ReturnPosition, message.AnimationDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    if (dragSessionReturnedPublisher != null)
                    {
                        DragSessionReturnedMessage returnedMessage = new DragSessionReturnedMessage(
                            message.ScrollElement,
                            this);
                        dragSessionReturnedPublisher.Publish(returnedMessage);
                    }

                    if (message.ShouldDespawn)
                    {
                        PooledObject.Despawn(gameObject);
                    }
                });
        }

        protected override void HandleDragSessionDisposalStarted(DragSessionDisposalStartedMessage message)
        {
            KillScaleTween();
            KillPositionTween();
            KillHolderTween();
            ResetAnimationHolder();
            positionTween = Root.DOMove(message.TargetPosition, message.AnimationDuration)
                .SetEase(Ease.InQuad);
            scaleTween = Root.DOScale(Vector3.zero, message.AnimationDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    if (dragSessionDisposedPublisher != null)
                    {
                        DragSessionDisposedMessage disposedMessage = new DragSessionDisposedMessage(
                            message.ScrollElement,
                            this);
                        dragSessionDisposedPublisher.Publish(disposedMessage);
                    }

                    PooledObject.Despawn(gameObject);
                });
        }

        protected override void HandleTowerBlockShifted(TowerBlockShiftedMessage message)
        {
            KillPositionTween();
            positionTween = Root.DOMove(message.TargetPosition, message.AnimationDuration)
                .SetEase(Ease.OutQuad);
        }

        private void KillScaleTween()
        {
            if (scaleTween == null)
            {
                return;
            }

            scaleTween.Kill();
            scaleTween = null;
        }

        private void KillPositionTween()
        {
            if (positionTween == null)
            {
                return;
            }

            positionTween.Kill();
            positionTween = null;
        }

        private void AnimateHolderFromStart(DragSessionStartedMessage message)
        {
            if (animationHolder == null)
            {
                return;
            }

            KillHolderTween();
            Vector3 worldOffset = message.StartPosition - message.TargetPosition;
            Vector3 localOffset = Root.InverseTransformVector(worldOffset);
            animationHolder.localPosition = localOffset;
            holderTween = animationHolder.DOLocalMove(Vector3.zero, message.AnimationDuration).SetEase(Ease.OutQuad);
        }

        private void ResetAnimationHolder()
        {
            if (animationHolder == null)
            {
                return;
            }

            animationHolder.localPosition = Vector3.zero;
        }

        private void KillHolderTween()
        {
            if (holderTween == null)
            {
                return;
            }

            holderTween.Kill();
            holderTween = null;
        }
    }
}

using CubeGame.Input;
using DG.Tweening;
using CubeGame.ObjectPoolManager;
using CubeGame.Scroll;
using UnityEngine;

namespace CubeGame.Drag
{
    public sealed class BasicDragElement : DragElementBase
    {
        [Zenject.Inject(Optional = true)] private MessagePipe.IPublisher<DragSessionReturnedMessage> dragSessionReturnedPublisher;

        private const float START_SCALE = 0.92f;
        private const float END_SCALE = 1f;
        private const float BOUNCE_DURATION = 0.2f;

        private Tween scaleTween;
        private Tween positionTween;

        public override void Initialize(ScrollElementData data)
        {
            base.Initialize(data);
            KillScaleTween();
            KillPositionTween();
            Root.localScale = Vector3.one;
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
            Root.localScale = Vector3.one;
        }

        protected override void HandleDragSessionStarted(DragSessionStartedMessage message)
        {
            KillPositionTween();
            Root.position = message.StartPosition;
            positionTween = Root.DOMove(message.TargetPosition, message.AnimationDuration).SetEase(Ease.OutQuad);
        }

        protected override void HandleDragSessionCancelled(DragSessionCancelledMessage message)
        {
            KillScaleTween();
            KillPositionTween();
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

                    PooledObject.Despawn(gameObject);
                });
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
    }
}

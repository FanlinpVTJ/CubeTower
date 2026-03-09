using CubeGame.Input;
using DG.Tweening;
using CubeGame.Hole;
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
        [Zenject.Inject(Optional = true)] private ScrollRuntimeConfig scrollRuntimeConfig;
        [Zenject.Inject(Optional = true)] private TowerConfig towerConfig;
        [Zenject.Inject(Optional = true)] private HoleConfig holeConfig;
        [SerializeField] private RectTransform animationHolder;

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
            Root.localScale = Vector3.one * ResolveDragStartScaleFrom();
            scaleTween = Root.DOScale(Vector3.one, ResolveDragStartScaleDuration()).SetEase(ResolveDragStartScaleEase());
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
                .SetEase(ResolveDragCancelMoveEase())
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

        protected override void HandleDragSessionPlaced(DragSessionPlacedMessage message)
        {
            KillScaleTween();
            KillPositionTween();
            KillHolderTween();
            AnimatePlacementHolder(message.AnimationDuration);
            positionTween = Root.DOMove(message.TargetPosition, message.AnimationDuration)
                .SetEase(ResolvePlacedMoveEase());
            Root.localScale = Vector3.one * ResolvePlacedScaleFrom();
            scaleTween = Root.DOScale(Vector3.one, message.AnimationDuration)
                .SetEase(ResolvePlacedScaleEase());
        }

        protected override void HandleDragSessionDisposalStarted(DragSessionDisposalStartedMessage message)
        {
            KillScaleTween();
            KillPositionTween();
            KillHolderTween();
            ResetAnimationHolder();
            positionTween = Root.DOMove(message.TargetPosition, message.AnimationDuration)
                .SetEase(ResolveDisposeMoveEase());
            scaleTween = Root.DOScale(Vector3.zero, message.AnimationDuration)
                .SetEase(ResolveDisposeScaleEase())
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
                .SetDelay(message.StartDelay)
                .SetEase(ResolveTowerShiftEase());
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
            holderTween = animationHolder.DOLocalMove(Vector3.zero, message.AnimationDuration).SetEase(ResolveDragStartMoveEase());
        }

        private void AnimatePlacementHolder(float animationDuration)
        {
            if (animationHolder == null)
            {
                return;
            }

            Vector3 startPosition = new Vector3(0f, ResolvePlacedHolderOffsetY(), 0f);
            animationHolder.localPosition = startPosition;
            holderTween = animationHolder.DOLocalMove(Vector3.zero, animationDuration).SetEase(ResolvePlacedHolderEase());
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

        private float ResolveDragStartScaleFrom()
        {
            if (scrollRuntimeConfig == null)
            {
                return 0.92f;
            }

            return scrollRuntimeConfig.DragStartScaleFrom;
        }

        private float ResolveDragStartScaleDuration()
        {
            if (scrollRuntimeConfig == null)
            {
                return 0.2f;
            }

            return scrollRuntimeConfig.DragStartScaleDuration;
        }

        private Ease ResolveDragStartScaleEase()
        {
            if (scrollRuntimeConfig == null)
            {
                return Ease.OutBack;
            }

            return scrollRuntimeConfig.DragStartScaleEase;
        }

        private Ease ResolveDragStartMoveEase()
        {
            if (scrollRuntimeConfig == null)
            {
                return Ease.OutQuad;
            }

            return scrollRuntimeConfig.DragStartMoveEase;
        }

        private Ease ResolveDragCancelMoveEase()
        {
            if (scrollRuntimeConfig == null)
            {
                return Ease.InQuad;
            }

            return scrollRuntimeConfig.DragCancelMoveEase;
        }

        private float ResolvePlacedScaleFrom()
        {
            if (towerConfig == null)
            {
                return 0.92f;
            }

            return towerConfig.BlockPlacedScaleFrom;
        }

        private float ResolvePlacedHolderOffsetY()
        {
            if (towerConfig == null)
            {
                return 36f;
            }

            return towerConfig.BlockPlacedHolderOffsetY;
        }

        private Ease ResolvePlacedMoveEase()
        {
            if (towerConfig == null)
            {
                return Ease.OutQuad;
            }

            return towerConfig.BlockPlacedMoveEase;
        }

        private Ease ResolvePlacedScaleEase()
        {
            if (towerConfig == null)
            {
                return Ease.OutBack;
            }

            return towerConfig.BlockPlacedScaleEase;
        }

        private Ease ResolvePlacedHolderEase()
        {
            if (towerConfig == null)
            {
                return Ease.OutBounce;
            }

            return towerConfig.BlockPlacedHolderEase;
        }

        private Ease ResolveDisposeMoveEase()
        {
            if (holeConfig == null)
            {
                return Ease.InQuad;
            }

            return holeConfig.DisposeMoveEase;
        }

        private Ease ResolveDisposeScaleEase()
        {
            if (holeConfig == null)
            {
                return Ease.InBack;
            }

            return holeConfig.DisposeScaleEase;
        }

        private Ease ResolveTowerShiftEase()
        {
            if (towerConfig == null)
            {
                return Ease.OutQuad;
            }

            return towerConfig.TowerShiftEase;
        }
    }
}

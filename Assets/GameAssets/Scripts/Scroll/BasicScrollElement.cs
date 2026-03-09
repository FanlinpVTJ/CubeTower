using System;
using CubeGame.Input;
using DG.Tweening;
using MessagePipe;
using UnityEngine;

namespace CubeGame.Scroll
{
    public sealed class BasicScrollElement : ScrollElementBase
    {
        [Zenject.Inject(Optional = true)] private ISubscriber<DragSessionPlacedMessage> dragSessionPlacedSubscriber;
        [Zenject.Inject(Optional = true)] private ISubscriber<DragSessionReturnedMessage> dragSessionReturnedSubscriber;

        private const float START_SCALE = 0.92f;
        private const float END_SCALE = 1f;
        private const float BOUNCE_DURATION = 0.2f;

        private Tween scaleTween;
        private IDisposable dragSessionPlacedSubscription;
        private IDisposable dragSessionReturnedSubscription;

        public override void Initialize(ScrollElementData data)
        {
            base.Initialize(data);
            KillScaleTween();
            Root.localScale = Vector3.one;

            if (ViewTarget != null)
            {
                ViewTarget.enabled = true;
            }
        }

        public override void OnDragStart(Vector2 pointerScreenPosition)
        {
            if (ViewTarget == null)
            {
                return;
            }

            KillScaleTween();
            Root.localScale = Vector3.one;
            ViewTarget.enabled = false;
        }

        public override void OnDragEnd(Vector2 pointerScreenPosition)
        {
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (dragSessionPlacedSubscriber != null)
            {
                dragSessionPlacedSubscription = dragSessionPlacedSubscriber.Subscribe(OnDragSessionPlaced);
            }

            if (dragSessionReturnedSubscriber != null)
            {
                dragSessionReturnedSubscription = dragSessionReturnedSubscriber.Subscribe(OnDragSessionReturned);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            dragSessionPlacedSubscription?.Dispose();
            dragSessionPlacedSubscription = null;
            dragSessionReturnedSubscription?.Dispose();
            dragSessionReturnedSubscription = null;
            KillScaleTween();
            Root.localScale = Vector3.one;

            if (ViewTarget != null)
            {
                ViewTarget.enabled = true;
            }
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

        private void OnDragSessionPlaced(DragSessionPlacedMessage message)
        {
            if (message.ScrollElement != this)
            {
                return;
            }

            ShowAnimated();
        }

        private void OnDragSessionReturned(DragSessionReturnedMessage message)
        {
            if (message.ScrollElement != this)
            {
                return;
            }

            ShowAnimated();
        }

        private void ShowAnimated()
        {
            if (ViewTarget == null)
            {
                return;
            }

            ViewTarget.enabled = true;
            KillScaleTween();
            Root.localScale = Vector3.one * START_SCALE;
            scaleTween = Root.DOScale(END_SCALE, BOUNCE_DURATION).SetEase(Ease.OutBack);
        }
    }
}

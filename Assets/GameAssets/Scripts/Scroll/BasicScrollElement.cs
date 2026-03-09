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
        [Zenject.Inject(Optional = true)] private ISubscriber<DragSessionDisposedMessage> dragSessionDisposedSubscriber;
        [Zenject.Inject(Optional = true)] private ScrollRuntimeConfig scrollRuntimeConfig;

        private Tween scaleTween;
        private IDisposable dragSessionPlacedSubscription;
        private IDisposable dragSessionReturnedSubscription;
        private IDisposable dragSessionDisposedSubscription;

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

            if (dragSessionDisposedSubscriber != null)
            {
                dragSessionDisposedSubscription = dragSessionDisposedSubscriber.Subscribe(OnDragSessionDisposed);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            dragSessionPlacedSubscription?.Dispose();
            dragSessionPlacedSubscription = null;
            dragSessionReturnedSubscription?.Dispose();
            dragSessionReturnedSubscription = null;
            dragSessionDisposedSubscription?.Dispose();
            dragSessionDisposedSubscription = null;
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

        private void OnDragSessionDisposed(DragSessionDisposedMessage message)
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
            Root.localScale = Vector3.one * ResolveShowScaleFrom();
            scaleTween = Root.DOScale(Vector3.one, ResolveShowScaleDuration()).SetEase(ResolveShowScaleEase());
        }

        private float ResolveShowScaleFrom()
        {
            if (scrollRuntimeConfig == null)
            {
                return 0.92f;
            }

            return scrollRuntimeConfig.ScrollElementShowScaleFrom;
        }

        private float ResolveShowScaleDuration()
        {
            if (scrollRuntimeConfig == null)
            {
                return 0.2f;
            }

            return scrollRuntimeConfig.ScrollElementShowScaleDuration;
        }

        private Ease ResolveShowScaleEase()
        {
            if (scrollRuntimeConfig == null)
            {
                return Ease.OutBack;
            }

            return scrollRuntimeConfig.ScrollElementShowScaleEase;
        }
    }
}

using System;
using MessagePipe;
using CubeGame.Screen;
using UnityEngine.UI;
using Zenject;

namespace CubeGame.Input
{
    public sealed class ScrollRectMovementController : IScrollMovementController, IInitializable, IDisposable
    {
        private readonly ScrollRect scrollRect;
        private readonly ISubscriber<DragSessionStartedMessage> startedSubscriber;
        private readonly ISubscriber<DragSessionEndedMessage> endedSubscriber;

        private IDisposable startedSubscription;
        private IDisposable endedSubscription;

        public float CurrentVelocityX => scrollRect != null ? scrollRect.velocity.x : 0f;

        public ScrollRectMovementController(
            IScrollView scrollView,
            ISubscriber<DragSessionStartedMessage> startedSubscriber,
            ISubscriber<DragSessionEndedMessage> endedSubscriber)
        {
            this.scrollRect = scrollView != null ? scrollView.Scroll : null;
            this.startedSubscriber = startedSubscriber;
            this.endedSubscriber = endedSubscriber;
        }

        public void Initialize()
        {
            startedSubscription = startedSubscriber.Subscribe(OnStarted);
            endedSubscription = endedSubscriber.Subscribe(OnEnded);
        }

        public void Dispose()
        {
            startedSubscription?.Dispose();
            endedSubscription?.Dispose();
        }

        public void Stop()
        {
            if (scrollRect == null)
            {
                return;
            }

            scrollRect.StopMovement();
            scrollRect.enabled = false;
        }

        public void Resume()
        {
            if (scrollRect == null)
            {
                return;
            }

            scrollRect.enabled = true;
        }

        private void OnStarted(DragSessionStartedMessage message)
        {
            Stop();
        }

        private void OnEnded(DragSessionEndedMessage message)
        {
            Resume();
        }
    }
}

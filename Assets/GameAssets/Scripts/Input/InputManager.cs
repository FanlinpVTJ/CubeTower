using System;
using CubeGame.Drag;
using CubeGame.Scroll;
using MessagePipe;
using Zenject;

namespace CubeGame.Input
{
    public sealed class InputManager : IInitializable, IDisposable
    {
        private readonly ISubscriber<ScrollElementPressedMessage> scrollPressedSubscriber;
        private readonly ISubscriber<DragElementPressedMessage> dragPressedSubscriber;
        private readonly IScrollInputHandler scrollInputHandler;
        private readonly IScrollDragSessionController dragSessionController;
        private IDisposable scrollPressedSubscription;
        private IDisposable dragPressedSubscription;

        public InputManager(
            ISubscriber<ScrollElementPressedMessage> scrollPressedSubscriber,
            ISubscriber<DragElementPressedMessage> dragPressedSubscriber,
            IScrollInputHandler scrollInputHandler,
            IScrollDragSessionController dragSessionController)
        {
            this.scrollPressedSubscriber = scrollPressedSubscriber;
            this.dragPressedSubscriber = dragPressedSubscriber;
            this.scrollInputHandler = scrollInputHandler;
            this.dragSessionController = dragSessionController;
        }

        public void Initialize()
        {
            scrollPressedSubscription = scrollPressedSubscriber.Subscribe(scrollInputHandler.HandlePressed);
            dragPressedSubscription = dragPressedSubscriber.Subscribe(OnDragPressed);
        }

        public void Dispose()
        {
            scrollPressedSubscription?.Dispose();
            dragPressedSubscription?.Dispose();
        }

        private void OnDragPressed(DragElementPressedMessage message)
        {
            if (message.Element == null)
            {
                return;
            }

            dragSessionController.TryStartFromTower(message.Element, message.PointerScreenPosition);
        }
    }
}

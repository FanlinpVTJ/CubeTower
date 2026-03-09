using System;
using CubeGame.Scroll;
using MessagePipe;
using Zenject;

namespace CubeGame.Input
{
    public sealed class InputManager : IInitializable, IDisposable
    {
        private readonly ISubscriber<ScrollElementPressedMessage> scrollPressedSubscriber;
        private readonly IScrollInputHandler scrollInputHandler;
        private IDisposable scrollPressedSubscription;

        public InputManager(
            ISubscriber<ScrollElementPressedMessage> scrollPressedSubscriber,
            IScrollInputHandler scrollInputHandler)
        {
            this.scrollPressedSubscriber = scrollPressedSubscriber;
            this.scrollInputHandler = scrollInputHandler;
        }

        public void Initialize()
        {
            scrollPressedSubscription = scrollPressedSubscriber.Subscribe(scrollInputHandler.HandlePressed);
        }

        public void Dispose()
        {
            scrollPressedSubscription?.Dispose();
        }
    }
}

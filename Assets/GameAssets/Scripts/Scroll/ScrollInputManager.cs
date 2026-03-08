using System;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace CubeGame.Scroll
{
    public sealed class ScrollInputManager : IInitializable, IDisposable
    {
        private readonly ISubscriber<ScrollElementPressedMessage> pressedSubscriber;
        private readonly IScrollElementRegistry elementRegistry;
        private IDisposable pressedSubscription;

        public ScrollInputManager(
            ISubscriber<ScrollElementPressedMessage> pressedSubscriber,
            IScrollElementRegistry elementRegistry)
        {
            this.pressedSubscriber = pressedSubscriber;
            this.elementRegistry = elementRegistry;
        }

        public void Initialize()
        {
            pressedSubscription = pressedSubscriber.Subscribe(OnElementPressed);
        }

        public void Dispose()
        {
            pressedSubscription?.Dispose();
        }

        private void OnElementPressed(ScrollElementPressedMessage message)
        {
            if (message.Element == null)
            {
                return;
            }

            Debug.Log($"[ScrollInputManager] Pressed: {message.Element.ElementId}. Active elements: {elementRegistry.Elements.Count}");
        }
    }
}

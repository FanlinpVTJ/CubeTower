using System;
using System.Collections.Generic;
using MessagePipe;
using Zenject;

namespace CubeGame.Scroll
{
    public sealed class ScrollElementRegistry : IScrollElementRegistry, IInitializable, IDisposable
    {
        private readonly ISubscriber<ScrollElementSpawnedMessage> spawnedSubscriber;
        private readonly ISubscriber<ScrollElementRemovedMessage> removedSubscriber;

        private readonly List<IScrollElement> elements = new List<IScrollElement>();
        private IDisposable spawnedSubscription;
        private IDisposable removedSubscription;

        public ScrollElementRegistry(
            ISubscriber<ScrollElementSpawnedMessage> spawnedSubscriber,
            ISubscriber<ScrollElementRemovedMessage> removedSubscriber)
        {
            this.spawnedSubscriber = spawnedSubscriber;
            this.removedSubscriber = removedSubscriber;
        }

        public IReadOnlyList<IScrollElement> Elements => elements;

        public void Initialize()
        {
            spawnedSubscription = spawnedSubscriber.Subscribe(OnSpawned);
            removedSubscription = removedSubscriber.Subscribe(OnRemoved);
        }

        public void Dispose()
        {
            spawnedSubscription?.Dispose();
            removedSubscription?.Dispose();
        }

        public bool TryGetById(string elementId, out IScrollElement element)
        {
            for (var i = 0; i < elements.Count; i++)
            {
                var current = elements[i];
                if (current != null && current.ElementId == elementId)
                {
                    element = current;
                    return true;
                }
            }

            element = null;
            return false;
        }

        private void OnSpawned(ScrollElementSpawnedMessage message)
        {
            if (message.Element == null || elements.Contains(message.Element))
            {
                return;
            }

            elements.Add(message.Element);
        }

        private void OnRemoved(ScrollElementRemovedMessage message)
        {
            if (message.Element == null)
            {
                return;
            }

            elements.Remove(message.Element);
        }
    }
}

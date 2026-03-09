using MessagePipe;
using Zenject;

namespace CubeGame.Scroll
{
    public sealed class ScrollElementSpawner : IInitializable
    {
        private readonly IScrollElementFactory elementFactory;
        private readonly IScrollElementDataRepository dataRepository;
        private readonly IPublisher<ScrollElementSpawnedMessage> spawnedPublisher;

        public ScrollElementSpawner(
            IScrollElementFactory elementFactory,
            IScrollElementDataRepository dataRepository,
            IPublisher<ScrollElementSpawnedMessage> spawnedPublisher)
        {
            this.elementFactory = elementFactory;
            this.dataRepository = dataRepository;
            this.spawnedPublisher = spawnedPublisher;
        }

        public void Initialize()
        {
            var initialElements = dataRepository.GetInitialElements();
            for (var i = 0; i < initialElements.Count; i++)
            {
                var data = initialElements[i];
                if (data == null)
                {
                    continue;
                }

                var element = elementFactory.Create(data);
                spawnedPublisher.Publish(new ScrollElementSpawnedMessage(element));
            }
        }
    }
}

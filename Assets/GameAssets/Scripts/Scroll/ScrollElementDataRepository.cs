using System;
using System.Collections.Generic;

namespace CubeGame.Scroll
{
    public sealed class ScrollElementDataRepository : IScrollElementDataRepository
    {
        private static readonly IReadOnlyList<ScrollElementData> Empty = Array.Empty<ScrollElementData>();

        private readonly ScrollFeedConfig feedConfig;

        public ScrollElementDataRepository(ScrollFeedConfig feedConfig)
        {
            this.feedConfig = feedConfig;
        }

        public ScrollElementData FindById(string elementId)
        {
            if (feedConfig == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(elementId))
            {
                return null;
            }

            IReadOnlyList<ScrollElementData> initialElements = feedConfig.InitialElements;

            if (initialElements == null || initialElements.Count == 0)
            {
                return null;
            }

            for (int i = 0; i < initialElements.Count; i++)
            {
                ScrollElementData data = initialElements[i];

                if (data == null)
                {
                    continue;
                }

                if (data.ElementId == elementId)
                {
                    return data;
                }
            }

            return null;
        }

        public IReadOnlyList<ScrollElementData> GetInitialElements()
        {
            if (feedConfig == null)
            {
                return Empty;
            }

            return feedConfig.InitialElements;
        }
    }
}

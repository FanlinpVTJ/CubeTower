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

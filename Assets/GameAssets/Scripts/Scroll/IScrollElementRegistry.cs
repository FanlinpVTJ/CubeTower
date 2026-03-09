using System.Collections.Generic;

namespace CubeGame.Scroll
{
    public interface IScrollElementRegistry
    {
        IReadOnlyList<IScrollElement> Elements { get; }
        bool TryGetById(string elementId, out IScrollElement element);
    }
}

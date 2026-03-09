using System.Collections.Generic;

namespace CubeGame.Scroll
{
    public interface IScrollElementDataRepository
    {
        IReadOnlyList<ScrollElementData> GetInitialElements();
        ScrollElementData FindById(string elementId);
    }
}

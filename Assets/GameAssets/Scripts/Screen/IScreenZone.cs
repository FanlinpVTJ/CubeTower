using UnityEngine;
using UnityEngine.UI;

namespace CubeGame.Screen
{
    public interface IScreenZone
    {
        RectTransform Root { get; }
        void SetVisible(bool isVisible);
        void Initialize();
    }

    public interface ILeftZone : IScreenZone
    {
    }

    public interface IHoleView
    {
        RectTransform HoleRoot { get; }
    }

    public interface IRightZone : IScreenZone
    {
    }

    public interface IScrollZone : IScreenZone
    {
        RectTransform ContentRoot { get; }
    }

    public interface IScrollView
    {
        ScrollRect Scroll { get; }
    }
}

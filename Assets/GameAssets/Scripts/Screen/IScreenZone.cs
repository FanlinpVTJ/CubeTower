using UnityEngine;

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

    public interface IRightZone : IScreenZone
    {
    }

    public interface IScrollZone : IScreenZone
    {
        RectTransform ContentRoot { get; }
    }
}

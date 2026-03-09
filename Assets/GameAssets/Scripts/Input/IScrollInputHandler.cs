using CubeGame.Scroll;

namespace CubeGame.Input
{
    public interface IScrollInputHandler
    {
        void HandlePressed(ScrollElementPressedMessage message);
    }
}

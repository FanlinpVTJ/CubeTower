using CubeGame.Scroll;

namespace CubeGame.Input
{
    public sealed class ScrollInputHandler : IScrollInputHandler
    {
        private readonly IScrollDragSessionController dragSessionController;

        public ScrollInputHandler(
            IScrollDragSessionController dragSessionController)
        {
            this.dragSessionController = dragSessionController;
        }

        public void HandlePressed(ScrollElementPressedMessage message)
        {
            if (message.Element == null)
            {
                return;
            }

            dragSessionController.TryStartFromScroll(message.Element, message.PointerScreenPosition);
        }
    }
}

using CubeGame.Drag;
using CubeGame.Scroll;

namespace CubeGame.Input
{
    public readonly struct DragSessionReturnedMessage
    {
        public DragSessionReturnedMessage(IScrollElement scrollElement, IDragElement dragElement)
        {
            ScrollElement = scrollElement;
            DragElement = dragElement;
        }

        public IScrollElement ScrollElement { get; }
        public IDragElement DragElement { get; }
    }
}

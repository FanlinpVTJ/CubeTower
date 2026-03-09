using CubeGame.Drag;
using CubeGame.Scroll;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace CubeGame.Input
{
    public sealed class ScrollDragSessionController : IScrollDragSessionController, ITickable
    {
        private const float DEFAULT_DRAG_START_DISTANCE_PIXELS = 35f;
        private const float DEFAULT_SCROLL_VELOCITY_TO_START_DRAG = 150f;

        private readonly IDragElementFactory dragElementFactory;
        private readonly IScrollMovementController scrollMovementController;
        private readonly ScrollRuntimeConfig runtimeConfig;
        private readonly IPublisher<DragSessionStartedMessage> startedPublisher;
        private readonly IPublisher<DragSessionMovedMessage> movedPublisher;
        private readonly IPublisher<DragSessionEndedMessage> endedPublisher;

        private IScrollElement pendingScrollElement;
        private IScrollElement activeScrollElement;
        private ScrollElementData pendingData;
        private IDragElement activeDragElement;
        private Vector2 pressPosition;
        private float startAnimationEndTime;
        private bool isStartAnimationPlaying;

        public ScrollDragSessionController(
            IDragElementFactory dragElementFactory,
            IScrollMovementController scrollMovementController,
            ScrollRuntimeConfig runtimeConfig,
            IPublisher<DragSessionStartedMessage> startedPublisher,
            IPublisher<DragSessionMovedMessage> movedPublisher,
            IPublisher<DragSessionEndedMessage> endedPublisher)
        {
            this.dragElementFactory = dragElementFactory;
            this.scrollMovementController = scrollMovementController;
            this.runtimeConfig = runtimeConfig;
            this.startedPublisher = startedPublisher;
            this.movedPublisher = movedPublisher;
            this.endedPublisher = endedPublisher;
        }

        public void TryStartFromScroll(IScrollElement scrollElement, Vector2 pointerScreenPosition)
        {
            if (scrollElement == null || scrollElement.Data == null || activeDragElement != null)
            {
                return;
            }

            pendingScrollElement = scrollElement;
            pendingData = scrollElement.Data;
            pressPosition = pointerScreenPosition;
        }

        public void Tick()
        {
            if (activeDragElement != null)
            {
                TickDragging();
                return;
            }

            if (pendingData != null)
            {
                TickPending();
            }
        }

        private void TickPending()
        {
            if (!UnityEngine.Input.GetMouseButton(0))
            {
                pendingScrollElement = null;
                pendingData = null;
                return;
            }

            Vector2 currentPointerPosition = (Vector2)UnityEngine.Input.mousePosition;
            float verticalDelta = currentPointerPosition.y - pressPosition.y;
            bool canStartByDistance = verticalDelta >= GetDragStartDistancePixels();
            bool canStartByVelocity = Mathf.Abs(scrollMovementController.CurrentVelocityX) <= GetScrollVelocityToStartDrag();

            if (!canStartByDistance || !canStartByVelocity)
            {
                return;
            }

            activeScrollElement = pendingScrollElement;
            Vector2 startPosition = activeScrollElement.Root.position;
            Vector2 targetPosition = currentPointerPosition + GetDragOffsetUI();
            float animationDuration = GetDragStartAnimationDuration();
            activeDragElement = dragElementFactory.Create(pendingData, startPosition);
            startedPublisher.Publish(new DragSessionStartedMessage(
                activeScrollElement,
                activeDragElement,
                currentPointerPosition,
                startPosition,
                targetPosition,
                animationDuration));
            startAnimationEndTime = Time.unscaledTime + animationDuration;
            isStartAnimationPlaying = true;
            pendingScrollElement = null;
            pendingData = null;
        }

        private void TickDragging()
        {
            if (isStartAnimationPlaying)
            {
                if (Time.unscaledTime < startAnimationEndTime)
                {
                    if (!UnityEngine.Input.GetMouseButtonUp(0))
                    {
                        return;
                    }
                }

                isStartAnimationPlaying = false;
            }

            Vector2 currentPointerPosition = (Vector2)UnityEngine.Input.mousePosition;
            Vector2 targetPosition = currentPointerPosition + GetDragOffsetUI();
            movedPublisher.Publish(new DragSessionMovedMessage(
                activeScrollElement,
                activeDragElement,
                currentPointerPosition,
                targetPosition));

            if (!UnityEngine.Input.GetMouseButtonUp(0))
            {
                return;
            }

            endedPublisher.Publish(new DragSessionEndedMessage(activeScrollElement, activeDragElement, currentPointerPosition));
            activeScrollElement = null;
            activeDragElement = null;
            isStartAnimationPlaying = false;
        }

        private float GetDragStartDistancePixels()
        {
            if (runtimeConfig == null)
            {
                return DEFAULT_DRAG_START_DISTANCE_PIXELS;
            }

            return runtimeConfig.DragStartDistancePixels;
        }

        private Vector2 GetDragOffsetUI()
        {
            if (runtimeConfig == null)
            {
                return Vector2.zero;
            }

            return runtimeConfig.DragOffsetUI;
        }

        private float GetScrollVelocityToStartDrag()
        {
            if (runtimeConfig == null)
            {
                return DEFAULT_SCROLL_VELOCITY_TO_START_DRAG;
            }

            return runtimeConfig.ScrollVelocityToStartDrag;
        }

        private float GetDragStartAnimationDuration()
        {
            if (runtimeConfig == null)
            {
                return 0.12f;
            }

            return runtimeConfig.DragStartAnimationDuration;
        }
    }
}

using System;
using CubeGame.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MessagePipe;
using Zenject;

namespace CubeGame.Scroll
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class ScrollElementBase : MonoBehaviour, IScrollElement, IPointerDownHandler
    {
        [SerializeField] private RectTransform root;
        [SerializeField] private Image viewTarget;
        [Inject(Optional = true)] private IPublisher<ScrollElementPressedMessage> pressedPublisher;
        [Inject(Optional = true)] private ISubscriber<DragSessionStartedMessage> dragStartedSubscriber;
        [Inject(Optional = true)] private ISubscriber<DragSessionMovedMessage> dragMovedSubscriber;
        [Inject(Optional = true)] private ISubscriber<DragSessionEndedMessage> dragEndedSubscriber;

        private IDisposable dragStartedSubscription;
        private IDisposable dragMovedSubscription;
        private IDisposable dragEndedSubscription;

        public RectTransform Root => root != null ? root : (RectTransform)transform;
        public ScrollElementData Data { get; private set; }
        public string ElementId => Data != null ? Data.ElementId : string.Empty;

        public virtual void Initialize(ScrollElementData data)
        {
            Data = data;

            if (data == null)
            {
                gameObject.name = "ScrollElement_Empty";

                if (viewTarget != null)
                {
                    viewTarget.sprite = null;
                }

                return;
            }

            gameObject.name = $"ScrollElement_{data.ElementId}";

            if (viewTarget != null)
            {
                viewTarget.sprite = data.ElementView;
            }
        }

        public virtual void OnDragStart(Vector2 pointerScreenPosition)
        {
        }

        public virtual void OnDrag(Vector2 pointerScreenPosition)
        {
        }

        public virtual void OnDragEnd(Vector2 pointerScreenPosition)
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Data == null || pressedPublisher == null)
            {
                return;
            }

            pressedPublisher.Publish(new ScrollElementPressedMessage(this, eventData.position));
        }

        protected virtual void OnEnable()
        {
            if (dragStartedSubscriber != null)
            {
                dragStartedSubscription = dragStartedSubscriber.Subscribe(OnDragSessionStarted);
            }

            if (dragMovedSubscriber != null)
            {
                dragMovedSubscription = dragMovedSubscriber.Subscribe(OnDragSessionMoved);
            }

            if (dragEndedSubscriber != null)
            {
                dragEndedSubscription = dragEndedSubscriber.Subscribe(OnDragSessionEnded);
            }
        }

        protected virtual void OnDisable()
        {
            dragStartedSubscription?.Dispose();
            dragStartedSubscription = null;
            dragMovedSubscription?.Dispose();
            dragMovedSubscription = null;
            dragEndedSubscription?.Dispose();
            dragEndedSubscription = null;
        }

        protected virtual void Reset()
        {
            root = (RectTransform)transform;
            viewTarget = GetComponent<Image>();
        }

        protected virtual void OnValidate()
        {
            if (root == null)
            {
                root = (RectTransform)transform;
            }

            if (viewTarget == null)
            {
                viewTarget = GetComponent<Image>();
            }
        }

        private void OnDragSessionStarted(DragSessionStartedMessage message)
        {
            if (message.ScrollElement != this)
            {
                return;
            }

            OnDragStart(message.PointerScreenPosition);
        }

        private void OnDragSessionMoved(DragSessionMovedMessage message)
        {
            if (message.ScrollElement != this)
            {
                return;
            }

            OnDrag(message.PointerScreenPosition);
        }

        private void OnDragSessionEnded(DragSessionEndedMessage message)
        {
            if (message.ScrollElement != this)
            {
                return;
            }

            OnDragEnd(message.PointerScreenPosition);
        }
    }
}

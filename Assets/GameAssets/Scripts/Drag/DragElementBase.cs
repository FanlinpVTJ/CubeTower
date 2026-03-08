using System;
using CubeGame.Input;
using CubeGame.Scroll;
using MessagePipe;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CubeGame.Drag
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class DragElementBase : MonoBehaviour, IDragElement
    {
        [SerializeField] private RectTransform root;
        [SerializeField] private Image viewTarget;
        [Inject(Optional = true)] private ISubscriber<DragSessionStartedMessage> dragStartedSubscriber;
        [Inject(Optional = true)] private ISubscriber<DragSessionMovedMessage> dragMovedSubscriber;
        [Inject(Optional = true)] private ISubscriber<DragSessionEndedMessage> dragEndedSubscriber;

        private IDisposable dragStartedSubscription;
        private IDisposable dragMovedSubscription;
        private IDisposable dragEndedSubscription;

        public RectTransform Root => root != null ? root : (RectTransform)transform;
        public ScrollElementData Data { get; private set; }

        public virtual void Initialize(ScrollElementData data)
        {
            Data = data;
            gameObject.name = data != null ? $"DragElement_{data.ElementId}" : "DragElement_Empty";

            if (viewTarget != null)
            {
                viewTarget.sprite = data != null ? data.ElementView : null;
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
            if (message.DragElement != this)
            {
                return;
            }

            OnDragStart(message.PointerScreenPosition);
        }

        private void OnDragSessionMoved(DragSessionMovedMessage message)
        {
            if (message.DragElement != this)
            {
                return;
            }

            Root.position = message.TargetPosition;
            OnDrag(message.PointerScreenPosition);
        }

        private void OnDragSessionEnded(DragSessionEndedMessage message)
        {
            if (message.DragElement != this)
            {
                return;
            }

            OnDragEnd(message.PointerScreenPosition);
        }
    }
}

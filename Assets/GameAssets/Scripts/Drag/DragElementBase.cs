using System;
using CubeGame.Input;
using CubeGame.Scroll;
using CubeGame.Tower;
using MessagePipe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace CubeGame.Drag
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class DragElementBase : MonoBehaviour, IDragElement, IPointerDownHandler
    {
        [SerializeField] private RectTransform root;
        [SerializeField] private Image viewTarget;
        [Inject(Optional = true)] private IPublisher<DragElementPressedMessage> pressedPublisher;
        [Inject(Optional = true)] private ISubscriber<DragSessionStartedMessage> dragStartedSubscriber;
        [Inject(Optional = true)] private ISubscriber<DragSessionMovedMessage> dragMovedSubscriber;
        [Inject(Optional = true)] private ISubscriber<DragSessionEndedMessage> dragEndedSubscriber;
        [Inject(Optional = true)] private ISubscriber<DragSessionCancelledMessage> dragCancelledSubscriber;
        [Inject(Optional = true)] private ISubscriber<DragSessionPlacedMessage> dragPlacedSubscriber;
        [Inject(Optional = true)] private ISubscriber<DragSessionDisposalStartedMessage> dragDisposalStartedSubscriber;
        [Inject(Optional = true)] private ISubscriber<TowerBlockShiftedMessage> towerBlockShiftedSubscriber;

        private IDisposable dragStartedSubscription;
        private IDisposable dragMovedSubscription;
        private IDisposable dragEndedSubscription;
        private IDisposable dragCancelledSubscription;
        private IDisposable dragPlacedSubscription;
        private IDisposable dragDisposalStartedSubscription;
        private IDisposable towerBlockShiftedSubscription;

        public RectTransform Root => root != null ? root : (RectTransform)transform;
        protected Image ViewTarget => viewTarget;
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

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Data == null || pressedPublisher == null)
            {
                return;
            }

            pressedPublisher.Publish(new DragElementPressedMessage(this, eventData.position));
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

            if (dragCancelledSubscriber != null)
            {
                dragCancelledSubscription = dragCancelledSubscriber.Subscribe(OnDragSessionCancelled);
            }

            if (dragPlacedSubscriber != null)
            {
                dragPlacedSubscription = dragPlacedSubscriber.Subscribe(OnDragSessionPlaced);
            }

            if (dragDisposalStartedSubscriber != null)
            {
                dragDisposalStartedSubscription = dragDisposalStartedSubscriber.Subscribe(OnDragSessionDisposalStarted);
            }

            if (towerBlockShiftedSubscriber != null)
            {
                towerBlockShiftedSubscription = towerBlockShiftedSubscriber.Subscribe(OnTowerBlockShifted);
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
            dragCancelledSubscription?.Dispose();
            dragCancelledSubscription = null;
            dragPlacedSubscription?.Dispose();
            dragPlacedSubscription = null;
            dragDisposalStartedSubscription?.Dispose();
            dragDisposalStartedSubscription = null;
            towerBlockShiftedSubscription?.Dispose();
            towerBlockShiftedSubscription = null;
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

            HandleDragSessionStarted(message);
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

        private void OnDragSessionCancelled(DragSessionCancelledMessage message)
        {
            if (message.DragElement != this)
            {
                return;
            }

            HandleDragSessionCancelled(message);
        }

        private void OnDragSessionPlaced(DragSessionPlacedMessage message)
        {
            if (message.DragElement != this)
            {
                return;
            }

            HandleDragSessionPlaced(message);
        }

        private void OnDragSessionDisposalStarted(DragSessionDisposalStartedMessage message)
        {
            if (message.DragElement != this)
            {
                return;
            }

            HandleDragSessionDisposalStarted(message);
        }

        private void OnTowerBlockShifted(TowerBlockShiftedMessage message)
        {
            if (message.DragElement != this)
            {
                return;
            }

            HandleTowerBlockShifted(message);
        }

        protected virtual void HandleDragSessionStarted(DragSessionStartedMessage message)
        {
        }

        protected virtual void HandleDragSessionCancelled(DragSessionCancelledMessage message)
        {
        }

        protected virtual void HandleDragSessionPlaced(DragSessionPlacedMessage message)
        {
        }

        protected virtual void HandleDragSessionDisposalStarted(DragSessionDisposalStartedMessage message)
        {
        }

        protected virtual void HandleTowerBlockShifted(TowerBlockShiftedMessage message)
        {
        }
    }
}

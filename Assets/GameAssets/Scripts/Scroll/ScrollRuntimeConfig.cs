using DG.Tweening;
using UnityEngine;

namespace CubeGame.Scroll
{
    [CreateAssetMenu(menuName = "CubeGame/Scroll/Runtime Config", fileName = "ScrollRuntimeConfig")]
    public sealed class ScrollRuntimeConfig : ScriptableObject
    {
        [Header("Drag Conditions")]
        [SerializeField] private float dragStartDistancePixels = 35f;
        [SerializeField] private float scrollVelocityToStartDrag = 150f;
        [SerializeField] private Vector2 dragOffsetUI;

        [Header("Drag Timing")]
        [SerializeField] private float dragStartAnimationDuration = 0.12f;
        [SerializeField] private float dragCancelAnimationDuration = 0.18f;

        [Header("Drag Start Animation")]
        [SerializeField] private float dragStartScaleFrom = 0.92f;
        [SerializeField] private float dragStartScaleDuration = 0.2f;
        [SerializeField] private Ease dragStartScaleEase = Ease.OutBack;
        [SerializeField] private Ease dragStartMoveEase = Ease.OutQuad;

        [Header("Drag Cancel Animation")]
        [SerializeField] private Ease dragCancelMoveEase = Ease.InQuad;

        [Header("Scroll Element Show Animation")]
        [SerializeField] private float scrollElementShowScaleFrom = 0.92f;
        [SerializeField] private float scrollElementShowScaleDuration = 0.2f;
        [SerializeField] private Ease scrollElementShowScaleEase = Ease.OutBack;

        public float DragStartDistancePixels => dragStartDistancePixels;
        public float ScrollVelocityToStartDrag => scrollVelocityToStartDrag;
        public Vector2 DragOffsetUI => dragOffsetUI;
        public float DragStartAnimationDuration => dragStartAnimationDuration;
        public float DragCancelAnimationDuration => dragCancelAnimationDuration;
        public float DragStartScaleFrom => dragStartScaleFrom;
        public float DragStartScaleDuration => dragStartScaleDuration;
        public Ease DragStartScaleEase => dragStartScaleEase;
        public Ease DragStartMoveEase => dragStartMoveEase;
        public Ease DragCancelMoveEase => dragCancelMoveEase;
        public float ScrollElementShowScaleFrom => scrollElementShowScaleFrom;
        public float ScrollElementShowScaleDuration => scrollElementShowScaleDuration;
        public Ease ScrollElementShowScaleEase => scrollElementShowScaleEase;
    }
}

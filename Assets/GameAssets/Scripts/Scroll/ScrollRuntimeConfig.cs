using UnityEngine;

namespace CubeGame.Scroll
{
    [CreateAssetMenu(menuName = "CubeGame/Scroll/Runtime Config", fileName = "ScrollRuntimeConfig")]
    public sealed class ScrollRuntimeConfig : ScriptableObject
    {
        [SerializeField] private float dragStartDistancePixels = 35f;
        [SerializeField] private float scrollVelocityToStartDrag = 150f;
        [SerializeField] private Vector2 dragOffsetUI;
        [SerializeField] private float dragStartAnimationDuration = 0.12f;
        [SerializeField] private float dragCancelAnimationDuration = 0.18f;

        public float DragStartDistancePixels => dragStartDistancePixels;
        public float ScrollVelocityToStartDrag => scrollVelocityToStartDrag;
        public Vector2 DragOffsetUI => dragOffsetUI;
        public float DragStartAnimationDuration => dragStartAnimationDuration;
        public float DragCancelAnimationDuration => dragCancelAnimationDuration;
    }
}

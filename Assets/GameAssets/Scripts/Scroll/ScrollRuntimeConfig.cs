using UnityEngine;

namespace CubeGame.Scroll
{
    [CreateAssetMenu(menuName = "CubeGame/Scroll/Runtime Config", fileName = "ScrollRuntimeConfig")]
    public sealed class ScrollRuntimeConfig : ScriptableObject
    {
        [SerializeField] private float dragStartDistancePixels = 35f;
        [SerializeField] private float scrollVelocityToStartDrag = 150f;
        [SerializeField] private Vector2 dragOffsetUI;

        public float DragStartDistancePixels => dragStartDistancePixels;
        public float ScrollVelocityToStartDrag => scrollVelocityToStartDrag;
        public Vector2 DragOffsetUI => dragOffsetUI;
    }
}

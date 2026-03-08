using System.Collections.Generic;
using UnityEngine;

namespace CubeGame.Scroll
{
    [CreateAssetMenu(menuName = "CubeGame/Scroll/Feed Config", fileName = "ScrollFeedConfig")]
    public sealed class ScrollFeedConfig : ScriptableObject
    {
        [SerializeField] private List<ScrollElementData> initialElements = new List<ScrollElementData>();

        public IReadOnlyList<ScrollElementData> InitialElements => initialElements;
    }
}

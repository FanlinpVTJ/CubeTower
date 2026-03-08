using UnityEngine;

namespace CubeGame.Tower
{
    [CreateAssetMenu(menuName = "CubeGame/Tower/Config", fileName = "TowerConfig")]
    public sealed class TowerConfig : ScriptableObject
    {
        [SerializeField] private float maxHorizontalOffsetFactor = 0.5f;
        [SerializeField] private string blockPlacedText = "Block placed";
        [SerializeField] private string blockMissedText = "Missed";
        [SerializeField] private string heightLimitReachedText = "Height limit reached";

        public float MaxHorizontalOffsetFactor => maxHorizontalOffsetFactor;
        public string BlockPlacedText => blockPlacedText;
        public string BlockMissedText => blockMissedText;
        public string HeightLimitReachedText => heightLimitReachedText;
    }
}

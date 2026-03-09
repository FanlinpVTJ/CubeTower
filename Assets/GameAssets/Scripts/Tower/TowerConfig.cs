using DG.Tweening;
using UnityEngine;

namespace CubeGame.Tower
{
    [CreateAssetMenu(menuName = "CubeGame/Tower/Config", fileName = "TowerConfig")]
    public sealed class TowerConfig : ScriptableObject
    {
        [Header("Placement Rules")]
        [SerializeField] private float maxHorizontalOffsetFactor = 0.5f;
        [SerializeField] private float firstBlockCenterBias = 0.2f;

        [Header("Placement Animation")]
        [SerializeField] private float blockPlacedAnimationDuration = 0.18f;
        [SerializeField] private float blockPlacedScaleFrom = 0.92f;
        [SerializeField] private float blockPlacedHolderOffsetY = 36f;
        [SerializeField] private Ease blockPlacedMoveEase = Ease.OutQuad;
        [SerializeField] private Ease blockPlacedScaleEase = Ease.OutBack;
        [SerializeField] private Ease blockPlacedHolderEase = Ease.OutBounce;

        [Header("Tower Shift Animation")]
        [SerializeField] private float towerShiftAnimationDuration = 0.2f;
        [SerializeField] private float towerShiftStepDelay = 0.04f;
        [SerializeField] private Ease towerShiftEase = Ease.OutQuad;

        [Header("Feedback Keys")]
        [SerializeField] private string blockPlacedText = "Block placed";
        [SerializeField] private string blockRemovedText = "Block removed";
        [SerializeField] private string blockReturnedText = "Block returned";
        [SerializeField] private string blockMissedText = "Missed";
        [SerializeField] private string heightLimitReachedText = "Height limit reached";

        public float MaxHorizontalOffsetFactor => maxHorizontalOffsetFactor;
        public float FirstBlockCenterBias => firstBlockCenterBias;
        public float BlockPlacedAnimationDuration => blockPlacedAnimationDuration;
        public float BlockPlacedScaleFrom => blockPlacedScaleFrom;
        public float BlockPlacedHolderOffsetY => blockPlacedHolderOffsetY;
        public Ease BlockPlacedMoveEase => blockPlacedMoveEase;
        public Ease BlockPlacedScaleEase => blockPlacedScaleEase;
        public Ease BlockPlacedHolderEase => blockPlacedHolderEase;
        public float TowerShiftAnimationDuration => towerShiftAnimationDuration;
        public float TowerShiftStepDelay => towerShiftStepDelay;
        public Ease TowerShiftEase => towerShiftEase;
        public string BlockPlacedText => blockPlacedText;
        public string BlockRemovedText => blockRemovedText;
        public string BlockReturnedText => blockReturnedText;
        public string BlockMissedText => blockMissedText;
        public string HeightLimitReachedText => heightLimitReachedText;
    }
}

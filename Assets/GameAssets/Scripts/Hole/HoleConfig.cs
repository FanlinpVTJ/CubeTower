using UnityEngine;

namespace CubeGame.Hole
{
    [CreateAssetMenu(menuName = "CubeGame/Hole/Config", fileName = "HoleConfig")]
    public sealed class HoleConfig : ScriptableObject
    {
        [SerializeField] private float disposeAnimationDuration = 0.22f;
        [SerializeField] private float towerShiftAnimationDuration = 0.2f;

        public float DisposeAnimationDuration => disposeAnimationDuration;
        public float TowerShiftAnimationDuration => towerShiftAnimationDuration;
    }
}

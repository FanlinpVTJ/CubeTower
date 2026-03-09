using DG.Tweening;
using UnityEngine;

namespace CubeGame.Hole
{
    [CreateAssetMenu(menuName = "CubeGame/Hole/Config", fileName = "HoleConfig")]
    public sealed class HoleConfig : ScriptableObject
    {
        [Header("Dispose Animation")]
        [SerializeField] private float disposeAnimationDuration = 0.22f;
        [SerializeField] private Ease disposeMoveEase = Ease.InQuad;
        [SerializeField] private Ease disposeScaleEase = Ease.InBack;

        public float DisposeAnimationDuration => disposeAnimationDuration;
        public Ease DisposeMoveEase => disposeMoveEase;
        public Ease DisposeScaleEase => disposeScaleEase;
    }
}

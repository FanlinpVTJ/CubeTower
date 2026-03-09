using System;
using System.Collections.Generic;

namespace CubeGame.Tower
{
    [Serializable]
    public sealed class TowerSnapshot
    {
        public TowerSnapshot()
        {
            blocks = new List<TowerSnapshotBlock>();
        }

        public TowerSnapshot(List<TowerSnapshotBlock> blocks, bool isHeightLimitReached)
        {
            this.blocks = blocks;
            this.isHeightLimitReached = isHeightLimitReached;
        }

        [UnityEngine.SerializeField] private List<TowerSnapshotBlock> blocks;
        [UnityEngine.SerializeField] private bool isHeightLimitReached;

        public List<TowerSnapshotBlock> Blocks => blocks;
        public bool IsHeightLimitReached => isHeightLimitReached;
    }
}

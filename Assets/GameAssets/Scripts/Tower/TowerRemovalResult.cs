using System.Collections.Generic;

namespace CubeGame.Tower
{
    public sealed class TowerRemovalResult
    {
        public TowerRemovalResult(bool isRemoved, TowerBlockEntry removedBlock, List<TowerShiftEntry> shiftedBlocks)
        {
            IsRemoved = isRemoved;
            RemovedBlock = removedBlock;
            ShiftedBlocks = shiftedBlocks;
        }

        public bool IsRemoved { get; }
        public TowerBlockEntry RemovedBlock { get; }
        public List<TowerShiftEntry> ShiftedBlocks { get; }
    }
}

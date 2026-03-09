using System.Collections.Generic;

namespace CubeGame.Tower
{
    public sealed class TowerState
    {
        private readonly List<TowerBlockEntry> blocks = new List<TowerBlockEntry>();

        public List<TowerBlockEntry> Blocks => blocks;
        public bool HasBlocks => blocks.Count > 0;
        public bool IsHeightLimitReached { get; private set; }

        public void AddBlock(TowerBlockEntry blockEntry)
        {
            if (blockEntry == null)
            {
                return;
            }

            blocks.Add(blockEntry);
        }

        public TowerBlockEntry GetTopBlock()
        {
            if (blocks.Count == 0)
            {
                return null;
            }

            return blocks[blocks.Count - 1];
        }

        public void MarkHeightLimitReached()
        {
            IsHeightLimitReached = true;
        }

        public void SetHeightLimitReached(bool isHeightLimitReached)
        {
            IsHeightLimitReached = isHeightLimitReached;
        }

        public void Clear()
        {
            blocks.Clear();
            IsHeightLimitReached = false;
        }
    }
}

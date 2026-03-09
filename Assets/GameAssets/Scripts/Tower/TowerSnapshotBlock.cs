using System;
using UnityEngine;

namespace CubeGame.Tower
{
    [Serializable]
    public sealed class TowerSnapshotBlock
    {
        public TowerSnapshotBlock()
        {
        }

        public TowerSnapshotBlock(string elementId, Vector2 position)
        {
            this.elementId = elementId;
            this.position = position;
        }

        [SerializeField] private string elementId;
        [SerializeField] private Vector2 position;

        public string ElementId => elementId;
        public Vector2 Position => position;
    }
}

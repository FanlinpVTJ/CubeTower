using System;
using CubeGame.Tower;
using UnityEngine;

namespace CubeGame.Save
{
    [Serializable]
    public sealed class GameSaveData
    {
        public GameSaveData()
        {
        }

        public GameSaveData(TowerSnapshot towerSnapshot)
        {
            this.towerSnapshot = towerSnapshot;
        }

        [SerializeField] private TowerSnapshot towerSnapshot;

        public TowerSnapshot TowerSnapshot => towerSnapshot;
    }
}

using System.Collections.Generic;

using DIM.DungeonSystem;

namespace DIM.SpawnSystem {
    [System.Serializable]
    public class SpawnableObjectsByLevel<T> {
        public DungeonLevelSO dungeonLevel;
        public List<SpawnableObjectRatio<T>> spawnableObjectRatioList;
    }
}

using System.Collections.Generic;

namespace DungeonGunner
{
    [System.Serializable]
    public class SpawnableObjectsByLevel<T>
    {
        public DungeonLevelSO dungeonLevel;
        public List<SpawnableObjectRatio<T>> spawnableObjectRatioList;
    }
}

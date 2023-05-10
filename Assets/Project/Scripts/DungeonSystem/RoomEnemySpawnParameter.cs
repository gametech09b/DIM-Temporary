using UnityEngine;

namespace DungeonGunner
{
    [System.Serializable]
    public class RoomEnemySpawnParameter
    {
        public DungeonLevelSO dungeonLevel;

        public int minTotalEnemy;

        public int maxTotalEnemy;

        public int minConcurrentEnemy;

        public int maxConcurrentEnemy;

        public int minSpawnInterval;

        public int maxSpawnInterval;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    public static class Settings
    {
        #region Dungeon Builder Settings
        public const int maxDungeonBuildAttempts = 10; // Max number of attempts to build the dungeon
        public const int maxDungeonRoomRebuildAttempts = 1000; // Max number of attempts to rebuild the dungeon for room graph
        #endregion



        #region Room Settings
        public const int maxChildCorridors = 3; // Max number of child corridors per room
        #endregion
    }
}

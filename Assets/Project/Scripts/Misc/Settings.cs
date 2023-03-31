using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    public static class Settings {
        #region Dungeon Builder Settings
        public const int maxDungeonBuildAttempts = 10; // Max number of attempts to build the dungeon
        public const int maxDungeonRoomRebuildAttempts = 1000; // Max number of attempts to rebuild the dungeon for room graph
        #endregion



        #region Room Settings
        public const int maxChildCorridors = 3; // Max number of child corridors per room
        #endregion



        #region Animator Parameters - Player
        public static int AimUp = Animator.StringToHash("aimUp");
        public static int AimUpRight = Animator.StringToHash("aimUpRight");
        public static int AimUpLeft = Animator.StringToHash("aimUpLeft");
        public static int AimRight = Animator.StringToHash("aimRight");
        public static int AimLeft = Animator.StringToHash("aimLeft");
        public static int AimDown = Animator.StringToHash("aimDown");

        public static int IsIdle = Animator.StringToHash("isIdle");
        public static int IsMoving = Animator.StringToHash("isMoving");
        #endregion
    }
}

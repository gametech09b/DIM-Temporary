using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    public static class Settings
    {
        #region Project Settings
        public const string ProjectName = "DungeonGunner";
        #endregion



        #region Units
        public const float PixelPerUnit = 16f;
        public const float TileSizePixel = 16f;
        #endregion



        #region Dungeon Builder Settings
        public const int MaxDungeonBuildAttempts = 10; // max number of attempts to build the dungeon
        public const int MaxDungeonRoomRebuildAttempts = 1000; // max number of attempts to rebuild the dungeon for room graph
        #endregion



        #region Room Settings
        public const float FadeInTime = 0.5f; // time it takes for the room to fade in
        public const int MaxChildCorridors = 3; // max number of child corridors per room
        #endregion



        #region Animator Parameters - Player
        public static float BaseSpeedForPlayer = 8f;

        public static int AimUp = Animator.StringToHash("aimUp");
        public static int AimUpRight = Animator.StringToHash("aimUpRight");
        public static int AimUpLeft = Animator.StringToHash("aimUpLeft");
        public static int AimRight = Animator.StringToHash("aimRight");
        public static int AimDown = Animator.StringToHash("aimDown");
        public static int AimLeft = Animator.StringToHash("aimLeft");

        public static int IsIdle = Animator.StringToHash("isIdle");
        public static int IsMoving = Animator.StringToHash("isMoving");

        public static int RollUp = Animator.StringToHash("rollUp");
        public static int RollRight = Animator.StringToHash("rollRight");
        public static int RollDown = Animator.StringToHash("rollDown");
        public static int RollLeft = Animator.StringToHash("rollLeft");
        #endregion



        #region Animator Parameters - Door
        public static int IsOpen = Animator.StringToHash("isOpen");
        #endregion



        #region GameObject Tags
        public const string PlayerTag = "Player";
        public const string PlayerWeaponTag = "playerWeapon";
        #endregion



        #region Combat Settings
        public const float AimAngleDistance = 3.5f; // distance between aim angles
        #endregion



        #region UI Settings
        public const float AmmoIconSpacing = 4f; // spacing between ammo icons
        public static Color ReloadProgressColor = Color.red;
        public static Color ReloadDoneColor = Color.green;
        #endregion



        #region AStar Pathfinding Settings
        public const int AStarDefaultMovementPenalty = 40;
        #endregion





    }
}

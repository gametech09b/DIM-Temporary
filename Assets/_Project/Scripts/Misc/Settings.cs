using System;
using UnityEngine;

namespace DIM {
    public static class Settings {
        #region Project Settings
        public const string ProjectName = "DIM";
        #endregion



        #region Units
        public const float PixelPerUnit = 16f;
        public const float TileSizePixel = 16f;
        #endregion



        #region Dungeon Builder Settings
        public const int DungeonMaxBuildAttempts = 10; // max number of attempts to build the dungeon
        public const int DungeonMaxRoomRebuildAttempts = 1000; // max number of attempts to rebuild the dungeon for room graph
        #endregion



        #region Room Settings
        public const float RoomFadeInTime = 0.5f; // time it takes for the room to fade in
        public const int RoomMaxChildCorridors = 3; // max number of child corridors per room
        public const float RoomUnlockDoorsDelay = 1f; // delay before unlocking doors
        #endregion



        #region Animator Parameters - Player
        public static float BaseSpeedForPlayerAnimation = 8f;

        public static int AimUp = Animator.StringToHash("aimUp");
        public static int AimUpRight = Animator.StringToHash("aimUpRight");
        public static int AimUpLeft = Animator.StringToHash("aimUpLeft");
        public static int AimRight = Animator.StringToHash("aimRight");
        public static int AimDown = Animator.StringToHash("aimDown");
        public static int AimLeft = Animator.StringToHash("aimLeft");
    

        public static int IsIdle = Animator.StringToHash("isIdle");
        public static int IsDeath = Animator.StringToHash("isDeath");
        public static int IsMoving = Animator.StringToHash("isMoving");

        public static int RollUp = Animator.StringToHash("rollUp");
        public static int RollRight = Animator.StringToHash("rollRight");
        public static int RollDown = Animator.StringToHash("rollDown");
        public static int RollLeft = Animator.StringToHash("rollLeft");
        #endregion



        #region Animator Parameters - Door
        public static int IsOpen = Animator.StringToHash("isOpen");
        #endregion



        #region Animator Parameters - Enemy
        public static float BaseSpeedForEnemyAnimation = 3f;
        #endregion



        #region Animator Parameters - DestroyableItem
        public static int Destroy = Animator.StringToHash("destroy");
        public static String DestroyedState = "Destroyed";
        #endregion



        #region Animator Parameters - MoveableItem
        public static int FlipUp = Animator.StringToHash("flipUp");
        public static int FlipRight = Animator.StringToHash("flipRight");
        public static int FlipDown = Animator.StringToHash("flipDown");
        public static int FlipLeft = Animator.StringToHash("flipLeft");
        #endregion



        #region Animator Parameters - Chest
        public static int Use = Animator.StringToHash("use");
        #endregion



        #region GameObject Tags
        public const string PlayerTag = "Player";
        public const string PlayerWeaponTag = "playerWeapon";
        #endregion



        #region Combat Settings
        public const float AimAngleDistance = 3.5f; // distance between aim angles
        #endregion



        #region UI Settings
        public const float UIAmmoIconSpacing = 4f; // spacing between ammo icons

        public const float UIHeartIconSpacing = 16f;
        #endregion



        #region AStar Pathfinding Settings
        public const int AStarTargetFrameRate = 60;
        public const int AStarDefaultMovementPenalty = 40;
        public const int AStarPreferredPathMovementPenalty = 1;

        public const float AStarPlayerDistanceToRebuildPath = 3f;
        public const float AStarEnemyRebuildCooldown = 2f;
        #endregion



        #region Enemy Settings
        public const int EnemyDefaultHealth = 20;
        #endregion



        #region Contact Damage Settings
        public const float ContactDamageCooldown = 0.5f;
        #endregion



        #region Audio System
        public const float MusicTrackFadeInTime = 0.5f;
        public const float MusicTrackFadeOutTime = 0.5f;
        #endregion



        #region Score System
        public const int ScoreMaxEntries = 10;
        #endregion
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class GameManager : SingletonMonobehaviour<GameManager>
    {
        [Space(10)]
        [Header("Dungeon Levels")]

        [Tooltip("The list of dungeon levels to use for this game")]
        [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

        [Tooltip("The current dungeon level")]
        [SerializeField] private int currentDungeonLevelIndex;

        [HideInInspector] public GameState gameState;



        private void Start()
        {
            gameState = GameState.GAME_STARTED;
        }



        private void Update()
        {
            HandleGameState();

            // FIXME: Development only
            #region DevOnly
            if (Input.GetKeyDown(KeyCode.R))
            {
                gameState = GameState.GAME_STARTED;
            }
            #endregion
        }



        /// <summary>
        /// Handles the game state
        /// </summary>
        private void HandleGameState()
        {
            switch (gameState)
            {
                case GameState.GAME_STARTED:
                    PlayDungeonLevel(currentDungeonLevelIndex);
                    gameState = GameState.PLAYING_LEVEL;

                    break;

                case GameState.PLAYING_LEVEL:
                    break;

                case GameState.ENGAGING_ENEMY:
                    break;

                case GameState.LEVEL_COMPLETED:
                    break;

                case GameState.GAME_WON:
                    break;

                case GameState.GAME_LOST:
                    break;

                case GameState.GAME_PAUSED:
                    break;

                case GameState.DUNGEON_OVERVIEW_MAP:
                    break;

                case GameState.RESTART_GAME:
                    break;

                default:
                    break;
            }
        }



        private void PlayDungeonLevel(int dungeonLevelIndex)
        {
            bool dungeonBuiltSuccessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelIndex]);

            if (!dungeonBuiltSuccessfully)
            {
                Debug.LogError("Couldn't build dungeon from specified rooms and node graphs");
            }
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
        }
#endif
        #endregion
    }
}

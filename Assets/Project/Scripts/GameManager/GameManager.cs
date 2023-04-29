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



        private Room currentRoom;
        private Room previousRoom;
        private PlayerDetailSO currentPlayerDetail;
        private Player currentPlayer;



        [HideInInspector] public GameState gameState;
        [HideInInspector] public GameState previousGameState;



        protected override void Awake()
        {
            base.Awake();

            currentPlayerDetail = GameResources.Instance.CurrentPlayer.playerDetail;

            InstantiatePlayer();
        }



        private void InstantiatePlayer()
        {
            GameObject currentPlayerGameObject = Instantiate(currentPlayerDetail.characterPrefab);

            currentPlayer = currentPlayerGameObject.GetComponent<Player>();
            currentPlayer.Init(currentPlayerDetail);
        }



        private void OnEnable()
        {
            DungeonStaticEvent.OnRoomChange += DungeonStaticEvent_OnRoomChange;
        }



        private void OnDisable()
        {
            DungeonStaticEvent.OnRoomChange -= DungeonStaticEvent_OnRoomChange;
        }



        private void Start()
        {
            previousGameState = GameState.GAME_STARTED;
            gameState = GameState.GAME_STARTED;
        }



        private void Update()
        {
            HandleGameState();

            // FIXME: Development only
            #region DevOnly
            if (Input.GetKeyDown(KeyCode.P))
            {
                gameState = GameState.GAME_STARTED;
            }
            #endregion
        }



        private void DungeonStaticEvent_OnRoomChange(OnRoomChangeEventArgs args)
        {
            SetCurrentRoom(args.room);
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

            DungeonStaticEvent.CallOnRoomChange(currentRoom);

            Vector3 currentRoomMiddlePosition = currentRoom.GetMiddlePosition();
            Vector3 nearestSpawnPoint = HelperUtilities.GetNearestSpawnPoint(currentRoomMiddlePosition);
            currentPlayer.transform.position = nearestSpawnPoint;
        }



        public Room GetCurrentRoom()
        {
            return currentRoom;
        }



        public void SetCurrentRoom(Room room)
        {
            previousRoom = currentRoom;
            currentRoom = room;
        }



        public Player GetCurrentPlayer()
        {
            return currentPlayer;
        }



        public Sprite GetCurrentPlayerMinimapIcon()
        {
            return currentPlayerDetail.minimapIconSprite;
        }



        public DungeonLevelSO GetCurrentDungeonLevel()
        {
            return dungeonLevelList[currentDungeonLevelIndex];
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckEnumerableValue(this, nameof(dungeonLevelList), dungeonLevelList);
        }
#endif
        #endregion
    }
}

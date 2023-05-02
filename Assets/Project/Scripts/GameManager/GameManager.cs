using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private RoomGameObject currentBossRoom;

        [HideInInspector] public GameState gameState;
        [HideInInspector] public GameState previousGameState;
        private long score;
        private int scoreMultiplier;



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
            DungeonStaticEvent.OnRoomChanged += DungeonStaticEvent_OnRoomChange;
            DungeonStaticEvent.OnRoomEnemiesDefeated += DungeonStaticEvent_OnRoomEnemiesDefeated;
            DungeonStaticEvent.OnPointScored += DungeonStaticEvent_OnPointScored;
            DungeonStaticEvent.OnMultiplierChanged += DungeonStaticEvent_OnMultiplierChanged;

            currentPlayer.destroyedEvent.OnDestroyed += CurrentPlayer_DestroyedEvent_OnDestroyed;
        }



        private void OnDisable()
        {
            DungeonStaticEvent.OnRoomChanged -= DungeonStaticEvent_OnRoomChange;
            DungeonStaticEvent.OnRoomEnemiesDefeated -= DungeonStaticEvent_OnRoomEnemiesDefeated;
            DungeonStaticEvent.OnPointScored -= DungeonStaticEvent_OnPointScored;
            DungeonStaticEvent.OnMultiplierChanged -= DungeonStaticEvent_OnMultiplierChanged;

            currentPlayer.destroyedEvent.OnDestroyed -= CurrentPlayer_DestroyedEvent_OnDestroyed;
        }



        private void Start()
        {
            previousGameState = GameState.GAME_STARTED;
            gameState = GameState.GAME_STARTED;

            score = 0;
            scoreMultiplier = 1;
        }



        private void Update()
        {
            HandleGameState();

            // FIXME: Development only
            #region Development Only
            if (Input.GetKeyDown(KeyCode.P))
                gameState = GameState.GAME_STARTED;
            #endregion
        }



        private void DungeonStaticEvent_OnRoomChange(OnRoomChangedEventArgs _args)
        {
            SetCurrentRoom(_args.room);
        }



        private void DungeonStaticEvent_OnRoomEnemiesDefeated(OnRoomEnemiesDefeatedEventArgs _args)
        {
            RoomEnemiesDefeated();
        }



        private void DungeonStaticEvent_OnPointScored(OnPointScoredEventArgs _args)
        {
            score += _args.point * scoreMultiplier;

            DungeonStaticEvent.CallOnScoreChanged(score, scoreMultiplier);
        }



        private void DungeonStaticEvent_OnMultiplierChanged(OnMultiplierChangedEventArgs _args)
        {
            if (_args.isMultiplier)
                scoreMultiplier++;
            else
                scoreMultiplier--;

            scoreMultiplier = Mathf.Clamp(scoreMultiplier, 1, 30);
            DungeonStaticEvent.CallOnScoreChanged(score, scoreMultiplier);
        }



        private void CurrentPlayer_DestroyedEvent_OnDestroyed(DestroyedEvent _sender, OnDestroyedEventArgs _args)
        {
            previousGameState = gameState;
            gameState = GameState.GAME_LOST;
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

                    RoomEnemiesDefeated();
                    break;

                case GameState.PLAYING_LEVEL:
                    break;

                case GameState.ENGAGING_ENEMY:
                    break;

                case GameState.LEVEL_COMPLETED:
                    StartCoroutine(LevelCompletedCoroutine());
                    break;

                case GameState.GAME_WON:
                    if (previousGameState != GameState.GAME_WON)
                        StartCoroutine(GameWonCoroutine());
                    break;

                case GameState.GAME_LOST:
                    if (previousGameState != GameState.GAME_LOST)
                    {
                        StopAllCoroutines();
                        StartCoroutine(GameLostCoroutine());
                    }
                    break;

                case GameState.GAME_PAUSED:
                    break;

                case GameState.DUNGEON_OVERVIEW_MAP:
                    break;

                case GameState.RESTART_GAME:
                    RestartGame();
                    break;

                default:
                    break;
            }
        }



        private void PlayDungeonLevel(int _dungeonLevelIndex)
        {
            bool dungeonBuiltSuccessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[_dungeonLevelIndex]);

            if (!dungeonBuiltSuccessfully)
            {
                Debug.LogError("Couldn't build dungeon from specified rooms and node graphs");
            }

            DungeonStaticEvent.CallOnRoomChanged(currentRoom);

            Vector3 currentRoomMiddlePosition = currentRoom.GetMiddlePosition();
            Vector3 nearestSpawnPoint = HelperUtilities.GetNearestSpawnPoint(currentRoomMiddlePosition);
            currentPlayer.transform.position = nearestSpawnPoint;
        }



        private IEnumerator BeginBossStageCoroutine()
        {
            currentBossRoom.gameObject.SetActive(true);

            currentBossRoom.UnlockDoors(0f);

            yield return new WaitForSeconds(2f);

            Debug.Log("Boss stage started");
        }



        private IEnumerator LevelCompletedCoroutine()
        {
            gameState = GameState.PLAYING_LEVEL;

            yield return new WaitForSeconds(2f);

            Debug.Log("Level completed");

            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null;
            }

            yield return null;

            currentDungeonLevelIndex++;

            PlayDungeonLevel(currentDungeonLevelIndex);
        }



        private IEnumerator GameWonCoroutine()
        {
            previousGameState = GameState.GAME_WON;

            Debug.Log("Game won");
            yield return new WaitForSeconds(10f);

            gameState = GameState.RESTART_GAME;
        }



        private IEnumerator GameLostCoroutine()
        {
            previousGameState = GameState.GAME_LOST;

            Debug.Log("Game lost");
            yield return new WaitForSeconds(10f);

            gameState = GameState.RESTART_GAME;
        }



        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }



        public Room GetCurrentRoom()
        {
            return currentRoom;
        }



        public void SetCurrentRoom(Room _room)
        {
            previousRoom = currentRoom;
            currentRoom = _room;
        }



        private void RoomEnemiesDefeated()
        {
            // TODO: Handle this for Nuradiance later
            bool isDungeonClearFromCommonEnemies = true;
            currentBossRoom = null;

            foreach (KeyValuePair<string, Room> roomDictionaryKVP in DungeonBuilder.Instance.roomDictionary)
            {
                if (roomDictionaryKVP.Value.roomNodeType.isBossRoom)
                {
                    currentBossRoom = roomDictionaryKVP.Value.roomGameObject;
                    continue;
                }

                if (!roomDictionaryKVP.Value.isCleared)
                {
                    isDungeonClearFromCommonEnemies = false;
                    break;
                }
            }

            if ((isDungeonClearFromCommonEnemies && currentBossRoom == null)
            || (isDungeonClearFromCommonEnemies && currentBossRoom.room.isCleared))
            {
                if (currentDungeonLevelIndex < dungeonLevelList.Count - 1)
                    gameState = GameState.LEVEL_COMPLETED;
                else
                    gameState = GameState.GAME_WON;
            }
            else if (isDungeonClearFromCommonEnemies)
            {
                gameState = GameState.BOSS_STAGE;

                StartCoroutine(BeginBossStageCoroutine());
            }
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

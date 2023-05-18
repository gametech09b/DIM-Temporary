using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using DIM.DungeonSystem;
using DIM.EnemySystem;
using DIM.HealthSystem;
using DIM.MapSystem;
using DIM.PlayerSystem;
using DIM.ScoreSystem;

namespace DIM {
    [DisallowMultipleComponent]
    public class GameManager : SingletonMonobehaviour<GameManager> {
        [Space(10)]
        [Header("Message UI")]

        [SerializeField] private TextMeshProUGUI messageTextMP;
        [SerializeField] private CanvasGroup fadeScreenCanvasGroup;
        private bool isFading = false;


        [Space(10)]
        [Header("Pause Menu")]

        [SerializeField] private GameObject pauseMenu;


        [Space(10)]
        [Header("Dungeon Levels")]

        [SerializeField] private List<DungeonLevelSO> dungeonLevelList;
        [SerializeField] private int currentDungeonLevelIndex;


        private Room currentRoom;
        private Room previousRoom;
        private PlayerDetailSO currentPlayerDetail;
        private Player currentPlayer;

        private RoomGameObject currentBossRoom;

        [HideInInspector] public GameState gameState;
        [HideInInspector] public GameState previousGameState;
        private long gameScore;
        private int scoreMultiplier;

        // ===================================================================

        protected override void Awake() {
            base.Awake();

            currentPlayerDetail = GameResources.Instance.CurrentPlayer.playerDetail;

            InstantiatePlayer();
        }



        private void InstantiatePlayer() {
            GameObject currentPlayerGameObject = Instantiate(currentPlayerDetail.characterPrefab);

            currentPlayer = currentPlayerGameObject.GetComponent<Player>();
            currentPlayer.Init(currentPlayerDetail);
        }



        private void OnEnable() {
            DungeonStaticEvent.OnRoomChanged += DungeonStaticEvent_OnRoomChange;
            DungeonStaticEvent.OnRoomEnemiesDefeated += DungeonStaticEvent_OnRoomEnemiesDefeated;
            DungeonStaticEvent.OnPointScored += DungeonStaticEvent_OnPointScored;
            DungeonStaticEvent.OnMultiplierChanged += DungeonStaticEvent_OnMultiplierChanged;

            currentPlayer.destroyedEvent.OnDestroyed += CurrentPlayer_DestroyedEvent_OnDestroyed;
        }



        private void OnDisable() {
            DungeonStaticEvent.OnRoomChanged -= DungeonStaticEvent_OnRoomChange;
            DungeonStaticEvent.OnRoomEnemiesDefeated -= DungeonStaticEvent_OnRoomEnemiesDefeated;
            DungeonStaticEvent.OnPointScored -= DungeonStaticEvent_OnPointScored;
            DungeonStaticEvent.OnMultiplierChanged -= DungeonStaticEvent_OnMultiplierChanged;

            currentPlayer.destroyedEvent.OnDestroyed -= CurrentPlayer_DestroyedEvent_OnDestroyed;
        }



        private void Start() {
            previousGameState = GameState.GAME_STARTED;
            gameState = GameState.GAME_STARTED;

            gameScore = 0;
            scoreMultiplier = 1;

            StartCoroutine(FadeCoroutine(0f, 1f, 0f, Color.black));
        }



        private void Update() {
            HandleGameState();

            // // FIXME: Development only
            // #region Development Only
            // if (Input.GetKeyDown(KeyCode.P))
            //     gameState = GameState.GAME_STARTED;
            // #endregion
        }



        private void DungeonStaticEvent_OnRoomChange(OnRoomChangedEventArgs _args) {
            SetCurrentRoom(_args.room);
        }



        private void DungeonStaticEvent_OnRoomEnemiesDefeated(OnRoomEnemiesDefeatedEventArgs _args) {
            RoomEnemiesDefeated();
        }



        private void DungeonStaticEvent_OnPointScored(OnPointScoredEventArgs _args) {
            gameScore += _args.point * scoreMultiplier;

            DungeonStaticEvent.CallOnScoreChanged(gameScore, scoreMultiplier);
        }



        private void DungeonStaticEvent_OnMultiplierChanged(OnMultiplierChangedEventArgs _args) {
            if (_args.isMultiplier)
                scoreMultiplier++;
            else
                scoreMultiplier--;

            scoreMultiplier = Mathf.Clamp(scoreMultiplier, 1, 30);
            DungeonStaticEvent.CallOnScoreChanged(gameScore, scoreMultiplier);
        }



        private void CurrentPlayer_DestroyedEvent_OnDestroyed(DestroyedEvent _sender, OnDestroyedEventArgs _args) {
            previousGameState = gameState;
            gameState = GameState.GAME_LOST;
        }



        /// <summary>
        /// Handles the game state
        /// </summary>
        private void HandleGameState() {
            switch (gameState) {
                case GameState.GAME_STARTED:
                    if(dungeonLevelList.Count > 0) {
                        PlayDungeonLevel(currentDungeonLevelIndex);
                        gameState = GameState.PLAYING_LEVEL;

                        RoomEnemiesDefeated();
                    }

                    break;

                case GameState.PLAYING_LEVEL:
                    if (Input.GetKeyDown(KeyCode.Tab))
                        DisplayMap();

                    if (Input.GetKeyDown(KeyCode.Escape))
                        PauseGame();
                    break;

                case GameState.ENGAGING_ENEMY:
                    if (Input.GetKeyDown(KeyCode.Escape))
                        PauseGame();
                    break;

                case GameState.BOSS_STAGE:
                    if (Input.GetKeyDown(KeyCode.Tab))
                        DisplayMap();

                    if (Input.GetKeyDown(KeyCode.Escape))
                        PauseGame();
                    break;

                case GameState.ENGAGING_BOSS:
                    if (Input.GetKeyDown(KeyCode.Escape))
                        PauseGame();
                    break;

                case GameState.LEVEL_COMPLETED:
                    StartCoroutine(LevelCompletedCoroutine());
                    break;

                case GameState.GAME_WON:
                    if (previousGameState != GameState.GAME_WON)
                        StartCoroutine(GameWonCoroutine());
                    break;

                case GameState.GAME_LOST:
                    if (previousGameState != GameState.GAME_LOST) {
                        StopAllCoroutines();
                        StartCoroutine(GameLostCoroutine());
                    }
                    break;

                case GameState.GAME_PAUSED:
                    if (Input.GetKeyDown(KeyCode.Escape))
                        PauseGame();
                    break;

                case GameState.DUNGEON_OVERVIEW_MAP:
                    if (Input.GetKeyUp(KeyCode.Tab))
                        Map.Instance.HideMap();

                    break;

                case GameState.RESTART_GAME:
                    RestartGame();
                    break;

                default:
                    break;
            }
        }



        private void PlayDungeonLevel(int _dungeonLevelIndex) {
            bool dungeonBuiltSuccessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[_dungeonLevelIndex]);

            if (!dungeonBuiltSuccessfully) {
                Debug.LogError("Couldn't build dungeon from specified rooms and node graphs");
            }

            DungeonStaticEvent.CallOnRoomChanged(currentRoom);

            Vector3 currentRoomMiddlePosition = currentRoom.GetMiddlePosition();
            Vector3 nearestSpawnPoint = HelperUtilities.GetNearestSpawnPoint(currentRoomMiddlePosition);
            currentPlayer.transform.position = nearestSpawnPoint;

            StartCoroutine(DisplayDungeonLevelTextCoroutine());
        }



        public IEnumerator FadeCoroutine(float _startAlpha, float _targetAlpha, float _fadeDuration, Color _backgroundColor) {
            isFading = true;

            Image image = fadeScreenCanvasGroup.GetComponent<Image>();
            image.color = _backgroundColor;

            float elapsedTime = 0f;

            while (elapsedTime <= _fadeDuration) {
                elapsedTime += Time.deltaTime;

                fadeScreenCanvasGroup.alpha = Mathf.Lerp(_startAlpha, _targetAlpha, (elapsedTime / _fadeDuration));

                yield return null;
            }

            isFading = false;
        }



        private IEnumerator DisplayDungeonLevelTextCoroutine() {
            StartCoroutine(FadeCoroutine(0f, 1f, 0f, Color.black));

            GetCurrentPlayer().controllerHandler.DisableController();

            string message = $"Dungeon Level {currentDungeonLevelIndex + 1}\n\n{dungeonLevelList[currentDungeonLevelIndex].levelName.ToUpper()}";

            yield return StartCoroutine(DisplayMessageCoroutine(message, Color.white, 2f));

            GetCurrentPlayer().controllerHandler.EnableController();

            yield return StartCoroutine(FadeCoroutine(1f, 0f, 2f, Color.black));
        }



        private IEnumerator DisplayMessageCoroutine(string _message, Color _textColor, float _displayDuration) {
            messageTextMP.SetText(_message);
            messageTextMP.color = _textColor;

            if (_displayDuration > 0) {
                float timer = _displayDuration;

                while (timer > 0 && !Input.GetKeyDown(KeyCode.Return)) {
                    timer -= Time.deltaTime;
                    yield return null;
                }
            } else {
                while (!Input.GetKeyDown(KeyCode.Return)) {
                    yield return null;
                }
            }

            messageTextMP.SetText("");
        }



        private IEnumerator BeginBossStageCoroutine() {
            currentBossRoom.gameObject.SetActive(true);

            currentBossRoom.UnlockDoors(0f);

            yield return new WaitForSeconds(2f);

            yield return StartCoroutine(FadeCoroutine(0f, 1f, 2f, new Color(0f, 0f, 0f, 0.4f)));

            yield return StartCoroutine(DisplayMessageCoroutine($"WELL DONE {GameResources.Instance.CurrentPlayer.playerName}! YOU'VE SURVIVED.... SO FAR\n\nNOW FIND AND DEFEAT THE BOSS.... GOOD LUCK!", Color.white, 5f));

            yield return StartCoroutine(FadeCoroutine(1f, 0f, 2f, new Color(0f, 0f, 0f, 0.4f)));
        }



        private IEnumerator LevelCompletedCoroutine() {
            gameState = GameState.PLAYING_LEVEL;

            yield return new WaitForSeconds(2f);

            yield return StartCoroutine(FadeCoroutine(0f, 1f, 2f, new Color(0f, 0f, 0f, 0.4f)));

            yield return StartCoroutine(DisplayMessageCoroutine($"WELL DONE {GameResources.Instance.CurrentPlayer.playerName}! YOU'VE SURVIVED THIS DUNGEON LEVEL!", Color.white, 5f));
            yield return StartCoroutine(DisplayMessageCoroutine($"COLLECT ANY LOOT.... THEN PRESS RETURN\n\nTO DESCEND FURTHER INTO THE DUNGEON", Color.white, 5f));

            while (!Input.GetKeyDown(KeyCode.Return)) {
                yield return null;
            }

            yield return null;

            currentDungeonLevelIndex++;

            PlayDungeonLevel(currentDungeonLevelIndex);
        }



        private IEnumerator GameWonCoroutine() {
            previousGameState = GameState.GAME_WON;

            GetCurrentPlayer().controllerHandler.DisableController();

            int rank = HighScoreManager.Instance.GetRank(gameScore);

            string rankText;

            if (rank > 0
            && rank <= Settings.ScoreMaxEntries) {
                rankText = $"YOUR SCORE IS RANKED {rank.ToString("#0")} IN THE TOP {Settings.ScoreMaxEntries.ToString("#0")} SCORES";
                string name = GameResources.Instance.CurrentPlayer.playerName;

                if (name == "")
                    name = "UNKNOWN";

                HighScoreManager.Instance.AddScore(new Score(name, $"LEVEL {(currentDungeonLevelIndex + 1).ToString("#0")} - {GetCurrentDungeonLevel().levelName.ToUpper()}", gameScore));
            } else {
                rankText = $"YOUR SCORE IS NOT RANKED IN THE TOP {Settings.ScoreMaxEntries.ToString("#0")} SCORES";
            }

            yield return StartCoroutine(FadeCoroutine(0f, 1f, 2f, Color.black));

            yield return StartCoroutine(DisplayMessageCoroutine($"WELL DONE {GameResources.Instance.CurrentPlayer.playerName}! YOU'VE DEFEATED ALL DUNGEONS!", Color.white, 3f));
            yield return StartCoroutine(DisplayMessageCoroutine($"YOU SCORED {gameScore.ToString("###,###0")} POINTS\n\n{rankText}", Color.white, 4f));
            yield return StartCoroutine(DisplayMessageCoroutine($"PRESS RETURN TO RESTART THE GAME", Color.white, 0f));

            gameState = GameState.RESTART_GAME;
        }



        private IEnumerator GameLostCoroutine() {
            previousGameState = GameState.GAME_LOST;

            GetCurrentPlayer().controllerHandler.DisableController();

            int rank = HighScoreManager.Instance.GetRank(gameScore);

            string rankText;

            if (rank > 0
            && rank <= Settings.ScoreMaxEntries) {
                rankText = $"YOUR SCORE IS RANKED {rank.ToString("#0")} IN THE TOP {Settings.ScoreMaxEntries.ToString("#0")} SCORES";
                string name = GameResources.Instance.CurrentPlayer.playerName;

                if (name == "")
                    name = "UNKNOWN";

                HighScoreManager.Instance.AddScore(new Score(name, $"LEVEL {(currentDungeonLevelIndex + 1).ToString("#0")} - {GetCurrentDungeonLevel().levelName.ToUpper()}", gameScore));
            } else {
                rankText = $"YOUR SCORE IS NOT RANKED IN THE TOP {Settings.ScoreMaxEntries.ToString("#0")} SCORES";
            }

            yield return new WaitForSeconds(1f);

            yield return StartCoroutine(FadeCoroutine(0f, 1f, 2f, Color.black));

            Enemy[] enemyArray = GameObject.FindObjectsOfType<Enemy>();

            foreach (Enemy enemy in enemyArray) {
                enemy.gameObject.SetActive(false);
            }

            yield return StartCoroutine(DisplayMessageCoroutine($"BAD LUCK {GameResources.Instance.CurrentPlayer.playerName}! YOU'VE BEEN DEFEATED!", Color.white, 3f));
            yield return StartCoroutine(DisplayMessageCoroutine($"YOU SCORED {gameScore.ToString("###,###0")} POINTS\n\n{rankText}", Color.white, 4f));
            yield return StartCoroutine(DisplayMessageCoroutine($"PRESS RETURN TO RESTART THE GAME", Color.white, 0f));

            gameState = GameState.RESTART_GAME;
        }



        private void RestartGame() {
            SceneManager.LoadScene("MainMenuScene");
        }



        public Room GetCurrentRoom() {
            return currentRoom;
        }



        public void SetCurrentRoom(Room _room) {
            previousRoom = currentRoom;
            currentRoom = _room;
        }



        private void RoomEnemiesDefeated() {
            // TODO: Handle this for Nuradiance later
            bool isDungeonClearFromCommonEnemies = true;
            currentBossRoom = null;

            foreach (KeyValuePair<string, Room> roomDictionaryKVP in DungeonBuilder.Instance.roomDictionary) {
                if (roomDictionaryKVP.Value.roomNodeType.isBossRoom) {
                    currentBossRoom = roomDictionaryKVP.Value.roomGameObject;
                    continue;
                }

                if (!roomDictionaryKVP.Value.isCleared) {
                    isDungeonClearFromCommonEnemies = false;
                    break;
                }
            }

            if ((isDungeonClearFromCommonEnemies && currentBossRoom == null)
            || (isDungeonClearFromCommonEnemies && currentBossRoom.room.isCleared)) {
                if (currentDungeonLevelIndex < dungeonLevelList.Count - 1)
                    gameState = GameState.LEVEL_COMPLETED;
                else
                    gameState = GameState.GAME_WON;
            } else if (isDungeonClearFromCommonEnemies) {
                gameState = GameState.BOSS_STAGE;

                StartCoroutine(BeginBossStageCoroutine());
            }
        }



        public void PauseGame() {
            if (gameState != GameState.GAME_PAUSED) {
                pauseMenu.SetActive(true);
                GetCurrentPlayer().controllerHandler.DisableController();

                previousGameState = gameState;
                gameState = GameState.GAME_PAUSED;
            } else
            if (gameState == GameState.GAME_PAUSED) {
                pauseMenu.SetActive(false);
                GetCurrentPlayer().controllerHandler.EnableController();

                gameState = previousGameState;
                previousGameState = GameState.GAME_PAUSED;
            }
        }



        private void DisplayMap() {
            if (isFading)
                return;

            Map.Instance.DisplayMap();
        }



        public Player GetCurrentPlayer() {
            return currentPlayer;
        }



        public Sprite GetCurrentPlayerMinimapIcon() {
            return currentPlayerDetail.minimapIconSprite;
        }



        public DungeonLevelSO GetCurrentDungeonLevel() {
            return dungeonLevelList[currentDungeonLevelIndex];
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckNullValue(this, nameof(pauseMenu), pauseMenu);
            HelperUtilities.CheckNullValue(this, nameof(messageTextMP), messageTextMP);
            HelperUtilities.CheckNullValue(this, nameof(fadeScreenCanvasGroup), fadeScreenCanvasGroup);

            HelperUtilities.CheckEnumerableValue(this, nameof(dungeonLevelList), dungeonLevelList);
        }
#endif
        #endregion
    }
}

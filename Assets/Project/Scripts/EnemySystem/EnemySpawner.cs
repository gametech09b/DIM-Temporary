using System.Collections;
using UnityEngine;

using DIM.DungeonSystem;
using DIM.AudioSystem;
using DIM.SpawnSystem;
using DIM.HealthSystem;

namespace DIM.EnemySystem {
    [DisallowMultipleComponent]
    public class EnemySpawner : SingletonMonobehaviour<EnemySpawner> {
        private int totalToSpawn;
        private int currentCount;
        private int spawnedCount;
        private int maxConcurrentSpawnCount;

        private Room currentRoom;
        private RoomEnemySpawnParameter currentRoomEnemySpawnParameter;

        // ===================================================================

        private void OnEnable() {
            DungeonStaticEvent.OnRoomChanged += DungeonStaticEvent_OnRoomChange;
        }



        private void OnDisable() {
            DungeonStaticEvent.OnRoomChanged -= DungeonStaticEvent_OnRoomChange;
        }



        private void DungeonStaticEvent_OnRoomChange(OnRoomChangedEventArgs _args) {
            currentCount = 0;
            spawnedCount = 0;

            currentRoom = _args.room;

            MusicManager.Instance.PlayMusic(currentRoom.ambientMusicTrack, 0.2f, 2f);

            if (currentRoom.roomNodeType.isCorridorEW
            || currentRoom.roomNodeType.isCorridorNS
            || currentRoom.roomNodeType.isEntrance)
                return;

            if (currentRoom.isCleared)
                return;

            totalToSpawn = currentRoom.GetNumberOfEnemyToSpawn(GameManager.Instance.GetCurrentDungeonLevel());

            currentRoomEnemySpawnParameter = currentRoom.GetRoomEnemySpawnParameter(GameManager.Instance.GetCurrentDungeonLevel());

            if (totalToSpawn == 0) {
                currentRoom.isCleared = true;
                return;
            }

            maxConcurrentSpawnCount = GetConcurrentSpawnCount();

            MusicManager.Instance.PlayMusic(currentRoom.battleMusicTrack, 0.2f, 0.5f);

            currentRoom.roomGameObject.LockDoors();

            SpawnEnemy();
        }



        private void SpawnEnemy() {
            if (GameManager.Instance.gameState == GameState.BOSS_STAGE) {
                GameManager.Instance.previousGameState = GameState.BOSS_STAGE;
                GameManager.Instance.gameState = GameState.ENGAGING_BOSS;
            } else if (GameManager.Instance.gameState == GameState.PLAYING_LEVEL) {
                GameManager.Instance.previousGameState = GameState.PLAYING_LEVEL;
                GameManager.Instance.gameState = GameState.ENGAGING_ENEMY;
            }

            StartCoroutine(SpawnEnemyCoroutine());
        }



        private IEnumerator SpawnEnemyCoroutine() {
            Grid grid = currentRoom.GetGrid();

            RandomSpawnableObject<EnemyDetailSO> randomSpawnableObject = new RandomSpawnableObject<EnemyDetailSO>(currentRoom.enemySpawnByLevelList);

            if (currentRoom.spawnPositionArray.Length > 0) {
                for (int i = 0; i < totalToSpawn; i++) {
                    while (currentCount >= maxConcurrentSpawnCount) {
                        yield return null;
                    }

                    Vector3Int cellPosition = (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Length)];

                    CreateEnemy(randomSpawnableObject.GetObject(), grid.CellToWorld(cellPosition));

                    yield return new WaitForSeconds(GetSpawnInterval());
                }
            }
        }



        private float GetSpawnInterval() {
            return Random.Range(currentRoomEnemySpawnParameter.minSpawnInterval, currentRoomEnemySpawnParameter.maxSpawnInterval);
        }



        private int GetConcurrentSpawnCount() {
            return Random.Range(currentRoomEnemySpawnParameter.minConcurrentEnemy, currentRoomEnemySpawnParameter.maxConcurrentEnemy);
        }



        private void CreateEnemy(EnemyDetailSO _enemyDetail, Vector3 _position) {
            currentCount++;
            spawnedCount++;

            DungeonLevelSO dungeonLevel = GameManager.Instance.GetCurrentDungeonLevel();

            GameObject enemyGameObject = Instantiate(_enemyDetail.prefab, _position, Quaternion.identity);

            Enemy enemy = enemyGameObject.GetComponent<Enemy>();

            enemy.Init(_enemyDetail, spawnedCount, dungeonLevel);

            enemy.GetComponent<DestroyedEvent>().OnDestroyed += Enemy_DestroyedEvent_OnDestroyed;
        }



        private void Enemy_DestroyedEvent_OnDestroyed(DestroyedEvent _sender, OnDestroyedEventArgs _args) {
            _sender.OnDestroyed -= Enemy_DestroyedEvent_OnDestroyed;

            currentCount--;

            DungeonStaticEvent.CallOnPointScored(_args.point);

            if (currentCount <= 0
            && spawnedCount == totalToSpawn) {
                currentRoom.isCleared = true;

                if (GameManager.Instance.gameState == GameState.ENGAGING_ENEMY) {
                    GameManager.Instance.previousGameState = GameState.ENGAGING_ENEMY;
                    GameManager.Instance.gameState = GameState.PLAYING_LEVEL;
                } else if (GameManager.Instance.gameState == GameState.ENGAGING_BOSS) {
                    GameManager.Instance.previousGameState = GameState.ENGAGING_BOSS;
                    GameManager.Instance.gameState = GameState.BOSS_STAGE;
                }

                currentRoom.roomGameObject.UnlockDoors(Settings.RoomUnlockDoorsDelay);

                MusicManager.Instance.PlayMusic(currentRoom.ambientMusicTrack, 0.2f, 2f);

                DungeonStaticEvent.CallOnRoomEnemiesDefeated(currentRoom);
            }
        }
    }
}

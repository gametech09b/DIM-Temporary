using System.Collections;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class EnemySpawner : SingletonMonobehaviour<EnemySpawner>
    {
        private int enemyToSpawn;
        private int currentCount;
        private int spawnedCount;
        private int maxConcurrentSpawnCount;

        private Room currentRoom;
        private RoomEnemySpawnParameter currentRoomEnemySpawnParameter;



        private void OnEnable()
        {
            DungeonStaticEvent.OnRoomChange += DungeonStaticEvent_OnRoomChange;
        }



        private void OnDisable()
        {
            DungeonStaticEvent.OnRoomChange -= DungeonStaticEvent_OnRoomChange;
        }



        private void DungeonStaticEvent_OnRoomChange(OnRoomChangeEventArgs _args)
        {
            currentCount = 0;
            spawnedCount = 0;

            currentRoom = _args.room;

            if (currentRoom.roomNodeType.isCorridorEW
            || currentRoom.roomNodeType.isCorridorNS
            || currentRoom.roomNodeType.isEntrance)
                return;

            if (currentRoom.isCleared)
                return;

            enemyToSpawn = currentRoom.GetNumberOfEnemyToSpawn(GameManager.Instance.GetCurrentDungeonLevel());

            currentRoomEnemySpawnParameter = currentRoom.GetRoomEnemySpawnParameter(GameManager.Instance.GetCurrentDungeonLevel());

            if (enemyToSpawn == 0)
            {
                currentRoom.isCleared = true;
                return;
            }

            maxConcurrentSpawnCount = GetConcurrentSpawnCount();

            currentRoom.roomGameObject.LockDoors();

            SpawnEnemy();
        }



        private void SpawnEnemy()
        {
            if (GameManager.Instance.gameState == GameState.PLAYING_LEVEL)
            {
                GameManager.Instance.previousGameState = GameState.PLAYING_LEVEL;
                GameManager.Instance.gameState = GameState.ENGAGING_ENEMY;
            }

            StartCoroutine(SpawnEnemyCoroutine());
        }



        private IEnumerator SpawnEnemyCoroutine()
        {
            Grid grid = currentRoom.GetGrid();

            RandomSpawnableObject<EnemyDetailSO> randomSpawnableObject = new RandomSpawnableObject<EnemyDetailSO>(currentRoom.enemySpawnByLevelList);

            if (currentRoom.spawnPositionArray.Length > 0)
            {
                for (int i = 0; i < enemyToSpawn; i++)
                {
                    while (currentCount >= maxConcurrentSpawnCount)
                    {
                        yield return null;
                    }

                    Vector3Int cellPosition = (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Length)];

                    CreateEnemy(randomSpawnableObject.GetObject(), grid.CellToWorld(cellPosition));

                    yield return new WaitForSeconds(GetSpawnInterval());
                }
            }
        }



        private float GetSpawnInterval()
        {
            return Random.Range(currentRoomEnemySpawnParameter.minSpawnInterval, currentRoomEnemySpawnParameter.maxSpawnInterval);
        }



        private int GetConcurrentSpawnCount()
        {
            return Random.Range(currentRoomEnemySpawnParameter.minConcurrentEnemy, currentRoomEnemySpawnParameter.maxConcurrentEnemy);
        }



        private void CreateEnemy(EnemyDetailSO _enemyDetail, Vector3 _position)
        {
            currentCount++;
            spawnedCount++;

            DungeonLevelSO dungeonLevel = GameManager.Instance.GetCurrentDungeonLevel();

            GameObject enemyGameObject = Instantiate(_enemyDetail.prefab, _position, Quaternion.identity);

            Enemy enemy = enemyGameObject.GetComponent<Enemy>();

            enemy.Init(_enemyDetail, spawnedCount, dungeonLevel);
        }
    }
}

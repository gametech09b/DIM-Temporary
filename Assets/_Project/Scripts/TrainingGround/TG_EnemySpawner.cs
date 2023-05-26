using System.Collections;
using UnityEngine;

using DIM.DungeonSystem;
using DIM.SpawnSystem;
using DIM.EnemySystem;

namespace DIM
{
    public class TG_EnemySpawner : MonoBehaviour
    {
        private int totalToSpawn;
        private int currentCount;
        private int spawnedCount;
        private int maxConcurrentSpawnCount;

        private Room currentRoom;
        private RoomEnemySpawnParameter currentRoomEnemySpawnParameter;

        void Start()
        {
            totalToSpawn = currentRoom.GetNumberOfEnemyToSpawn(GameManager.Instance.GetCurrentDungeonLevel());
            currentRoomEnemySpawnParameter = currentRoom.GetRoomEnemySpawnParameter(GameManager.Instance.GetCurrentDungeonLevel());
        }
        // Update is called once per frame
        void Update()
        {
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

        private void CreateEnemy(EnemyDetailSO _enemyDetail, Vector3 _position) {
            currentCount++;
            spawnedCount++;

            DungeonLevelSO dungeonLevel = GameManager.Instance.GetCurrentDungeonLevel();

            GameObject enemyGameObject = Instantiate(_enemyDetail.prefab, _position, Quaternion.identity);

            Enemy enemy = enemyGameObject.GetComponent<Enemy>();

            enemy.Init(_enemyDetail, spawnedCount, dungeonLevel);

        }
    }

    
}

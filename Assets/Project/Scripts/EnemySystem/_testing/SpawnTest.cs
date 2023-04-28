using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    public class SpawnTest : MonoBehaviour
    {
        public RoomTemplateSO roomTemplate;
        private List<SpawnableObjectsByLevel<EnemyDetailSO>> _enemiesByLevelList;
        private RandomSpawnableObject<EnemyDetailSO> _randomSpawnableObject;
        private GameObject spawnedEnemy;



        private void Start()
        {
            _enemiesByLevelList = roomTemplate.enemiesByLevelList;
            _randomSpawnableObject = new RandomSpawnableObject<EnemyDetailSO>(_enemiesByLevelList);
        }



        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (spawnedEnemy != null)
                {
                    Destroy(spawnedEnemy);
                }

                EnemyDetailSO enemyDetail = _randomSpawnableObject.GetObject();

                if (enemyDetail != null)
                {
                    Vector3 spawnPosition = HelperUtilities.GetNearestSpawnPoint(HelperUtilities.GetMouseWorldPosition());

                    spawnedEnemy = Instantiate(enemyDetail.prefab, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Debug.Log("No enemy to spawn");
                }
            }
        }
    }
}

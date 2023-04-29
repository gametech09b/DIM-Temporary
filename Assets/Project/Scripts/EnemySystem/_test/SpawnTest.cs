using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    public class SpawnTest : MonoBehaviour
    {
        private List<SpawnableObjectsByLevel<EnemyDetailSO>> enemySpawnByLevelList;
        private RandomSpawnableObject<EnemyDetailSO> randomSpawnableObject;
        private List<GameObject> _spawnedEnemyList = new List<GameObject>();



        private void OnEnable()
        {
            DungeonStaticEvent.OnRoomChange += DungeonStaticEvent_OnRoomChange;
        }



        private void OnDisable()
        {
            DungeonStaticEvent.OnRoomChange -= DungeonStaticEvent_OnRoomChange;
        }



        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {

                EnemyDetailSO enemyDetail = randomSpawnableObject.GetObject();

                if (enemyDetail != null)
                {
                    Vector3 spawnPosition = HelperUtilities.GetNearestSpawnPoint(HelperUtilities.GetMouseWorldPosition());

                    GameObject spawnedEnemy = Instantiate(enemyDetail.prefab, spawnPosition, Quaternion.identity);

                    _spawnedEnemyList.Add(spawnedEnemy);
                }
                else
                {
                    Debug.Log("No enemy to spawn");
                }
            }
        }



        private void DungeonStaticEvent_OnRoomChange(OnRoomChangeEventArgs _args)
        {
            if (_spawnedEnemyList != null
            && _spawnedEnemyList.Count > 0)
            {
                foreach (GameObject spawnedEnemy in _spawnedEnemyList)
                {
                    Destroy(spawnedEnemy);
                }

                _spawnedEnemyList.Clear();
            }

            RoomTemplateSO roomTemplate = DungeonBuilder.Instance.GetRoomTemplate(_args.room.templateID);

            if (roomTemplate != null)
            {
                enemySpawnByLevelList = roomTemplate.enemySpawnByLevelList;

                randomSpawnableObject = new RandomSpawnableObject<EnemyDetailSO>(enemySpawnByLevelList);
            }
        }
    }
}

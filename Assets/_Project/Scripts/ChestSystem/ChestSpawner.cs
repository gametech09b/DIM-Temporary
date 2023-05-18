using System.Collections.Generic;
using UnityEngine;

using DIM.CombatSystem;
using DIM.DungeonSystem;
using DIM.SpawnSystem;

namespace DIM.ChestSystem {
    public class ChestSpawner : MonoBehaviour {
        [System.Serializable]
        public struct RangeByLevel {
            public DungeonLevelSO dungeonLevel;
            [Range(0, 100)] public int min;
            [Range(0, 100)] public int max;
        }



        [SerializeField] private GameObject chestPrefab;

        [SerializeField][Range(0, 100)] private int minChestSpawnChance;
        [SerializeField][Range(0, 100)] private int maxChestSpawnChance;
        [SerializeField] private List<RangeByLevel> chestSpawnChanceByLevelList;

        [SerializeField] private ChestSpawnEventType chestSpawnEventType;
        [SerializeField] private ChestSpawnPointType chestSpawnPointType;

        [SerializeField][Range(0, 3)] private int minNumberOfItems;
        [SerializeField][Range(0, 3)] private int maxNumberOfItems;

        [SerializeField] private List<RangeByLevel> ammoByLevelList;
        [SerializeField] private List<RangeByLevel> healthByLevelList;
        [SerializeField] private List<SpawnableObjectsByLevel<WeaponDetailSO>> weaponByLevelList;

        private bool isChestSpawned = false;
        private Room chestRoom;

        // ===================================================================

        private void OnEnable() {
            DungeonStaticEvent.OnRoomChanged += DungeonStaticEvent_OnRoomChanged;
            DungeonStaticEvent.OnRoomEnemiesDefeated += DungeonStaticEvent_OnRoomEnemiesDefeated;
        }



        private void OnDisable() {
            DungeonStaticEvent.OnRoomChanged -= DungeonStaticEvent_OnRoomChanged;
            DungeonStaticEvent.OnRoomEnemiesDefeated -= DungeonStaticEvent_OnRoomEnemiesDefeated;
        }



        private void DungeonStaticEvent_OnRoomChanged(OnRoomChangedEventArgs _args) {
            if (chestRoom == null) {
                chestRoom = GetComponentInParent<RoomGameObject>().room;
            } else
            if (!isChestSpawned
            && chestSpawnEventType == ChestSpawnEventType.ON_ROOM_ENTRY
            && chestRoom == _args.room) {
                SpawnChest();
            }

        }



        private void DungeonStaticEvent_OnRoomEnemiesDefeated(OnRoomEnemiesDefeatedEventArgs _args) {
            if (chestRoom == null) {
                chestRoom = GetComponentInParent<RoomGameObject>().room;
            }

            if (!isChestSpawned
            && chestSpawnEventType == ChestSpawnEventType.ON_ENEMIES_DEFEATED
            && chestRoom == _args.room) {
                SpawnChest();
            }
        }



        private void SpawnChest() {
            isChestSpawned = true;

            if (!RandomSpawnChest())
                return;

            GetItemsToSpawn(out int ammoCount, out int healthCount, out int weaponCount);

            GameObject chestGameObject = Instantiate(chestPrefab, this.transform);

            if (chestSpawnPointType == ChestSpawnPointType.SPAWNER_POSITION) {
                chestGameObject.transform.position = this.transform.position;
            } else
            if (chestSpawnPointType == ChestSpawnPointType.PLAYER_POSITION) {
                Vector3 spawnPosition = HelperUtilities.GetNearestSpawnPoint(GameManager.Instance.GetCurrentPlayer().GetPosition());

                Vector3 variation = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

                chestGameObject.transform.position = spawnPosition + variation;
            }

            Chest chest = chestGameObject.GetComponent<Chest>();

            if (chestSpawnEventType == ChestSpawnEventType.ON_ROOM_ENTRY) {
                chest.Init(false, GetAmmoPercentToSpawn(ammoCount), GetHealthPercentToSpawn(healthCount), GetWeaponDetailToSpawn(weaponCount));
            } else {
                chest.Init(true, GetAmmoPercentToSpawn(ammoCount), GetHealthPercentToSpawn(healthCount), GetWeaponDetailToSpawn(weaponCount));
            }
        }


        private bool RandomSpawnChest() {
            int chancePercent = Random.Range(minChestSpawnChance, maxChestSpawnChance + 1);

            foreach (RangeByLevel rangeByLevel in chestSpawnChanceByLevelList) {
                if (rangeByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel()) {
                    chancePercent = Random.Range(rangeByLevel.min, rangeByLevel.max);
                    break;
                }
            }

            int randomPercent = Random.Range(0, 100 + 1);

            if (randomPercent <= chancePercent)
                return true;

            return false;
        }



        private void GetItemsToSpawn(out int _ammoCount, out int _healthCount, out int _weaponCount) {
            _ammoCount = 0;
            _healthCount = 0;
            _weaponCount = 0;

            int numberOfItems = Random.Range(minNumberOfItems, maxNumberOfItems + 1);

            int choice;

            if (numberOfItems == 1) {
                choice = Random.Range(0, 3);
                if (choice == 0)
                    _ammoCount++;
                else
                if (choice == 1)
                    _healthCount++;
                else
                if (choice == 2)
                    _weaponCount++;

                return;

            } else
            if (numberOfItems == 2) {
                choice = Random.Range(0, 3);
                if (choice == 0) {
                    _ammoCount = 1;
                    _healthCount = 1;
                } else
                if (choice == 1) {
                    _ammoCount = 1;
                    _weaponCount = 1;
                } else
                if (choice == 2) {
                    _healthCount = 1;
                    _weaponCount = 1;
                }

                return;

            } else
            if (numberOfItems == 3) {
                _ammoCount++;
                _healthCount++;
                _weaponCount++;
                return;
            }
        }



        private int GetAmmoPercentToSpawn(int _ammoCount) {
            if (_ammoCount == 0)
                return 0;

            foreach (RangeByLevel rangeByLevel in ammoByLevelList) {
                if (rangeByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel()) {
                    return Random.Range(rangeByLevel.min, rangeByLevel.max + 1);
                }
            }

            return 0;
        }



        private int GetHealthPercentToSpawn(int _healthCount) {
            if (_healthCount == 0)
                return 0;

            foreach (RangeByLevel rangeByLevel in healthByLevelList) {
                if (rangeByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel()) {
                    return Random.Range(rangeByLevel.min, rangeByLevel.max + 1);
                }
            }

            return 0;
        }



        private WeaponDetailSO GetWeaponDetailToSpawn(int _weaponCount) {
            if (_weaponCount == 0)
                return null;

            RandomSpawnableObject<WeaponDetailSO> randomSpawnableObject = new RandomSpawnableObject<WeaponDetailSO>(weaponByLevelList);

            WeaponDetailSO weaponDetail = randomSpawnableObject.GetObject();

            return weaponDetail;
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckNullValue(this, nameof(chestPrefab), chestPrefab);
            HelperUtilities.CheckPositiveRange(this, nameof(minChestSpawnChance), nameof(maxChestSpawnChance), minChestSpawnChance, maxChestSpawnChance);
            HelperUtilities.CheckPositiveRange(this, nameof(minNumberOfItems), nameof(maxNumberOfItems), minNumberOfItems, maxNumberOfItems);

            if (chestSpawnChanceByLevelList != null
            && chestSpawnChanceByLevelList.Count > 0) {
                foreach (RangeByLevel rangeByLevel in chestSpawnChanceByLevelList) {
                    HelperUtilities.CheckNullValue(this, nameof(rangeByLevel.dungeonLevel), rangeByLevel.dungeonLevel);
                    HelperUtilities.CheckPositiveRange(this, nameof(rangeByLevel.min), nameof(rangeByLevel.max), rangeByLevel.min, rangeByLevel.max);
                }
            }

            HelperUtilities.CheckPositiveRange(this, nameof(minNumberOfItems), nameof(maxNumberOfItems), minNumberOfItems, maxNumberOfItems, true);

            if (ammoByLevelList != null
            && ammoByLevelList.Count > 0) {
                HelperUtilities.CheckEnumerableValue(this, nameof(ammoByLevelList), ammoByLevelList);

                foreach (RangeByLevel rangeByLevel in ammoByLevelList) {
                    HelperUtilities.CheckNullValue(this, nameof(rangeByLevel.dungeonLevel), rangeByLevel.dungeonLevel);
                    HelperUtilities.CheckPositiveRange(this, nameof(rangeByLevel.min), nameof(rangeByLevel.max), rangeByLevel.min, rangeByLevel.max);
                }
            }

            if (healthByLevelList != null
            && healthByLevelList.Count > 0) {
                HelperUtilities.CheckEnumerableValue(this, nameof(healthByLevelList), healthByLevelList);

                foreach (RangeByLevel rangeByLevel in healthByLevelList) {
                    HelperUtilities.CheckNullValue(this, nameof(rangeByLevel.dungeonLevel), rangeByLevel.dungeonLevel);
                    HelperUtilities.CheckPositiveRange(this, nameof(rangeByLevel.min), nameof(rangeByLevel.max), rangeByLevel.min, rangeByLevel.max);
                }
            }

            if (weaponByLevelList != null
            && weaponByLevelList.Count > 0) {
                foreach (SpawnableObjectsByLevel<WeaponDetailSO> spawnableObjectsByLevel in weaponByLevelList) {
                    HelperUtilities.CheckNullValue(this, nameof(spawnableObjectsByLevel.dungeonLevel), spawnableObjectsByLevel.dungeonLevel);

                    foreach (SpawnableObjectRatio<WeaponDetailSO> spawnableObjectRatio in spawnableObjectsByLevel.spawnableObjectRatioList) {
                        HelperUtilities.CheckNullValue(this, nameof(spawnableObjectRatio.spawnableObject), spawnableObjectRatio.spawnableObject);
                        HelperUtilities.CheckPositiveValue(this, nameof(spawnableObjectRatio.ratio), spawnableObjectRatio.ratio);
                    }
                }
            }

        }
#endif
        #endregion
    }
}

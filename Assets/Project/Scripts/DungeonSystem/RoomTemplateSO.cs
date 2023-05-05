using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DungeonGunner
{
    [CreateAssetMenu(fileName = "Room_", menuName = "Scriptable Objects/Dungeon/Room")]
    public class RoomTemplateSO : ScriptableObject
    {
        [HideInInspector] public string id;



        [Space(10)]
        [Header("Prefab")]

        #region Tooltip
        [Tooltip("The gameobject prefab for the room (this will contain all the tilemaps for the room and environment game objects")]
        #endregion
        public GameObject prefab;
        [HideInInspector] public GameObject previousPrefab; // this is used to regenerate the guid if the SO is copied and the prefab is changed



        [Space(10)]
        [Header("Config")]

        #region Tooltip
        [Tooltip("The room node type SO. The room node types correspond to the room nodes used in the room node graph.  The exceptions being with corridors.  In the room node graph there is just one corridor type 'Corridor'.  For the room templates there are 2 corridor node types - CorridorNS and CorridorEW.")]
        #endregion
        public RoomNodeTypeSO roomNodeType;

        #region Tooltip
        [Tooltip("If you imagine a rectangle around the room tilemap that just completely encloses it, the room lower bounds represent the bottom left corner of that rectangle. This should be determined from the tilemap for the room (using the coordinate brush pointer to get the tilemap grid position for that bottom left corner (Note: this is the local tilemap position and NOT world position")]
        #endregion
        public Vector2Int lowerBounds;

        #region Tooltip
        [Tooltip("If you imagine a rectangle around the room tilemap that just completely encloses it, the room upper bounds represent the top right corner of that rectangle. This should be determined from the tilemap for the room (using the coordinate brush pointer to get the tilemap grid position for that top right corner (Note: this is the local tilemap position and NOT world position")]
        #endregion
        public Vector2Int upperBounds;

        #region Tooltip
        [Tooltip("There should be a maximum of four doorways for a room - one for each compass direction.  These should have a consistent 3 tile opening size, with the middle tile position being the doorway coordinate 'position'")]
        #endregion
        [SerializeField] private List<Doorway> doorwayList;

        #region Tooltip
        [Tooltip("Each possible spawn position (used for enemies and chests) for the room in tilemap coordinates should be added to this array")]
        #endregion
        public Vector2Int[] spawnPositionArray;





        [Space(10)]
        [Header("Enemies In Room Detail")]


        public List<SpawnableObjectsByLevel<EnemyDetailSO>> enemySpawnByLevelList;

        public List<RoomEnemySpawnParameter> roomEnemySpawnParameterList;






        /// <summary>
        /// Returns the list of Entrances for the room template
        /// </summary>
        public List<Doorway> GetDoorwayList()
        {
            return doorwayList;
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            // Set unique GUID, if empty or the prefab changes
            if (id == "" || previousPrefab != prefab)
            {
                id = GUID.Generate().ToString();
                previousPrefab = prefab;
                EditorUtility.SetDirty(this);
            }

            HelperUtilities.CheckNullValue(this, nameof(prefab), prefab);
            HelperUtilities.CheckNullValue(this, nameof(roomNodeType), roomNodeType);

            HelperUtilities.CheckEnumerableValue(this, nameof(doorwayList), doorwayList);
            HelperUtilities.CheckEnumerableValue(this, nameof(spawnPositionArray), spawnPositionArray);

            if (enemySpawnByLevelList.Count > 0 || roomEnemySpawnParameterList.Count > 0)
            {
                HelperUtilities.CheckEnumerableValue(this, nameof(enemySpawnByLevelList), enemySpawnByLevelList);
                HelperUtilities.CheckEnumerableValue(this, nameof(roomEnemySpawnParameterList), roomEnemySpawnParameterList);

                foreach (RoomEnemySpawnParameter roomEnemySpawnParameter in roomEnemySpawnParameterList)
                {
                    HelperUtilities.CheckNullValue(this, nameof(roomEnemySpawnParameter.dungeonLevel), roomEnemySpawnParameter.dungeonLevel);

                    HelperUtilities.CheckPositiveRange(
                        this,
                        nameof(roomEnemySpawnParameter.minTotalEnemy),
                        nameof(roomEnemySpawnParameter.maxTotalEnemy),
                        roomEnemySpawnParameter.minTotalEnemy,
                        roomEnemySpawnParameter.maxTotalEnemy
                    );

                    HelperUtilities.CheckPositiveRange(
                        this,
                        nameof(roomEnemySpawnParameter.minConcurrentEnemy),
                        nameof(roomEnemySpawnParameter.maxConcurrentEnemy),
                        roomEnemySpawnParameter.minConcurrentEnemy,
                        roomEnemySpawnParameter.maxConcurrentEnemy
                    );

                    HelperUtilities.CheckPositiveRange(
                        this,
                        nameof(roomEnemySpawnParameter.minSpawnInterval),
                        nameof(roomEnemySpawnParameter.maxSpawnInterval),
                        roomEnemySpawnParameter.minSpawnInterval,
                        roomEnemySpawnParameter.maxSpawnInterval
                    );

                    bool isEnemyTypeListValid = false;

                    foreach (SpawnableObjectsByLevel<EnemyDetailSO> enemiesSpawnByLevel in enemySpawnByLevelList)
                    {
                        if (enemiesSpawnByLevel.dungeonLevel == roomEnemySpawnParameter.dungeonLevel
                        && enemiesSpawnByLevel.spawnableObjectRatioList.Count > 0)
                            isEnemyTypeListValid = true;

                        HelperUtilities.CheckNullValue(this, nameof(enemiesSpawnByLevel.dungeonLevel), enemiesSpawnByLevel.dungeonLevel);

                        foreach (SpawnableObjectRatio<EnemyDetailSO> enemySpawnRatio in enemiesSpawnByLevel.spawnableObjectRatioList)
                        {
                            HelperUtilities.CheckNullValue(this, nameof(enemySpawnRatio.spawnableObject), enemySpawnRatio.spawnableObject);
                            HelperUtilities.CheckPositiveValue(this, nameof(enemySpawnRatio.ratio), enemySpawnRatio.ratio);
                        }
                    }

                    if (!isEnemyTypeListValid
                    && roomEnemySpawnParameter.dungeonLevel != null)
                        Debug.LogError($"RoomTemplateSO: {name} has no enemies for dungeon level {roomEnemySpawnParameter.dungeonLevel.name}");
                }
            }
        }

#endif
        #endregion Validation
    }
}

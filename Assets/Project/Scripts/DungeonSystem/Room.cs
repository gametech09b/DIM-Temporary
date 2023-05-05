using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    public class Room
    {
        public string id;
        public string templateID;
        public GameObject prefab;
        public RoomNodeTypeSO roomNodeType;
        public Vector2Int lowerBounds;
        public Vector2Int upperBounds;
        public Vector2Int templateLowerBounds;
        public Vector2Int templateUpperBounds;
        public Vector2Int[] spawnPositionArray;


        public string parentRoomID;
        public List<string> childRoomIDList;


        public List<Doorway> doorwayList;
        public bool isPositioned;


        public bool isLit = false;
        public bool isCleared = false;
        public bool isVisited = false;


        public RoomGameObject roomGameObject;


        public List<SpawnableObjectsByLevel<EnemyDetailSO>> enemySpawnByLevelList;
        public List<RoomEnemySpawnParameter> roomEnemySpawnParameterList;



        public Room()
        {
            childRoomIDList = new List<string>();
            doorwayList = new List<Doorway>();
        }



        public Vector3 GetMiddlePosition()
        {
            Vector3 middlePosition = new Vector3();
            middlePosition.x = (lowerBounds.x + upperBounds.x) / 2f;
            middlePosition.y = (lowerBounds.y + upperBounds.y) / 2f;

            return middlePosition;
        }



        public Grid GetGrid()
        {
            return roomGameObject.grid;
        }



        public int GetNumberOfEnemyToSpawn(DungeonLevelSO _dungeonLevel)
        {
            foreach (RoomEnemySpawnParameter roomEnemySpawnParameter in roomEnemySpawnParameterList)
            {
                if (roomEnemySpawnParameter.dungeonLevel == _dungeonLevel)
                    return Random.Range(roomEnemySpawnParameter.minTotalEnemy, roomEnemySpawnParameter.maxTotalEnemy);
            }

            return 0;
        }



        public RoomEnemySpawnParameter GetRoomEnemySpawnParameter(DungeonLevelSO _dungeonLevel)
        {
            foreach (RoomEnemySpawnParameter roomEnemySpawnParameter in roomEnemySpawnParameterList)
            {
                if (roomEnemySpawnParameter.dungeonLevel == _dungeonLevel)
                    return roomEnemySpawnParameter;
            }

            return null;
        }
    }
}

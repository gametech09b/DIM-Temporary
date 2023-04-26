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
        public List<string> childRoomIDList;
        public string parentRoomID;
        public List<Doorway> doorwayList;
        public bool isPositioned;
        public RoomGameObject roomGameObject;
        public bool isLit = false;
        public bool isCleared = false;
        public bool isVisited = false;



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
    }
}

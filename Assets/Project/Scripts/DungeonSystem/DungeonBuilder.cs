using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class DungeonBuilder : SingletonMonobehaviour<DungeonBuilder>
    {
        public Dictionary<string, Room> roomDictionary = new Dictionary<string, Room>();
        private Dictionary<string, RoomTemplateSO> roomTemplateDictionary = new Dictionary<string, RoomTemplateSO>();
        private List<RoomTemplateSO> roomTemplateList = null;
        private RoomNodeTypeListSO roomNodeTypeList;
        private bool dungeonBuildSuccessful;



        protected override void Awake()
        {
            base.Awake();

            LoadRoomNodeTypeList();
        }



        private void OnEnable()
        {
            GameResources.Instance.DimmedMaterial.SetFloat("Alpha_Slider", 0);
        }



        private void OnDisable()
        {
            GameResources.Instance.DimmedMaterial.SetFloat("Alpha_Slider", 1f);
        }



        /// <summary>
        /// Load the room node type list
        /// </summary>
        private void LoadRoomNodeTypeList()
        {
            roomNodeTypeList = GameResources.Instance.RoomNodeTypeList;
        }



        /// <summary>
        /// Generate random dungeon, returns true if dungeon built, false if failed
        /// </summary>
        public bool GenerateDungeon(DungeonLevelSO currentDungeonLevel)
        {
            roomTemplateList = currentDungeonLevel.roomTemplateList;

            // Load the scriptable object room templates into the dictionary
            LoadRoomTemplatesIntoDictionary();

            dungeonBuildSuccessful = false;
            int dungeonBuildAttempts = 0;
            while (!dungeonBuildSuccessful && dungeonBuildAttempts < Settings.MaxDungeonBuildAttempts)
            {
                dungeonBuildAttempts++;

                // Select a random room node graph from the list
                RoomNodeGraphSO roomNodeGraph = SelectRandomRoomNodeGraph(currentDungeonLevel.roomNodeGraphList);

                int roomBuildAttempts = 0;
                dungeonBuildSuccessful = false;

                // Loop until dungeon successfully built or more than max attempts for node graph
                while (!dungeonBuildSuccessful && roomBuildAttempts <= Settings.MaxDungeonRoomRebuildAttempts)
                {
                    ClearDungeon();

                    roomBuildAttempts++;

                    // Attempt To Build A Random Dungeon For The Selected room node graph
                    dungeonBuildSuccessful = AttemptToBuildRandomDungeon(roomNodeGraph);
                }


                if (dungeonBuildSuccessful)
                {
                    InstantiateRoomGameObjects();
                }
            }

            return dungeonBuildSuccessful;
        }



        /// <summary>
        /// Load the room templates into the dictionary
        /// </summary>
        private void LoadRoomTemplatesIntoDictionary()
        {
            roomTemplateDictionary.Clear();

            foreach (RoomTemplateSO roomTemplate in roomTemplateList)
            {
                if (!roomTemplateDictionary.ContainsKey(roomTemplate.id))
                {
                    roomTemplateDictionary.Add(roomTemplate.id, roomTemplate);
                }
                else
                {
                    Debug.Log($"Duplicate Room Template Key In {roomTemplateList}");
                }
            }
        }



        /// <summary>
        /// Attempt to randomly build the dungeon for the specified room nodeGraph.
        /// </summary>
        /// <param name="roomNodeGraph"></param>
        /// <returns>Returns true if a successful random layout was generated, else returns false if a problem was encoutered and another attempt is required.</returns>
        private bool AttemptToBuildRandomDungeon(RoomNodeGraphSO roomNodeGraph)
        {
            // Create Room Node Build Queue
            Queue<RoomNodeSO> roomNodeBuildQueue = new Queue<RoomNodeSO>();

            // Add Entrance Node To Room Node Queue From Room Node Graph
            RoomNodeSO entranceNode = roomNodeGraph.GetRoomNode(roomNodeTypeList.list.Find(x => x.isEntrance));

            if (entranceNode != null)
            {
                roomNodeBuildQueue.Enqueue(entranceNode);
            }
            else
            {
                Debug.Log("No Entrance Node");
                return false;
            }

            // Start with no room overlaps
            bool isRoomOverlaps = false;

            // Process room nodes build queue
            isRoomOverlaps = TryBuildRoomNode(roomNodeGraph, roomNodeBuildQueue, isRoomOverlaps);

            // If all the room nodes have been processed and there hasn't been a room overlap then return true
            if (roomNodeBuildQueue.Count == 0 && !isRoomOverlaps)
            {
                return true;
            }

            return false;
        }



        /// <summary>
        /// Process rooms in the room node build queue.
        /// </summary>
        /// <param name="roomNodeGraph"></param>
        /// <param name="roomNodeBuildQueue"></param>
        /// <param name="isRoomOverlaps"></param>
        /// <returns>Returning true if there are no room overlaps</returns>
        private bool TryBuildRoomNode(RoomNodeGraphSO roomNodeGraph, Queue<RoomNodeSO> roomNodeBuildQueue, bool isRoomOverlaps)
        {
            // While room nodes in open room node queue & no room overlaps detected.
            while (roomNodeBuildQueue.Count > 0 && !isRoomOverlaps)
            {
                // Get next room node from open room node queue.
                RoomNodeSO roomNode = roomNodeBuildQueue.Dequeue();

                // Add child Nodes to queue from room node graph (with links to this parent Room)
                foreach (RoomNodeSO childRoomNode in roomNodeGraph.GetChildRoomNodes(roomNode))
                {
                    roomNodeBuildQueue.Enqueue(childRoomNode);
                }

                // if the room is the entrance mark as positioned and add to room dictionary
                if (roomNode.roomNodeType.isEntrance)
                {
                    RoomTemplateSO roomTemplate = GetRandomRoomTemplate(roomNode.roomNodeType);

                    Room room = CreateRoomFromRoomTemplate(roomTemplate, roomNode);

                    room.isPositioned = true;

                    // Add room to room dictionary
                    roomDictionary.Add(room.id, room);
                }

                // else if the room type isn't an entrance
                else
                {
                    // Else get parent room for node
                    Room parentRoom = roomDictionary[roomNode.parentRoomNodeIDList[0]];

                    // See if room can be placed without overlaps
                    isRoomOverlaps = TryPlaceRoomWithNoOverlaps(roomNode, parentRoom);
                }

            }

            return isRoomOverlaps;
        }



        /// <summary>
        /// Attempt to place the room node in the dungeon - if room can be placed return the room, else return null
        /// </summary>
        private bool TryPlaceRoomWithNoOverlaps(RoomNodeSO roomNode, Room parentRoom)
        {
            // initialise and assume overlap until proven otherwise.
            bool isOverlaps = true;

            // Do While Room Overlaps - try to place against all available doorways of the parent until
            // the room is successfully placed without overlap.
            while (isOverlaps)
            {
                // Select random unconnected available doorway for Parent
                List<Doorway> unconnectedAvailableParentDoorwayList = GetUnconnectedAvailableDoorways(parentRoom.doorwayList).ToList();

                if (unconnectedAvailableParentDoorwayList.Count == 0)
                {
                    // If no more doorways to try then overlap failure.
                    return true; // room overlaps
                }

                Doorway currentDoorway = unconnectedAvailableParentDoorwayList[UnityEngine.Random.Range(0, unconnectedAvailableParentDoorwayList.Count)];

                // Get a random room template for room node that is consistent with the parent door orientation
                RoomTemplateSO roomtemplate = GetRandomRoomTemplateCurrentDoorway(roomNode, currentDoorway);

                // Create a room
                Room room = CreateRoomFromRoomTemplate(roomtemplate, roomNode);

                // Place the room - returns true if the room doesn't overlap
                if (TryPlaceRoom(parentRoom, currentDoorway, room))
                {
                    // If room doesn't overlap then set to false to exit while loop
                    isOverlaps = false;

                    // Mark room as positioned
                    room.isPositioned = true;

                    // Add room to dictionary
                    roomDictionary.Add(room.id, room);
                }
                else
                {
                    isOverlaps = true;
                }
            }

            return false;  // no room overlaps

        }



        /// <summary>
        /// Get random room template for room node taking into account the parent doorway orientation.
        /// </summary>
        private RoomTemplateSO GetRandomRoomTemplateCurrentDoorway(RoomNodeSO roomNode, Doorway doorway)
        {
            RoomTemplateSO roomtemplate = null;

            // If room node is a corridor then select random correct Corridor room template based on
            // parent doorway orientation
            if (roomNode.roomNodeType.isCorridor)
            {
                switch (doorway.orientation)
                {
                    case Orientation.NORTH:
                    case Orientation.SOUTH:
                        roomtemplate = GetRandomRoomTemplate(roomNodeTypeList.list.Find(x => x.isCorridorNS));
                        break;


                    case Orientation.EAST:
                    case Orientation.WEST:
                        roomtemplate = GetRandomRoomTemplate(roomNodeTypeList.list.Find(x => x.isCorridorEW));
                        break;


                    case Orientation.NONE:
                        break;

                    default:
                        break;
                }
            }
            // Else select random room template
            else
            {
                roomtemplate = GetRandomRoomTemplate(roomNode.roomNodeType);
            }

            return roomtemplate;
        }



        /// <summary>
        /// Place the room - returns true if the room doesn't overlap, false otherwise
        /// </summary>
        /// <param name="currentRoom"></param>
        /// <param name="currentDoorway"></param>
        /// <param name="roomToPlace"></param>
        /// <returns></returns>
        private bool TryPlaceRoom(Room currentRoom, Doorway currentDoorway, Room roomToPlace)
        {
            // Get opposite room doorway position
            Doorway oppositeDoorway = GetOppositeDoorway(currentDoorway, roomToPlace.doorwayList);

            // Return if no doorway in room opposite to parent doorway
            if (oppositeDoorway == null)
            {
                // Just mark the parent doorway as unavailable so we don't try and connect it again
                currentDoorway.isUnavailable = true;

                return false;
            }

            // Calculate 'world' grid parent doorway position
            Vector2Int currentDoorwayPosition = currentRoom.lowerBounds + currentDoorway.position - currentRoom.templateLowerBounds;

            Vector2Int adjustment = Vector2Int.zero;

            // Calculate adjustment position offset based on room doorway position that we are trying to connect (e.g. if this doorway is WEST then we need to add (1,0) to the east parent doorway)
            switch (oppositeDoorway.orientation)
            {
                case Orientation.NORTH:
                    adjustment = new Vector2Int(0, -1);
                    break;

                case Orientation.EAST:
                    adjustment = new Vector2Int(-1, 0);
                    break;

                case Orientation.SOUTH:
                    adjustment = new Vector2Int(0, 1);
                    break;

                case Orientation.WEST:
                    adjustment = new Vector2Int(1, 0);
                    break;

                case Orientation.NONE:
                    break;

                default:
                    break;
            }

            // Calculate room lower bounds and upper bounds based on positioning to align with parent doorway
            roomToPlace.lowerBounds = currentDoorwayPosition + adjustment + roomToPlace.templateLowerBounds - oppositeDoorway.position;
            roomToPlace.upperBounds = roomToPlace.lowerBounds + roomToPlace.templateUpperBounds - roomToPlace.templateLowerBounds;

            Room overlappingRoom = CheckForRoomOverlap(roomToPlace);

            if (overlappingRoom == null)
            {
                // mark doorways as connected & unavailable
                currentDoorway.isConnected = true;
                currentDoorway.isUnavailable = true;

                oppositeDoorway.isConnected = true;
                oppositeDoorway.isUnavailable = true;

                // return true to show rooms have been connected with no overlap
                return true;
            }
            else
            {
                // Just mark the parent doorway as unavailable so we don't try and connect it again
                currentDoorway.isUnavailable = true;

                return false;
            }
        }



        /// <summary>
        /// Get the compared doorway from the doorway list that has the opposite orientation to current doorway
        /// </summary>
        /// <param name="currentDoorway"></param>
        /// <param name="doorwayList"></param>
        /// <returns></returns>
        private Doorway GetOppositeDoorway(Doorway currentDoorway, List<Doorway> doorwayList)
        {
            foreach (Doorway comparedDoorway in doorwayList)
            {
                if (currentDoorway.orientation == Orientation.EAST && comparedDoorway.orientation == Orientation.WEST)
                {
                    return comparedDoorway;
                }
                else if (currentDoorway.orientation == Orientation.WEST && comparedDoorway.orientation == Orientation.EAST)
                {
                    return comparedDoorway;
                }
                else if (currentDoorway.orientation == Orientation.NORTH && comparedDoorway.orientation == Orientation.SOUTH)
                {
                    return comparedDoorway;
                }
                else if (currentDoorway.orientation == Orientation.SOUTH && comparedDoorway.orientation == Orientation.NORTH)
                {
                    return comparedDoorway;
                }
            }

            return null;
        }



        /// <summary>
        /// Check for rooms that overlap the upper and lower bounds parameters.
        /// </summary>
        /// <param name="roomToCheck"></param>
        /// <returns>If there are overlapping rooms then return room else return null</returns>
        private Room CheckForRoomOverlap(Room roomToCheck)
        {
            // Iterate through all rooms
            foreach (KeyValuePair<string, Room> roomDictionaryKVP in roomDictionary)
            {
                Room room = roomDictionaryKVP.Value;

                // skip if same room as room to test or room hasn't been positioned
                if (room.id == roomToCheck.id || !room.isPositioned)
                    continue;

                // If room overlaps
                if (IsRoomOverlapping(roomToCheck, room))
                {
                    return room;
                }
            }

            return null;
        }



        /// <summary>
        /// Check if 2 rooms overlap each other
        /// </summary>
        /// <param name="roomA"></param>
        /// <param name="roomB"></param>
        /// <returns>True if they overlap or false if they don't overlap</returns>
        private bool IsRoomOverlapping(Room roomA, Room roomB)
        {
            bool isOverlappingX = IsIntervalOverlapping(roomA.lowerBounds.x, roomA.upperBounds.x, roomB.lowerBounds.x, roomB.upperBounds.x);
            bool isOverlappingY = IsIntervalOverlapping(roomA.lowerBounds.y, roomA.upperBounds.y, roomB.lowerBounds.y, roomB.upperBounds.y);

            if (isOverlappingX && isOverlappingY)
            {
                return true;
            }

            return false;
        }



        /// <summary>
        /// Check if interval 1 overlaps interval 2 - this method is used by the IsOverlappingRoom method
        /// </summary>
        /// <param name="imin1"></param>
        /// <param name="imax1"></param>
        /// <param name="imin2"></param>
        /// <param name="imax2"></param>
        /// <returns></returns>
        private bool IsIntervalOverlapping(int imin1, int imax1, int imin2, int imax2)
        {
            if (Mathf.Max(imin1, imin2) <= Mathf.Min(imax1, imax2))
            {
                return true;
            }

            return false;
        }



        /// <summary>
        /// /// Get a random room template from the roomtemplatelist that matches the roomType and return it
        /// </summary>
        /// <param name="roomNodeType"></param>
        /// <returns>Null if no matching room templates found</returns>
        private RoomTemplateSO GetRandomRoomTemplate(RoomNodeTypeSO roomNodeType)
        {
            List<RoomTemplateSO> matchTypeRoomTemplateList = new List<RoomTemplateSO>();

            foreach (RoomTemplateSO roomTemplate in roomTemplateList)
            {
                if (roomTemplate.roomNodeType == roomNodeType)
                {
                    matchTypeRoomTemplateList.Add(roomTemplate);
                }
            }

            if (matchTypeRoomTemplateList.Count == 0) return null;

            return matchTypeRoomTemplateList[UnityEngine.Random.Range(0, matchTypeRoomTemplateList.Count)];
        }



        /// <summary>
        /// Get unconnected doorways
        /// </summary>
        /// <param name="roomDoorwayList"></param>
        /// <returns></returns>
        private IEnumerable<Doorway> GetUnconnectedAvailableDoorways(List<Doorway> roomDoorwayList)
        {
            foreach (Doorway doorway in roomDoorwayList)
            {
                if (!doorway.isConnected && !doorway.isUnavailable)
                    yield return doorway;
            }
        }



        /// <summary>
        /// Create room based on roomTemplate and layoutNode, and return the created room
        /// </summary>
        /// <param name="roomTemplate"></param>
        /// <param name="roomNode"></param>
        /// <returns></returns>
        private Room CreateRoomFromRoomTemplate(RoomTemplateSO roomTemplate, RoomNodeSO roomNode)
        {
            Room room = new Room();

            room.id = roomNode.id;
            room.templateID = roomTemplate.id;
            room.prefab = roomTemplate.prefab;
            room.roomNodeType = roomTemplate.roomNodeType;
            room.lowerBounds = roomTemplate.lowerBounds;
            room.upperBounds = roomTemplate.upperBounds;
            room.templateLowerBounds = roomTemplate.lowerBounds;
            room.templateUpperBounds = roomTemplate.upperBounds;
            room.spawnPositionArray = roomTemplate.spawnPositionArray;
            room.childRoomIDList = CopyStringList(roomNode.childRoomNodeIDList);
            room.doorwayList = CopyDoorwayList(roomTemplate.GetDoorwayList());

            room.enemySpawnByLevelList = roomTemplate.enemySpawnByLevelList;
            room.roomEnemySpawnParameterList = roomTemplate.roomEnemySpawnParameterList;

            if (roomNode.parentRoomNodeIDList.Count == 0) // Entrance
            {
                room.parentRoomID = "";
                room.isVisited = true;

                GameManager.Instance.SetCurrentRoom(room);
            }
            else
            {
                room.parentRoomID = roomNode.parentRoomNodeIDList[0];
            }

            if (room.GetNumberOfEnemyToSpawn(GameManager.Instance.GetCurrentDungeonLevel()) == 0)
            {
                room.isCleared = true;
            }

            return room;
        }



        /// <summary>
        /// Select a random room node graph from the list of room node graphs
        /// </summary>
        /// <param name="roomNodeGraphList"></param>
        /// <returns></returns>
        private RoomNodeGraphSO SelectRandomRoomNodeGraph(List<RoomNodeGraphSO> roomNodeGraphList)
        {
            if (roomNodeGraphList.Count > 0)
            {
                return roomNodeGraphList[UnityEngine.Random.Range(0, roomNodeGraphList.Count)];
            }

            Debug.Log("No room node graphs in list");
            return null;
        }



        /// <summary>
        /// Create deep copy of doorway list
        /// </summary>
        /// <param name="oldDoorwayList"></param>
        /// <returns></returns>
        private List<Doorway> CopyDoorwayList(List<Doorway> oldDoorwayList)
        {
            List<Doorway> newDoorwayList = new List<Doorway>();

            foreach (Doorway doorway in oldDoorwayList)
            {
                Doorway newDoorway = new Doorway();

                newDoorway.position = doorway.position;
                newDoorway.orientation = doorway.orientation;
                newDoorway.doorPrefab = doorway.doorPrefab;
                newDoorway.isConnected = doorway.isConnected;
                newDoorway.isUnavailable = doorway.isUnavailable;
                newDoorway.startCopyPosition = doorway.startCopyPosition;
                newDoorway.copyTileWidth = doorway.copyTileWidth;
                newDoorway.copyTileHeight = doorway.copyTileHeight;

                newDoorwayList.Add(newDoorway);
            }

            return newDoorwayList;
        }



        /// <summary>
        /// Create deep copy of string list
        /// </summary>
        /// <param name="oldStringList"></param>
        /// <returns></returns>
        private List<string> CopyStringList(List<string> oldStringList)
        {
            List<string> newStringList = new List<string>();

            foreach (string stringValue in oldStringList)
            {
                newStringList.Add(stringValue);
            }

            return newStringList;
        }



        /// <summary>
        /// Instantiate the dungeon room gameobjects from the prefabs
        /// </summary>
        private void InstantiateRoomGameObjects()
        {
            foreach (KeyValuePair<string, Room> roomDictionaryKVP in roomDictionary)
            {
                Room room = roomDictionaryKVP.Value;

                Vector3 roomPosition = new Vector3(room.lowerBounds.x - room.templateLowerBounds.x, room.lowerBounds.y - room.templateLowerBounds.y, 0f);

                GameObject instantiatedRoomGameObject = Instantiate(room.prefab, roomPosition, Quaternion.identity, transform);

                RoomGameObject roomGameObjectComponent = instantiatedRoomGameObject.GetComponent<RoomGameObject>();

                roomGameObjectComponent.room = room;

                roomGameObjectComponent.Init(instantiatedRoomGameObject);

                room.roomGameObject = roomGameObjectComponent;
            }
        }




        /// <summary>
        /// Get a room template by room template ID
        /// </summary>
        /// <param name="roomTemplateID"></param>
        /// <returns>null if ID doesn't exist</returns>
        public RoomTemplateSO GetRoomTemplate(string roomTemplateID)
        {
            if (roomTemplateDictionary.TryGetValue(roomTemplateID, out RoomTemplateSO roomTemplate))
            {
                return roomTemplate;
            }

            return null;
        }



        /// <summary>
        /// Get room by roomID, if no room exists with that ID return null
        /// </summary>
        /// <param name="roomID"></param>
        /// <returns></returns>
        public Room GetRoom(string roomID)
        {
            if (roomDictionary.TryGetValue(roomID, out Room room))
            {
                return room;
            }

            return null;
        }



        /// <summary>
        /// Clear dungeon room gameobjects and dungeon room dictionary
        /// </summary>
        private void ClearDungeon()
        {
            // Destroy instantiated dungeon gameobjects and clear dungeon manager room dictionary
            if (roomDictionary.Count > 0)
            {
                foreach (KeyValuePair<string, Room> roomDictionaryKVP in roomDictionary)
                {
                    Room room = roomDictionaryKVP.Value;

                    if (room.roomGameObject != null)
                    {
                        Destroy(room.roomGameObject.gameObject);
                    }
                }

                roomDictionary.Clear();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [CreateAssetMenu(fileName = "RoomNodeGraphSO_", menuName = "Scriptable Objects/Dungeon/Room Node Graph")]
    public class RoomNodeGraphSO : ScriptableObject
    {
        [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
        [HideInInspector] public List<RoomNodeSO> roomNodeList = new List<RoomNodeSO>();
        [HideInInspector] public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();



        private void Awake()
        {
            LoadRoomNodeDictionary();
        }



        /// <summary>
        /// Load the room node dictionary
        /// </summary>
        private void LoadRoomNodeDictionary()
        {
            roomNodeDictionary.Clear();

            foreach (RoomNodeSO roomNode in roomNodeList)
            {
                roomNodeDictionary.Add(roomNode.id, roomNode);
            }
        }



        #region Editor
#if UNITY_EDITOR
        [HideInInspector] public RoomNodeSO roomNodeToDrawLineFrom;
        [HideInInspector] public Vector2 linePosition;



        public void OnValidate()
        {
            LoadRoomNodeDictionary();
        }



        /// <summary>
        /// Set the room node to draw a connection line from
        /// </summary>
        /// <param name="node"></param>
        /// <param name="position"></param>
        public void SetNodeToDrawConnectionLineFrom(RoomNodeSO node, Vector2 position)
        {
            roomNodeToDrawLineFrom = node;
            linePosition = position;
        }
#endif
        #endregion
    }
}

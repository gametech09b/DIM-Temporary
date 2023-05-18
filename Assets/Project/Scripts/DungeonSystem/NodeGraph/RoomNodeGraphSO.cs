using System.Collections.Generic;
using UnityEngine;

namespace DIM.DungeonSystem {
    [CreateAssetMenu(fileName = "RoomNodeGraphSO_", menuName = "Scriptable Objects/Dungeon/Room Node Graph")]
    public class RoomNodeGraphSO : ScriptableObject {
        [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
        [HideInInspector] public List<RoomNodeSO> roomNodeList = new List<RoomNodeSO>();
        [HideInInspector] public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();

        // ===================================================================

        private void Awake() {
            LoadRoomNodeDictionary();
        }



        /// <summary>
        /// Load the room node dictionary
        /// </summary>
        private void LoadRoomNodeDictionary() {
            roomNodeDictionary.Clear();

            foreach (RoomNodeSO roomNode in roomNodeList) {
                roomNodeDictionary.Add(roomNode.id, roomNode);
            }
        }


        /// <summary>
        /// Get the room node by room node ID
        /// </summary>
        /// <param name="_roomNodeID"></param>
        /// <returns></returns>
        public RoomNodeSO GetRoomNode(string _roomNodeID) {
            if (roomNodeDictionary.TryGetValue(_roomNodeID, out RoomNodeSO roomNode))
                return roomNode;

            return null;
        }



        /// <summary>
        /// Get the room node by room node type
        /// </summary>
        /// <param name="_roomNodeType"></param>
        /// <returns></returns>
        public RoomNodeSO GetRoomNode(RoomNodeTypeSO _roomNodeType) {
            foreach (RoomNodeSO roomNode in roomNodeList) {
                if (roomNode.roomNodeType == _roomNodeType)
                    return roomNode;
            }
            return null;
        }



        /// <summary>
        /// Get every child RoomNode ID in parentRoomNode
        /// </summary>
        /// <param name="_parentRoomNode"></param>
        /// <returns></returns>
        public IEnumerable<RoomNodeSO> GetChildRoomNodes(RoomNodeSO _parentRoomNode) {
            foreach (string childRoomNodeID in _parentRoomNode.childRoomNodeIDList) {
                yield return GetRoomNode(childRoomNodeID);
            }
        }


        #region Editor
#if UNITY_EDITOR
        [HideInInspector] public RoomNodeSO roomNodeToDrawLineFrom;
        [HideInInspector] public Vector2 linePosition;

        // ===================================================================

        public void OnValidate() {
            LoadRoomNodeDictionary();
        }



        /// <summary>
        /// Set the room node to draw a connection line from
        /// </summary>
        /// <param name="_node"></param>
        /// <param name="_position"></param>
        public void SetNodeToDrawConnectionLineFrom(RoomNodeSO _node, Vector2 _position) {
            roomNodeToDrawLineFrom = _node;
            linePosition = _position;
        }
#endif
        #endregion
    }
}

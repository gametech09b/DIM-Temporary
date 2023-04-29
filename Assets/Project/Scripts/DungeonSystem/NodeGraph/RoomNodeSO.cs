using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DungeonGunner {
    public class RoomNodeSO : ScriptableObject {
        [HideInInspector] public string id;
        [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>();
        [HideInInspector] public List<string> childRoomNodeIDList = new List<string>();
        [HideInInspector] public RoomNodeGraphSO roomNodeGraph;
        public RoomNodeTypeSO roomNodeType;
        [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;



        #region Editor
#if UNITY_EDITOR
        [HideInInspector] public Rect rect;
        [HideInInspector] public bool isLeftClickDragging;
        [HideInInspector] public bool isSelected;



        /// <summary>
        /// Initialize the room node
        /// </summary>
        /// <param name="_rect"></param>
        /// <param name="_roomNodeGraph"></param>
        /// <param name="_roomNodeType"></param>
        public void Initialize(Rect _rect, RoomNodeGraphSO _roomNodeGraph, RoomNodeTypeSO _roomNodeType) {
            this.rect = _rect;
            this.id = Guid.NewGuid().ToString();
            this.name = "RoomNode";
            this.roomNodeGraph = _roomNodeGraph;
            this.roomNodeType = _roomNodeType;

            roomNodeTypeList = GameResources.Instance.RoomNodeTypeList;
        }



        /// <summary>
        /// Draw the room node
        /// </summary>
        /// <param name="_nodeStyle"></param>
        public void Draw(GUIStyle _nodeStyle) {
            GUILayout.BeginArea(rect, _nodeStyle);
            EditorGUI.BeginChangeCheck();

            if (parentRoomNodeIDList.Count > 0 || roomNodeType.isEntrance) {
                EditorGUILayout.LabelField(roomNodeType.roomNodeTypeName);
            } else {

                int currentSelectedTypeIndex = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);
                int newSelectedTypeIndex = EditorGUILayout.Popup("", currentSelectedTypeIndex, GetRoomNodeTypesToDisplay());

                roomNodeType = roomNodeTypeList.list[newSelectedTypeIndex];

                RoomNodeTypeSO currentSelectedType = roomNodeTypeList.list[currentSelectedTypeIndex];
                RoomNodeTypeSO newSelectedType = roomNodeTypeList.list[newSelectedTypeIndex];

                // check invalid
                if (currentSelectedType.isCorridor && !newSelectedType.isCorridor
                || !currentSelectedType.isCorridor && newSelectedType.isCorridor
                || !currentSelectedType.isBossRoom && newSelectedType.isBossRoom) {
                    if (childRoomNodeIDList.Count <= 0) return;

                    for (int i = childRoomNodeIDList.Count - 1; i >= 0; i--) {
                        string childRoomNodeID = childRoomNodeIDList[i];
                        RoomNodeSO childRoomNode = roomNodeGraph.GetRoomNode(childRoomNodeID);

                        if (childRoomNode == null) continue;

                        RemoveChildRoomNodeIDFromRoomNode(childRoomNodeID);
                        childRoomNode.RemoveParentRoomNodeIDToRoomNode(id);
                    }
                }
            }

            if (EditorGUI.EndChangeCheck()) {
                EditorUtility.SetDirty(this);
            }

            GUILayout.EndArea();
        }



        /// <summary>
        /// Get the room node types to display in the popup
        /// </summary>
        /// <returns></returns>
        public string[] GetRoomNodeTypesToDisplay() {
            string[] roomNodeTypesArray = new string[roomNodeTypeList.list.Count];

            for (int i = 0; i < roomNodeTypeList.list.Count; i++) {
                if (roomNodeTypeList.list[i].displayInNodeGraphEditor) {
                    roomNodeTypesArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
                }
            }

            return roomNodeTypesArray;
        }



        /// <summary>
        /// Process RoomNode events
        /// </summary>
        /// <param name="_currentEvent"></param>
        public void ProcessEvents(Event _currentEvent) {
            switch (_currentEvent.type) {
                case EventType.MouseDown:
                    ProcessMouseDownEvent(_currentEvent);
                    break;

                case EventType.MouseDrag:
                    ProcessMouseDragEvent(_currentEvent);
                    break;

                case EventType.MouseUp:
                    ProcessMouseUpEvent(_currentEvent);
                    break;

                default:
                    break;
            }
        }


        /// <summary>
        /// Process mouse down event
        /// </summary>
        /// <param name="_currentEvent"></param>
        public void ProcessMouseDownEvent(Event _currentEvent) {
            if (_currentEvent.button == 0) {
                ProcessMouseDownLeft();
            }

            if (_currentEvent.button == 1) {
                ProcessMouseDownRight(_currentEvent);
            }
        }


        /// <summary>
        /// Process left click down event
        /// </summary>
        public void ProcessMouseDownLeft() {
            Selection.activeObject = this;

            isSelected = !isSelected;
        }



        /// <summary>
        /// Process right click down event
        /// </summary>
        /// <param name="_currentEvent"></param>
        public void ProcessMouseDownRight(Event _currentEvent) {
            roomNodeGraph.SetNodeToDrawConnectionLineFrom(this, _currentEvent.mousePosition);
        }



        /// <summary>
        /// Process mouse drag event
        /// </summary>
        /// <param name="_currentEvent"></param>
        public void ProcessMouseDragEvent(Event _currentEvent) {
            if (_currentEvent.button == 0) {
                ProcessMouseDragLeft(_currentEvent);
            }
        }


        /// <summary>
        /// Process left click drag event
        /// </summary>
        /// <param name="_currentEvent"></param>
        public void ProcessMouseDragLeft(Event _currentEvent) {
            if (!isSelected) {
                return;
            }

            isLeftClickDragging = true;
            DragNode(_currentEvent.delta);
        }



        /// <summary>
        /// Handle dragging the node
        /// </summary>
        /// <param name="_delta"></param>
        public void DragNode(Vector2 _delta) {
            rect.position += _delta;
            EditorUtility.SetDirty(this);
            GUI.changed = true;
        }



        /// <summary>
        /// Process mouse up event
        /// </summary>
        /// <param name="_currentEvent"></param>
        public void ProcessMouseUpEvent(Event _currentEvent) {
            if (_currentEvent.button == 0) {
                ProcessMouseUpLeft();
            }
        }



        /// <summary>
        /// Process left click up event
        /// </summary>
        public void ProcessMouseUpLeft() {
            if (isLeftClickDragging) {
                isLeftClickDragging = false;
            }
        }



        /// <summary>
        /// Check if the child room node is valid
        /// </summary>
        /// <param name="_childNodeRoomID"></param>
        /// <returns></returns>
        public bool IsChildRoomValid(string _childNodeRoomID) {
            // barrier 1
            if (id == _childNodeRoomID) return false;
            if (childRoomNodeIDList.Contains(_childNodeRoomID)) return false;
            if (parentRoomNodeIDList.Contains(_childNodeRoomID)) return false;


            // barrier 2
            RoomNodeSO currentRoomNode = roomNodeGraph.GetRoomNode(_childNodeRoomID);
            RoomNodeTypeSO currentRoomNodeType = currentRoomNode.roomNodeType;

            if (currentRoomNode.parentRoomNodeIDList.Count > 0) return false;


            // barrier 3
            if (currentRoomNodeType.isNone) return false;
            if (currentRoomNodeType.isCorridor && roomNodeType.isCorridor) return false;
            if (!currentRoomNodeType.isCorridor && !roomNodeType.isCorridor) return false;
            if (currentRoomNodeType.isCorridor && childRoomNodeIDList.Count >= Settings.MaxChildCorridors) return false;
            if (currentRoomNodeType.isEntrance) return false;
            if (!currentRoomNodeType.isCorridor && childRoomNodeIDList.Count > 0) return false;


            //  barrier 4
            bool isBossRoomNodeAlreadyConnected = false;
            foreach (RoomNodeSO roomNode in roomNodeGraph.roomNodeList) {
                if (roomNode.roomNodeType.isBossRoom && roomNode.parentRoomNodeIDList.Count > 0) {
                    isBossRoomNodeAlreadyConnected = true;
                }
            }

            if (currentRoomNodeType.isBossRoom && isBossRoomNodeAlreadyConnected) return false;

            // valid
            return true;
        }



        /// <summary>
        /// Add child room node id to room node
        /// </summary>
        /// <param name="_childRoomNodeID"></param>
        /// <returns></returns>
        public bool AddChildRoomNodeIDToRoomNode(string _childRoomNodeID) {
            if (!IsChildRoomValid(_childRoomNodeID)) {
                return false;
            }

            childRoomNodeIDList.Add(_childRoomNodeID);
            return true;
        }



        /// <summary>
        /// Remove child room node id from room node
        /// </summary>
        /// <param name="_childRoomNodeID"></param>
        /// <returns></returns>
        public bool RemoveChildRoomNodeIDFromRoomNode(string _childRoomNodeID) {
            if (childRoomNodeIDList.Contains(_childRoomNodeID)) {
                childRoomNodeIDList.Remove(_childRoomNodeID);
                return true;
            }

            return false;
        }



        /// <summary>
        /// Add parent room node id to room node
        /// </summary>
        /// <param name="_parentRoomNodeID"></param>
        /// <returns></returns>
        public bool AddParentRoomNodeIDToRoomNode(string _parentRoomNodeID) {
            if (!parentRoomNodeIDList.Contains(_parentRoomNodeID)) {
                parentRoomNodeIDList.Add(_parentRoomNodeID);
                return true;
            }

            return false;
        }



        /// <summary>
        /// Remove parent room node id from room node
        /// </summary>
        /// <param name="_parentRoomNodeID"></param>
        /// <returns></returns>
        public bool RemoveParentRoomNodeIDToRoomNode(string _parentRoomNodeID) {
            if (parentRoomNodeIDList.Contains(_parentRoomNodeID)) {
                parentRoomNodeIDList.Remove(_parentRoomNodeID);
                return true;
            }

            return false;
        }
#endif
        #endregion
    }
}

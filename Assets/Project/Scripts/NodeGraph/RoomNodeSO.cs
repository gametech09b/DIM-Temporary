using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DungeonGunner
{
    public class RoomNodeSO : ScriptableObject
    {
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
        /// <param name="rect"></param>
        /// <param name="roomNodeGraph"></param>
        /// <param name="roomNodeType"></param>
        public void Initialize(Rect rect, RoomNodeGraphSO roomNodeGraph, RoomNodeTypeSO roomNodeType)
        {
            this.rect = rect;
            this.id = Guid.NewGuid().ToString();
            this.name = "RoomNode";
            this.roomNodeGraph = roomNodeGraph;
            this.roomNodeType = roomNodeType;

            roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
        }



        /// <summary>
        /// Draw the room node
        /// </summary>
        /// <param name="nodeStyle"></param>
        public void Draw(GUIStyle nodeStyle)
        {
            GUILayout.BeginArea(rect, nodeStyle);
            EditorGUI.BeginChangeCheck();

            if (parentRoomNodeIDList.Count > 0 || roomNodeType.isEntrance)
            {
                EditorGUILayout.LabelField(roomNodeType.roomNodeTypeName);
            }
            else
            {

                int currentSelectedTypeIndex = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);
                int newSelectedTypeIndex = EditorGUILayout.Popup("", currentSelectedTypeIndex, GetRoomNodeTypesToDisplay());

                roomNodeType = roomNodeTypeList.list[newSelectedTypeIndex];

                RoomNodeTypeSO currentSelectedType = roomNodeTypeList.list[currentSelectedTypeIndex];
                RoomNodeTypeSO newSelectedType = roomNodeTypeList.list[newSelectedTypeIndex];

                // check invalid
                if (currentSelectedType.isCorridor && !newSelectedType.isCorridor
                || !currentSelectedType.isCorridor && newSelectedType.isCorridor
                || !currentSelectedType.isBossRoom && newSelectedType.isBossRoom)
                {
                    if (childRoomNodeIDList.Count <= 0) return;

                    for (int i = childRoomNodeIDList.Count - 1; i >= 0; i--)
                    {
                        string childRoomNodeID = childRoomNodeIDList[i];
                        RoomNodeSO childRoomNode = roomNodeGraph.GetRoomNodeByID(childRoomNodeID);

                        if (childRoomNode == null) continue;

                        RemoveChildRoomNodeIDFromRoomNode(childRoomNodeID);
                        childRoomNode.RemoveParentRoomNodeIDToRoomNode(id);
                    }
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(this);
            }

            GUILayout.EndArea();
        }



        /// <summary>
        /// Get the room node types to display in the popup
        /// </summary>
        /// <returns></returns>
        public string[] GetRoomNodeTypesToDisplay()
        {
            string[] roomNodeTypesArray = new string[roomNodeTypeList.list.Count];

            for (int i = 0; i < roomNodeTypeList.list.Count; i++)
            {
                if (roomNodeTypeList.list[i].displayInNodeGraphEditor)
                {
                    roomNodeTypesArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
                }
            }

            return roomNodeTypesArray;
        }



        /// <summary>
        /// Process RoomNode events
        /// </summary>
        /// <param name="currentEvent"></param>
        public void ProcessEvents(Event currentEvent)
        {
            switch (currentEvent.type)
            {
                case EventType.MouseDown:
                    ProcessMouseDownEvent(currentEvent);
                    break;

                case EventType.MouseDrag:
                    ProcessMouseDragEvent(currentEvent);
                    break;

                case EventType.MouseUp:
                    ProcessMouseUpEvent(currentEvent);
                    break;

                default:
                    break;
            }
        }


        /// <summary>
        /// Process mouse down event
        /// </summary>
        /// <param name="currentEvent"></param>
        public void ProcessMouseDownEvent(Event currentEvent)
        {
            if (currentEvent.button == 0)
            {
                ProcessLeftClickDownEvent();
            }

            if (currentEvent.button == 1)
            {
                ProcessRightClickDownEvent(currentEvent);
            }
        }


        /// <summary>
        /// Process left click down event
        /// </summary>
        public void ProcessLeftClickDownEvent()
        {
            Selection.activeObject = this;

            isSelected = !isSelected;
        }



        /// <summary>
        /// Process right click down event
        /// </summary>
        /// <param name="currentEvent"></param>
        public void ProcessRightClickDownEvent(Event currentEvent)
        {
            roomNodeGraph.SetNodeToDrawConnectionLineFrom(this, currentEvent.mousePosition);
        }



        /// <summary>
        /// Process mouse drag event
        /// </summary>
        /// <param name="currentEvent"></param>
        public void ProcessMouseDragEvent(Event currentEvent)
        {
            if (currentEvent.button == 0)
            {
                ProcessLeftClickDragEvent(currentEvent);
            }
        }


        /// <summary>
        /// Process left click drag event
        /// </summary>
        /// <param name="currentEvent"></param>
        public void ProcessLeftClickDragEvent(Event currentEvent)
        {
            if (!isSelected)
            {
                return;
            }

            isLeftClickDragging = true;
            DragNode(currentEvent.delta);
        }



        /// <summary>
        /// Handle dragging the node
        /// </summary>
        /// <param name="delta"></param>
        public void DragNode(Vector2 delta)
        {
            rect.position += delta;
            EditorUtility.SetDirty(this);
            GUI.changed = true;
        }



        /// <summary>
        /// Process mouse up event
        /// </summary>
        /// <param name="currentEvent"></param>
        public void ProcessMouseUpEvent(Event currentEvent)
        {
            if (currentEvent.button == 0)
            {
                ProcessLeftClickUpEvent();
            }
        }



        /// <summary>
        /// Process left click up event
        /// </summary>
        public void ProcessLeftClickUpEvent()
        {
            if (isLeftClickDragging)
            {
                isLeftClickDragging = false;
            }
        }



        public bool AddChildRoomNodeIDToRoomNode(string childRoomNodeID)
        {
            if (!IsChildRoomValid(childRoomNodeID))
            {
                return false;
            }

            childRoomNodeIDList.Add(childRoomNodeID);
            return true;
        }



        public bool IsChildRoomValid(string childNodeRoomID)
        {
            // barrier 1
            if (id == childNodeRoomID) return false;
            if (childRoomNodeIDList.Contains(childNodeRoomID)) return false;
            if (parentRoomNodeIDList.Contains(childNodeRoomID)) return false;


            // barrier 2
            RoomNodeSO currentRoomNode = roomNodeGraph.GetRoomNodeByID(childNodeRoomID);
            RoomNodeTypeSO currentRoomNodeType = currentRoomNode.roomNodeType;

            if (currentRoomNode.parentRoomNodeIDList.Count > 0) return false;


            // barrier 3
            if (currentRoomNodeType.isNone) return false;
            if (currentRoomNodeType.isCorridor && roomNodeType.isCorridor) return false;
            if (!currentRoomNodeType.isCorridor && !roomNodeType.isCorridor) return false;
            if (currentRoomNodeType.isCorridor && childRoomNodeIDList.Count >= Settings.maxChildCorridors) return false;
            if (currentRoomNodeType.isEntrance) return false;
            if (!currentRoomNodeType.isCorridor && childRoomNodeIDList.Count > 0) return false;


            //  barrier 4
            bool isBossRoomNodeAlreadyConnected = false;
            foreach (RoomNodeSO roomNode in roomNodeGraph.roomNodeList)
            {
                if (roomNode.roomNodeType.isBossRoom && roomNode.parentRoomNodeIDList.Count > 0)
                {
                    isBossRoomNodeAlreadyConnected = true;
                }
            }

            if (currentRoomNodeType.isBossRoom && isBossRoomNodeAlreadyConnected) return false;

            // valid
            return true;
        }



        public bool RemoveChildRoomNodeIDFromRoomNode(string childRoomNodeID)
        {
            if (childRoomNodeIDList.Contains(childRoomNodeID))
            {
                childRoomNodeIDList.Remove(childRoomNodeID);
                return true;
            }

            return false;
        }



        public bool AddParentRoomNodeIDToRoomNode(string parentRoomNodeID)
        {
            if (!parentRoomNodeIDList.Contains(parentRoomNodeID))
            {
                parentRoomNodeIDList.Add(parentRoomNodeID);
                return true;
            }

            return false;
        }



        public bool RemoveParentRoomNodeIDToRoomNode(string parentRoomNodeID)
        {
            if (parentRoomNodeIDList.Contains(parentRoomNodeID))
            {
                parentRoomNodeIDList.Remove(parentRoomNodeID);
                return true;
            }

            return false;
        }
#endif
        #endregion
    }
}

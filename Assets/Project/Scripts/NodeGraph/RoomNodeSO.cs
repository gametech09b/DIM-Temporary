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

                int defaultSelectedType = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);
                int selectTypePopup = EditorGUILayout.Popup("", defaultSelectedType, GetRoomNodeTypesToDisplay());

                roomNodeType = roomNodeTypeList.list[selectTypePopup];
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
            if (isSelected)
            {
                isSelected = false;
            }

            if (isLeftClickDragging)
            {
                isLeftClickDragging = false;
            }
        }



        public bool AddChildRoomNodeIDToRoomNode(string childRoomNodeID)
        {
            if (!childRoomNodeIDList.Contains(childRoomNodeID))
            {
                childRoomNodeIDList.Add(childRoomNodeID);
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
#endif
        #endregion
    }
}

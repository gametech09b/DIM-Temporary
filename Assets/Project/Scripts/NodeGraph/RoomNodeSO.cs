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

        public void Initialize(Rect rect, RoomNodeGraphSO roomNodeGraph, RoomNodeTypeSO roomNodeType)
        {
            this.rect = rect;
            this.id = Guid.NewGuid().ToString();
            this.name = "RoomNode";
            this.roomNodeGraph = roomNodeGraph;
            this.roomNodeType = roomNodeType;

            roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
        }

        public void Draw(GUIStyle nodeStyle)
        {
            GUILayout.BeginArea(rect, nodeStyle);
            EditorGUI.BeginChangeCheck();

            int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);
            int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());

            roomNodeType = roomNodeTypeList.list[selection];

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(this);
            }

            GUILayout.EndArea();
        }



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
#endif
        #endregion
    }
}

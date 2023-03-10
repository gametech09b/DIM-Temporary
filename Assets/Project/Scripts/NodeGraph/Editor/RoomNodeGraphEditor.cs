using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace DungeonGunner
{
    public class RoomNodeGraphEditor : EditorWindow
    {
        private static RoomNodeGraphSO currentRoomNodeGraph;
        private RoomNodeSO currentRoomNode;
        private RoomNodeTypeListSO roomNodeTypeList;

        // room node view values
        private GUIStyle roomNodeStyle;
        private GUIStyle roomNodeSelectedStyle;
        private const float roomNodeWidth = 160f;
        private const float roomNodeHeight = 75f;
        private const int roomNodePadding = 25;
        private const int roomNodeBorder = 12;

        // line view values
        private const float lineThickness = 3f;
        private const float lineArrowSize = 6f;



        [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]
        private static void OpenWindow()
        {
            GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
        }



        private void OnEnable()
        {
            // subscribe to selection change event
            Selection.selectionChanged += OnSelectionChanged;

            // define node layout style 
            roomNodeStyle = new GUIStyle();
            roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            roomNodeStyle.normal.textColor = Color.white;
            roomNodeStyle.padding = new RectOffset(roomNodePadding, roomNodePadding, roomNodePadding, roomNodePadding);
            roomNodeStyle.border = new RectOffset(roomNodeBorder, roomNodeBorder, roomNodeBorder, roomNodeBorder);

            // define node selected layout style
            roomNodeSelectedStyle = new GUIStyle();
            roomNodeSelectedStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;
            roomNodeSelectedStyle.normal.textColor = Color.white;
            roomNodeSelectedStyle.padding = new RectOffset(roomNodePadding, roomNodePadding, roomNodePadding, roomNodePadding);
            roomNodeSelectedStyle.border = new RectOffset(roomNodeBorder, roomNodeBorder, roomNodeBorder, roomNodeBorder);

            // load room node type list
            roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
        }



        private void OnDisable()
        {
            // unsubscribe from selection change event
            Selection.selectionChanged -= OnSelectionChanged;
        }



        private void OnGUI()
        {
            if (currentRoomNodeGraph != null)
            {
                DrawDraggedLine();
                ProcessEvents(Event.current);
                DrawRoomConnections();
                DrawRoomNodes();
            }

            if (GUI.changed)
            {
                Repaint();
            }
        }



        /// <summary>
        /// Handle Selection.selectionChanged event
        /// </summary>
        private void OnSelectionChanged()
        {
            ChangeRoomNodeGraphSelection(Selection.activeObject as RoomNodeGraphSO);
        }



        /// <summary>
        /// Change the current room node graph
        /// </summary>
        /// <param name="roomNodeGraph"></param>
        private void ChangeRoomNodeGraphSelection(RoomNodeGraphSO roomNodeGraph)
        {
            if (roomNodeGraph != null)
            {
                currentRoomNodeGraph = roomNodeGraph;
            }
        }



        /// <summary>
        /// Create a room node at the mouse position
        /// </summary>
        /// <param name="mousePositionObject"></param>
        private void CreateRoomNode(object mousePositionObject)
        {
            if (currentRoomNodeGraph.roomNodeList.Count == 0)
            {
                CreateRoomNode(new Vector2(200f, 200f), roomNodeTypeList.list.Find(x => x.isEntrance));
            }

            CreateRoomNode(mousePositionObject, roomNodeTypeList.list.Find(x => x.isNone));
        }



        /// <summary>
        /// Create a room node at the mouse position - with a specific room node type
        /// </summary>
        /// <param name="mousePositionObject"></param>
        /// <param name="roomNodeType"></param>
        private void CreateRoomNode(object mousePositionObject, RoomNodeTypeSO roomNodeType)
        {
            Vector2 mousePosition = (Vector2)mousePositionObject;

            RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();

            currentRoomNodeGraph.roomNodeList.Add(roomNode);

            roomNode.Initialize(new Rect(mousePosition, new Vector2(roomNodeWidth, roomNodeHeight)), currentRoomNodeGraph, roomNodeType);

            AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);
            AssetDatabase.SaveAssets();

            currentRoomNodeGraph.OnValidate();
        }



        /// <summary>
        /// Draw line from roomNodeToDrawLineFrom to mouse position
        /// </summary>
        private void DrawDraggedLine()
        {
            if (currentRoomNodeGraph.linePosition == Vector2.zero)
            {
                return;
            }

            Vector2 startPosition = currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center;
            Vector2 endPosition = currentRoomNodeGraph.linePosition;
            Color color = Color.white;

            Handles.DrawBezier(
                startPosition, endPosition,
                startPosition, endPosition,
                color, null, lineThickness
            );
        }



        /// <summary>
        /// Draw connections between room nodes
        /// </summary>
        private void DrawRoomConnections()
        {
            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
            {
                if (roomNode.childRoomNodeIDList.Count <= 0) continue;

                foreach (string childRoomNodeID in roomNode.childRoomNodeIDList)
                {
                    if (!currentRoomNodeGraph.roomNodeDictionary.ContainsKey(childRoomNodeID)) continue;

                    DrawConnectionLine(roomNode, currentRoomNodeGraph.roomNodeDictionary[childRoomNodeID]);
                    GUI.changed = true;
                }
            }
        }



        /// <summary>
        /// Draw line between two room nodes
        /// </summary>
        /// <param name="parentRoomNode"></param>
        /// <param name="childRoomNode"></param>
        private void DrawConnectionLine(RoomNodeSO parentRoomNode, RoomNodeSO childRoomNode)
        {
            Color color = Color.white;

            // line view
            Vector2 startPosition = parentRoomNode.rect.center;
            Vector2 endPosition = childRoomNode.rect.center;

            // arrow view
            Vector2 direction = endPosition - startPosition;
            Vector2 middlePoint = (startPosition + endPosition) / 2f;

            Vector2 arrowHeadPoint = middlePoint + direction.normalized * lineArrowSize;
            Vector2 arrowTailPoint1 = middlePoint - new Vector2(-direction.y, direction.x).normalized * lineArrowSize;
            Vector2 arrowTailPoint2 = middlePoint + new Vector2(-direction.y, direction.x).normalized * lineArrowSize;

            // draw line
            Handles.DrawBezier(
                startPosition, endPosition,
                startPosition, endPosition,
                color, null, lineThickness
            );

            // draw arrow
            Handles.DrawBezier(
                arrowHeadPoint, arrowTailPoint1,
                arrowHeadPoint, arrowTailPoint1,
                color, null, lineThickness
            );
            Handles.DrawBezier(
                arrowHeadPoint, arrowTailPoint2,
                arrowHeadPoint, arrowTailPoint2,
                color, null, lineThickness
            );

            GUI.changed = true;
        }



        /// <summary>
        /// Draw room nodes in the RoomNodeGraph window
        /// </summary>
        private void DrawRoomNodes()
        {
            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
            {
                if (roomNode.isSelected)
                {
                    roomNode.Draw(roomNodeSelectedStyle);
                }
                else
                {
                    roomNode.Draw(roomNodeStyle);
                }
            }

            GUI.changed = true;
        }



        /// <summary>
        /// Open the window when double clicking on a RoomNodeGraphSO asset
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        [OnOpenAsset(0)]
        public static bool OnDoubleClickAsset(int instanceID, int line)
        {
            RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;

            if (roomNodeGraph == null)
            {
                return false;
            }

            OpenWindow();
            currentRoomNodeGraph = roomNodeGraph;

            return true;
        }



        /// <summary>
        /// Process all events
        /// </summary>
        /// <param name="currentEvent"></param>
        private void ProcessEvents(Event currentEvent)
        {
            if (currentEvent.keyCode == KeyCode.Delete)
            {
                DeleteSelectedRoomNodes();
            }

            if (currentRoomNode == null || !currentRoomNode.isLeftClickDragging)
            {
                currentRoomNode = IsMouseOverRoomNode(currentEvent);
            }

            if (currentRoomNode == null || currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
            {
                ProcessRoomNodeGraphEvents(currentEvent);
            }
            else
            {
                currentRoomNode.ProcessEvents(currentEvent);
            }
        }



        /// <summary>
        /// Check if mouse is over a room node
        /// </summary>
        /// <param name="currentEvent"></param>
        /// <returns></returns>
        private RoomNodeSO IsMouseOverRoomNode(Event currentEvent)
        {
            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
            {
                if (roomNode.rect.Contains(currentEvent.mousePosition))
                {
                    return roomNode;
                }
            }

            return null;
        }



        /// <summary>
        /// Process RoomNodeGraph events
        /// </summary>
        /// <param name="currentEvent"></param>
        private void ProcessRoomNodeGraphEvents(Event currentEvent)
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
        /// Process mouse down events on the RoomNodeGraph (not over a node)
        /// </summary>
        /// <param name="currentEvent"></param>
        private void ProcessMouseDownEvent(Event currentEvent)
        {
            if (currentEvent.button == 0)
            {
                ProcessLeftClickDownEvent(currentEvent);
            }

            if (currentEvent.button == 1)
            {
                ProcessRightClickDownEvent(currentEvent);
            }
        }



        /// <summary>
        /// Process left mouse down events on the RoomNodeGraph (not over a node)
        /// </summary>
        /// <param name="currentEvent"></param>
        private void ProcessLeftClickDownEvent(Event currentEvent)
        {
            ClearLineDrag();
            ClearAllSelectedRoomNodes();
        }



        /// <summary>
        /// Process right mouse down events on the RoomNodeGraph (not over a node)
        /// </summary>
        /// <param name="currentEvent"></param>
        private void ProcessRightClickDownEvent(Event currentEvent)
        {
            if (currentRoomNodeGraph.roomNodeToDrawLineFrom == null)
            {
                ShowContextMenu(currentEvent.mousePosition);
            }
            else
            {
                currentRoomNodeGraph.roomNodeToDrawLineFrom = null;
            }
        }



        /// <summary>
        /// Show the RoomNodeGraphSO context menu
        /// </summary>
        /// <param name="mousePosition"></param>
        private void ShowContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition);

            genericMenu.AddSeparator("");
            genericMenu.AddItem(new GUIContent("Select All Room Nodes"), false, SelectAllRoomNodes);

            genericMenu.AddSeparator("");
            genericMenu.AddItem(new GUIContent("Delete Selected Room Node Links"), false, DeleteSelectedRoomNodeLinks);
            genericMenu.AddItem(new GUIContent("Delete Selected Room Nodes"), false, DeleteSelectedRoomNodes);

            genericMenu.ShowAsContext();
        }



        /// <summary>
        /// Clear selection of all room nodes
        /// </summary>
        private void ClearAllSelectedRoomNodes()
        {
            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
            {
                if (!roomNode.isSelected) continue;

                roomNode.isSelected = false;
                GUI.changed = true;
            }
        }



        /// <summary>
        /// Select all room nodes
        /// </summary>
        private void SelectAllRoomNodes()
        {
            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
            {
                if (roomNode.isSelected) continue;

                roomNode.isSelected = true;
                GUI.changed = true;
            }
        }



        /// <summary>
        /// Delete selected room node links
        /// </summary>
        private void DeleteSelectedRoomNodeLinks()
        {
            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
            {
                if (roomNode.childRoomNodeIDList.Count <= 0) continue;
                if (!roomNode.isSelected) continue;

                string roomNodeID = roomNode.id;
                for (int i = roomNode.childRoomNodeIDList.Count - 1; i >= 0; i--)
                {
                    string childRoomNodeID = roomNode.childRoomNodeIDList[i];
                    RoomNodeSO childRoomNode = currentRoomNodeGraph.GetRoomNodeByID(childRoomNodeID);

                    if (childRoomNode == null) continue;
                    if (!childRoomNode.isSelected) continue;

                    roomNode.RemoveChildRoomNodeIDFromRoomNode(childRoomNodeID);
                    childRoomNode.RemoveParentRoomNodeIDToRoomNode(roomNodeID);
                }
            }
        }



        /// <summary>
        /// Delete selected room nodes
        /// </summary>
        private void DeleteSelectedRoomNodes()
        {
            Queue<RoomNodeSO> roomNodeToDeleteQueue = new Queue<RoomNodeSO>();

            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
            {
                if (!roomNode.isSelected) continue;
                if (roomNode.roomNodeType.isEntrance) continue;

                roomNodeToDeleteQueue.Enqueue(roomNode);

                string roomNodeID = roomNode.id;
                foreach (string childRoomNodeID in roomNode.childRoomNodeIDList)
                {
                    RoomNodeSO childRoomNode = currentRoomNodeGraph.GetRoomNodeByID(childRoomNodeID);

                    if (childRoomNode == null) continue;

                    childRoomNode.RemoveParentRoomNodeIDToRoomNode(roomNodeID);
                }

                foreach (string parentRoomNodeID in roomNode.parentRoomNodeIDList)
                {
                    RoomNodeSO parentRoomNode = currentRoomNodeGraph.GetRoomNodeByID(parentRoomNodeID);

                    if (parentRoomNode == null) continue;

                    parentRoomNode.RemoveParentRoomNodeIDToRoomNode(roomNodeID);
                }
            }

            while (roomNodeToDeleteQueue.Count > 0)
            {
                RoomNodeSO roomNodeToDelete = roomNodeToDeleteQueue.Dequeue();

                currentRoomNodeGraph.roomNodeDictionary.Remove(roomNodeToDelete.id);
                currentRoomNodeGraph.roomNodeList.Remove(roomNodeToDelete);

                DestroyImmediate(roomNodeToDelete, true);
                AssetDatabase.SaveAssets();
            }
        }



        /// <summary>
        /// Process mouse drag events on the RoomNodeGraph
        /// </summary>
        /// <param name="currentEvent"></param>
        private void ProcessMouseDragEvent(Event currentEvent)
        {
            if (currentEvent.button == 1)
            {
                ProcessRightClickDragEvent(currentEvent);
            }
        }



        /// <summary>
        /// Process right mouse drag events on the RoomNodeGraph
        /// </summary>
        /// <param name="currentEvent"></param>
        private void ProcessRightClickDragEvent(Event currentEvent)
        {
            if (currentRoomNodeGraph.roomNodeToDrawLineFrom == null) return;

            DragConnectingLine(currentEvent.delta);
        }



        /// <summary>
        /// Drag the connecting line from a room node to the mouse position
        /// </summary>
        /// <param name="delta"></param>
        private void DragConnectingLine(Vector2 delta)
        {
            currentRoomNodeGraph.linePosition += delta;
            GUI.changed = true;
        }



        /// <summary>
        /// Process mouse up events on the RoomNodeGraph
        /// </summary>
        /// <param name="currentEvent"></param>
        private void ProcessMouseUpEvent(Event currentEvent)
        {
            if (currentEvent.button == 1)
            {
                ProcessRightClickUpEvent(currentEvent);
            }
        }

        /// <summary>
        /// Process right mouse up events on the RoomNodeGraph
        /// </summary>
        /// <param name="currentEvent"></param>
        private void ProcessRightClickUpEvent(Event currentEvent)
        {
            if (currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
            {
                RoomNodeSO roomNodeToConnectTo = IsMouseOverRoomNode(currentEvent);

                if (roomNodeToConnectTo == null) return;

                ConnectRoomNodes(currentRoomNodeGraph.roomNodeToDrawLineFrom, roomNodeToConnectTo);
            }

            ClearLineDrag();
        }



        /// <summary>
        /// Connect two room nodes
        /// </summary>
        /// <param name="roomNodeFrom"></param>
        /// <param name="roomNodeTo"></param>
        private void ConnectRoomNodes(RoomNodeSO roomNodeFrom, RoomNodeSO roomNodeTo)
        {
            if (roomNodeFrom == roomNodeTo)
            {
                return;
            }

            if (roomNodeFrom.AddChildRoomNodeIDToRoomNode(roomNodeTo.id))
            {
                roomNodeTo.AddParentRoomNodeIDToRoomNode(roomNodeFrom.id);
            }
        }



        /// <summary>
        /// Clear the line drag
        /// </summary>
        private void ClearLineDrag()
        {
            currentRoomNodeGraph.roomNodeToDrawLineFrom = null;
            currentRoomNodeGraph.linePosition = Vector2.zero;
        }
    }
}

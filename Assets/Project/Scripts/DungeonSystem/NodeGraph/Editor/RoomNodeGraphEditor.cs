using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace DIM.DungeonSystem {
    public class RoomNodeGraphEditor : EditorWindow {
        private static RoomNodeGraphSO currentRoomNodeGraph;
        private RoomNodeSO currentRoomNode;
        private RoomNodeTypeListSO roomNodeTypeList;

        private Vector2 graphOffset;
        private Vector2 graphDrag;

        // grid view values
        private Color gridColor;
        private const float gridLargeSpacing = 100f;
        private const float gridSmallSpacing = 25f;

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

        // ===================================================================

        [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]
        private static void OpenWindow() {
            GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
        }



        /// <summary>
        /// Handle double click on RoomNodeGraphSO asset
        /// </summary>
        /// <param name="_instanceID"></param>
        /// <param name="_line"></param>
        /// <returns></returns>
        [OnOpenAsset(0)]
        public static bool OnDoubleClickAsset(int _instanceID, int _line) {
            RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(_instanceID) as RoomNodeGraphSO;

            if (roomNodeGraph == null) {
                return false;
            }

            OpenWindow();
            currentRoomNodeGraph = roomNodeGraph;

            return true;
        }


        private void OnEnable() {
            gridColor = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).name == "DarkSkin"
                                    ? Color.black : Color.gray;

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
            roomNodeTypeList = GameResources.Instance.RoomNodeTypeList;
        }



        private void OnDisable() {
            // unsubscribe from selection change event
            Selection.selectionChanged -= OnSelectionChanged;
        }



        private void OnGUI() {
            if (currentRoomNodeGraph != null) {
                Color gridColor = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).name == "DarkSkin"
                                    ? Color.black : Color.gray;
                DrawBackgroundGrid(gridColor);

                DrawDraggedLine();

                ProcessEvents(Event.current);

                DrawRoomConnections();

                DrawRoomNodes();
            }

            if (GUI.changed)
                Repaint();
        }



        /// <summary>
        /// Handle Selection.selectionChanged event
        /// </summary>
        private void OnSelectionChanged() {
            ChangeRoomNodeGraphSelection(Selection.activeObject as RoomNodeGraphSO);
        }



        /// <summary>
        /// Change the current room node graph
        /// </summary>
        /// <param name="_roomNodeGraph"></param>
        private void ChangeRoomNodeGraphSelection(RoomNodeGraphSO _roomNodeGraph) {
            if (_roomNodeGraph != null)
                currentRoomNodeGraph = _roomNodeGraph;
        }



        /// <summary>
        /// Draw the background grid
        /// </summary>
        /// <param name="_color"></param>
        private void DrawBackgroundGrid(Color _color) {
            DrawGrid(gridSmallSpacing, _color, 0.2f);
            DrawGrid(gridLargeSpacing, _color, 0.3f);
        }



        /// <summary>
        /// Draw the grid
        /// </summary>
        /// <param name="_gridSpacing"></param>
        /// <param name="_gridColor"></param>
        /// <param name="_gridOpacity"></param>
        private void DrawGrid(float _gridSpacing, Color _gridColor, float _gridOpacity) {
            int verticalLineCount = Mathf.CeilToInt((position.width + _gridSpacing) / _gridSpacing);
            int horizontalLineCount = Mathf.CeilToInt((position.height + _gridSpacing) / _gridSpacing);

            Handles.color = new Color(_gridColor.r, _gridColor.g, _gridColor.b, _gridOpacity);

            graphOffset += graphDrag * 0.5f;

            Vector3 gridOffset = new Vector3(graphOffset.x % _gridSpacing, graphOffset.y % _gridSpacing, 0);

            for (int i = 0; i < verticalLineCount; i++) {
                Vector3 start = new Vector3(_gridSpacing * i, -_gridSpacing, 0) + gridOffset;
                Vector3 end = new Vector3(_gridSpacing * i, position.height, 0f) + gridOffset;

                Handles.DrawLine(start, end);
            }

            for (int i = 0; i < horizontalLineCount; i++) {
                Vector3 start = new Vector3(-_gridSpacing, _gridSpacing * i, 0) + gridOffset;
                Vector3 end = new Vector3(position.width, _gridSpacing * i, 0f) + gridOffset;

                Handles.DrawLine(start, end);
            }

            Handles.color = Color.white;
        }



        /// <summary>
        /// Draw line from roomNodeToDrawLineFrom to mouse position
        /// </summary>
        private void DrawDraggedLine() {
            if (currentRoomNodeGraph.linePosition == Vector2.zero)
                return;

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
        /// Process all events
        /// </summary>
        /// <param name="_currentEvent"></param>
        private void ProcessEvents(Event _currentEvent) {
            graphDrag = Vector2.zero;

            // FIXME: Development only
            // #region DevOnly
            // if (_currentEvent.type == EventType.KeyDown && _currentEvent.keyCode == KeyCode.Delete)
            //     DeleteSelectedRoomNodes();
            // else if (_currentEvent.type == EventType.KeyDown && _currentEvent.keyCode == KeyCode.Z)
            //     CreateRoomNode((object)_currentEvent.mousePosition, roomNodeTypeList.list.Find(x => x.isCorridor));
            // #endregion

            if (currentRoomNode == null || !currentRoomNode.isLeftClickDragging)
                currentRoomNode = IsMouseOverRoomNode(_currentEvent);

            if (currentRoomNode == null || currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
                ProcessRoomNodeGraphEvents(_currentEvent);
            else
                currentRoomNode.ProcessEvents(_currentEvent);
        }



        /// <summary>
        /// Process RoomNodeGraph events
        /// </summary>
        /// <param name="_currentEvent"></param>
        private void ProcessRoomNodeGraphEvents(Event _currentEvent) {
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
        /// Create a room node at the mouse position
        /// </summary>
        /// <param name="_mousePositionObject"></param>
        private void CreateRoomNode(object _mousePositionObject) {
            if (currentRoomNodeGraph.roomNodeList.Count == 0)
                CreateRoomNode(new Vector2(200f, 200f), roomNodeTypeList.list.Find(x => x.isEntrance));

            CreateRoomNode(_mousePositionObject, roomNodeTypeList.list.Find(x => x.isNone));
        }



        /// <summary>
        /// Create a room node at the mouse position - with a specific room node type
        /// </summary>
        /// <param name="_mousePositionObject"></param>
        /// <param name="_roomNodeType"></param>
        private void CreateRoomNode(object _mousePositionObject, RoomNodeTypeSO _roomNodeType) {
            Vector2 mousePosition = (Vector2)_mousePositionObject;

            RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();

            currentRoomNodeGraph.roomNodeList.Add(roomNode);

            roomNode.Initialize(new Rect(mousePosition, new Vector2(roomNodeWidth, roomNodeHeight)), currentRoomNodeGraph, _roomNodeType);

            AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);
            AssetDatabase.SaveAssets();

            currentRoomNodeGraph.OnValidate();
        }



        /// <summary>
        /// Draw connections between room nodes
        /// </summary>
        private void DrawRoomConnections() {
            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList) {
                if (roomNode.childRoomNodeIDList.Count <= 0) continue;

                foreach (string childRoomNodeID in roomNode.childRoomNodeIDList) {
                    if (!currentRoomNodeGraph.roomNodeDictionary.ContainsKey(childRoomNodeID)) continue;

                    DrawConnectionLine(roomNode, currentRoomNodeGraph.roomNodeDictionary[childRoomNodeID]);
                    GUI.changed = true;
                }
            }
        }



        /// <summary>
        /// Draw line between two room nodes
        /// </summary>
        /// <param name="_parentRoomNode"></param>
        /// <param name="_childRoomNode"></param>
        private void DrawConnectionLine(RoomNodeSO _parentRoomNode, RoomNodeSO _childRoomNode) {
            Color color = Color.white;

            // line view
            Vector2 startPosition = _parentRoomNode.rect.center;
            Vector2 endPosition = _childRoomNode.rect.center;

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
        private void DrawRoomNodes() {
            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList) {
                if (roomNode.isSelected)
                    roomNode.Draw(roomNodeSelectedStyle);
                else
                    roomNode.Draw(roomNodeStyle);
            }

            GUI.changed = true;
        }



        /// <summary>
        /// Process mouse down events on the RoomNodeGraph (not over a node)
        /// </summary>
        /// <param name="_currentEvent"></param>
        private void ProcessMouseDownEvent(Event _currentEvent) {
            if (_currentEvent.button == 0)
                ProcessMouseDownLeft(_currentEvent);

            if (_currentEvent.button == 1)
                ProcessMouseDownRight(_currentEvent);
        }



        /// <summary>
        /// Process left mouse down events on the RoomNodeGraph (not over a node)
        /// </summary>
        /// <param name="_currentEvent"></param>
        private void ProcessMouseDownLeft(Event _currentEvent) {
            ClearLineDrag();
            ClearAllSelectedRoomNodes();
        }



        /// <summary>
        /// Process right mouse down events on the RoomNodeGraph (not over a node)
        /// </summary>
        /// <param name="_currentEvent"></param>
        private void ProcessMouseDownRight(Event _currentEvent) {
            if (currentRoomNodeGraph.roomNodeToDrawLineFrom == null)
                ShowContextMenu(_currentEvent.mousePosition);
            else
                currentRoomNodeGraph.roomNodeToDrawLineFrom = null;
        }



        /// <summary>
        /// Show the RoomNodeGraphSO context menu
        /// </summary>
        /// <param name="_mousePosition"></param>
        private void ShowContextMenu(Vector2 _mousePosition) {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, _mousePosition);

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
        private void ClearAllSelectedRoomNodes() {
            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList) {
                if (!roomNode.isSelected)
                    continue;

                roomNode.isSelected = false;
                GUI.changed = true;
            }
        }



        /// <summary>
        /// Select all room nodes
        /// </summary>
        private void SelectAllRoomNodes() {
            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList) {
                if (roomNode.isSelected)
                    continue;

                roomNode.isSelected = true;
                GUI.changed = true;
            }
        }



        /// <summary>
        /// Delete selected room node links
        /// </summary>
        private void DeleteSelectedRoomNodeLinks() {
            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList) {
                if (roomNode.childRoomNodeIDList.Count <= 0)
                    continue;
                if (!roomNode.isSelected)
                    continue;

                string roomNodeID = roomNode.id;
                for (int i = roomNode.childRoomNodeIDList.Count - 1; i >= 0; i--) {
                    string childRoomNodeID = roomNode.childRoomNodeIDList[i];
                    RoomNodeSO childRoomNode = currentRoomNodeGraph.GetRoomNode(childRoomNodeID);

                    if (childRoomNode == null)
                        continue;
                    if (!childRoomNode.isSelected)
                        continue;

                    roomNode.RemoveChildRoomNodeIDFromRoomNode(childRoomNodeID);
                    childRoomNode.RemoveParentRoomNodeIDToRoomNode(roomNodeID);
                }
            }
        }



        /// <summary>
        /// Delete selected room nodes
        /// </summary>
        private void DeleteSelectedRoomNodes() {
            Queue<RoomNodeSO> roomNodeToDeleteQueue = new Queue<RoomNodeSO>();

            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList) {
                if (!roomNode.isSelected)
                    continue;
                if (roomNode.roomNodeType.isEntrance)
                    continue;

                roomNodeToDeleteQueue.Enqueue(roomNode);

                string roomNodeID = roomNode.id;
                foreach (string childRoomNodeID in roomNode.childRoomNodeIDList) {
                    RoomNodeSO childRoomNode = currentRoomNodeGraph.GetRoomNode(childRoomNodeID);

                    if (childRoomNode == null)
                        continue;

                    childRoomNode.RemoveParentRoomNodeIDToRoomNode(roomNodeID);
                }

                foreach (string parentRoomNodeID in roomNode.parentRoomNodeIDList) {
                    RoomNodeSO parentRoomNode = currentRoomNodeGraph.GetRoomNode(parentRoomNodeID);

                    if (parentRoomNode == null)
                        continue;

                    parentRoomNode.RemoveParentRoomNodeIDToRoomNode(roomNodeID);
                }
            }

            while (roomNodeToDeleteQueue.Count > 0) {
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
        /// <param name="_currentEvent"></param>
        private void ProcessMouseDragEvent(Event _currentEvent) {
            if (_currentEvent.button == 0)
                ProcessMouseDragLeft(_currentEvent);

            if (_currentEvent.button == 1)
                ProcessMouseDragRight(_currentEvent);
        }



        /// <summary>
        /// Process left mouse drag events on the RoomNodeGraph
        /// </summary>
        /// <param name="_currentEvent"></param>
        private void ProcessMouseDragLeft(Event _currentEvent) {
            DragGraph(_currentEvent.delta);
        }



        /// <summary>
        /// Drag graph from certain position
        /// </summary>
        /// <param name="_delta"></param>
        private void DragGraph(Vector2 _delta) {
            graphDrag = _delta;

            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList) {
                roomNode.DragNode(_delta);
            }

            GUI.changed = true;
        }



        /// <summary>
        /// Process right mouse drag events on the RoomNodeGraph
        /// </summary>
        /// <param name="_currentEvent"></param>
        private void ProcessMouseDragRight(Event _currentEvent) {
            if (currentRoomNodeGraph.roomNodeToDrawLineFrom == null)
                return;

            DragConnectingLine(_currentEvent.delta);
        }



        /// <summary>
        /// Drag the connecting line from a room node to the mouse position
        /// </summary>
        /// <param name="_delta"></param>
        private void DragConnectingLine(Vector2 _delta) {
            currentRoomNodeGraph.linePosition += _delta;
            GUI.changed = true;
        }



        /// <summary>
        /// Process mouse up events on the RoomNodeGraph
        /// </summary>
        /// <param name="_currentEvent"></param>
        private void ProcessMouseUpEvent(Event _currentEvent) {
            if (_currentEvent.button == 1)
                ProcessMouseUpRight(_currentEvent);
        }



        /// <summary>
        /// Process right mouse up events on the RoomNodeGraph
        /// </summary>
        /// <param name="_currentEvent"></param>
        private void ProcessMouseUpRight(Event _currentEvent) {
            if (currentRoomNodeGraph.roomNodeToDrawLineFrom != null) {
                RoomNodeSO roomNodeToConnectTo = IsMouseOverRoomNode(_currentEvent);

                if (roomNodeToConnectTo != null)
                    ConnectRoomNodes(currentRoomNodeGraph.roomNodeToDrawLineFrom, roomNodeToConnectTo);
            }

            ClearLineDrag();
        }



        /// <summary>
        /// Connect two room nodes
        /// </summary>
        /// <param name="_roomNodeFrom"></param>
        /// <param name="_roomNodeTo"></param>
        private void ConnectRoomNodes(RoomNodeSO _roomNodeFrom, RoomNodeSO _roomNodeTo) {
            if (_roomNodeFrom == _roomNodeTo)
                return;

            if (_roomNodeFrom.AddChildRoomNodeIDToRoomNode(_roomNodeTo.id))
                _roomNodeTo.AddParentRoomNodeIDToRoomNode(_roomNodeFrom.id);
        }



        /// <summary>
        /// Clear the line drag
        /// </summary>
        private void ClearLineDrag() {
            currentRoomNodeGraph.roomNodeToDrawLineFrom = null;
            currentRoomNodeGraph.linePosition = Vector2.zero;
        }



        /// <summary>
        /// Check if mouse is over a room node
        /// </summary>
        /// <param name="_currentEvent"></param>
        /// <returns></returns>
        private RoomNodeSO IsMouseOverRoomNode(Event _currentEvent) {
            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList) {
                if (roomNode.rect.Contains(_currentEvent.mousePosition))
                    return roomNode;
            }

            return null;
        }
    }
}

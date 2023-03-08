using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace DungeonGunner
{
    public class RoomNodeGraphEditor : EditorWindow
    {
        private GUIStyle roomNodeStyle;
        private static RoomNodeGraphSO currentRoomNodeGraph;
        private RoomNodeTypeListSO roomNodeTypeList;

        // node layout values
        private const float nodeWidth = 160f;
        private const float nodeHeight = 75f;
        private const int nodePadding = 25;
        private const int nodeBorder = 12;

        private void OnEnable()
        {
            // define node layout style 
            roomNodeStyle = new GUIStyle();
            roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            roomNodeStyle.normal.textColor = Color.white;
            roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
            roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

            // load room node type list
            roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
        }

        private void OnGUI()
        {
            if (currentRoomNodeGraph != null)
            {
                ProcessEvents(Event.current);
                DrawRoomNodes();
            }

            if (GUI.changed)
            {
                Repaint();
            }
        }

        [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]
        private static void OpenWindow()
        {
            GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
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

            if (roomNodeGraph != null)
            {
                OpenWindow();

                currentRoomNodeGraph = roomNodeGraph;

                return true;
            }
            return false;
        }



        /// <summary>
        /// Show the RoomNodeGraphSO context menu
        /// </summary>
        /// <param name="mousePosition"></param>
        private void ShowContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition);
            genericMenu.ShowAsContext();
        }



        /// <summary>
        /// Create a room node at the mouse position
        /// </summary>
        /// <param name="mousePositionObject"></param>
        private void CreateRoomNode(object mousePositionObject)
        {
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

            roomNode.Initialize(new Rect(mousePosition, new Vector2(nodeWidth, nodeHeight)), currentRoomNodeGraph, roomNodeType);

            AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);
            AssetDatabase.SaveAssets();
        }



        /// <summary>
        /// Draw room nodes in the RoomNodeGraph window
        /// </summary>
        private void DrawRoomNodes()
        {
            foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
            {
                roomNode.Draw(roomNodeStyle);
            }

            GUI.changed = true;
        }



        /// <summary>
        /// Process all events
        /// </summary>
        /// <param name="currentEvent"></param>
        private void ProcessEvents(Event currentEvent)
        {
            ProcessRoomNodeGraphEvents(currentEvent);
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
            if (currentEvent.button == 1)
            {
                ShowContextMenu(currentEvent.mousePosition);
            }
        }
    }
}

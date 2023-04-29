using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGunner
{
    public class AStarTest : MonoBehaviour
    {
        private RoomGameObject roomGameObject;
        private Grid grid;
        private Tilemap frontTilemap;
        private Tilemap pathTilemap;

        private Vector3Int startGridPosition;
        private Vector3Int endGridPosition;

        private TileBase startPathTile;
        private TileBase finishPathTile;

        private Vector3Int noValue = new Vector3Int(9999, 9999, 9999);

        private Stack<Vector3> pathStack;



        private void OnEnable()
        {
            DungeonStaticEvent.OnRoomChange += DungeonStaticEvent_OnRoomChange;
        }



        private void OnDisable()
        {
            DungeonStaticEvent.OnRoomChange -= DungeonStaticEvent_OnRoomChange;
        }


        private void Start()
        {
            startPathTile = GameResources.Instance.EnemyPreferredPathTile;
            finishPathTile = GameResources.Instance.EnemyUnwalkableTileArray[0];
        }



        private void DungeonStaticEvent_OnRoomChange(OnRoomChangeEventArgs _args)
        {
            pathStack = null;
            roomGameObject = _args.room.roomGameObject;
            frontTilemap = roomGameObject.transform.Find("Grid/Tilemap4_Front").GetComponent<Tilemap>();
            grid = roomGameObject.transform.GetComponentInChildren<Grid>();
            startGridPosition = noValue;
            endGridPosition = noValue;

            SetUpPathTilemap();
        }



        private void SetUpPathTilemap()
        {
            Transform tilemapCloneTransform = roomGameObject.transform.Find("Grid/Tilemap4_Front(Clone)");

            if (tilemapCloneTransform == null)
            {
                pathTilemap = Instantiate(frontTilemap, grid.transform);
                pathTilemap.GetComponent<TilemapRenderer>().sortingOrder = 2;
                pathTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.LitMaterial;
                pathTilemap.gameObject.tag = "Untagged";
            }
            else
            {
                pathTilemap = roomGameObject.transform.Find("Grid/Tilemap4_Front(Clone)").GetComponent<Tilemap>();
                pathTilemap.ClearAllTiles();
            }
        }



        private void Update()
        {
            if (roomGameObject == null
            || startPathTile == null
            || finishPathTile == null
            || grid == null
            || pathTilemap == null)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                ClearPath();
                SetStartPosition();
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                ClearPath();
                SetEndPosition();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                DisplayPath();
            }
        }



        private bool IsPositionWithinBounds(Vector3Int _position)
        {
            Vector2Int templateLowerBounds = roomGameObject.room.templateLowerBounds;
            Vector2Int templateUpperBounds = roomGameObject.room.templateUpperBounds;

            if (_position.x < templateLowerBounds.x
            || _position.x > templateUpperBounds.x
            || _position.y < templateLowerBounds.y
            || _position.y > templateUpperBounds.y)
            {
                return false;
            }

            return true;
        }



        private void SetStartPosition()
        {
            if (startGridPosition == noValue)
            {
                startGridPosition = grid.WorldToCell(HelperUtilities.GetMouseWorldPosition());

                if (!IsPositionWithinBounds(startGridPosition))
                {
                    startGridPosition = noValue;
                    return;
                }

                pathTilemap.SetTile(startGridPosition, startPathTile);
            }
            else
            {
                pathTilemap.SetTile(startGridPosition, null);
                startGridPosition = noValue;
            }
        }



        private void SetEndPosition()
        {
            if (endGridPosition == noValue)
            {
                endGridPosition = grid.WorldToCell(HelperUtilities.GetMouseWorldPosition());

                if (!IsPositionWithinBounds(endGridPosition))
                {
                    endGridPosition = noValue;
                    return;
                }

                pathTilemap.SetTile(endGridPosition, finishPathTile);
            }
            else
            {
                pathTilemap.SetTile(endGridPosition, null);
                endGridPosition = noValue;
            }
        }



        private void ClearPath()
        {
            if (pathStack == null) return;

            foreach (Vector3 cellWorldPosition in pathStack)
            {
                pathTilemap.SetTile(grid.WorldToCell(cellWorldPosition), null);
            }

            pathStack = null;

            endGridPosition = noValue;
            startGridPosition = noValue;
        }



        private void DisplayPath()
        {
            if (startGridPosition == noValue || endGridPosition == noValue) return;

            pathStack = AStarPathfinding.AStar.BuildPath(roomGameObject.room, startGridPosition, endGridPosition);

            if (pathStack == null) return;

            foreach (Vector3 cellWorldPosition in pathStack)
            {
                pathTilemap.SetTile(grid.WorldToCell(cellWorldPosition), startPathTile);
            }
        }
    }
}

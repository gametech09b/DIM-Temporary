using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider2D))]
    public class RoomGameObject : MonoBehaviour
    {
        [HideInInspector] public Room room;
        [HideInInspector] public Grid grid;


        [HideInInspector] public Tilemap groundTilemap;
        [HideInInspector] public Tilemap decorationTilemap1;
        [HideInInspector] public Tilemap decorationTilemap2;
        [HideInInspector] public Tilemap frontTilemap;
        [HideInInspector] public Tilemap collisionTilemap;
        [HideInInspector] public Tilemap minimapTilemap;
        [HideInInspector] public Bounds roomColliderBounds;


        private BoxCollider2D roomCollider;



        private void Awake()
        {
            roomCollider = GetComponent<BoxCollider2D>();
            roomColliderBounds = roomCollider.bounds;
        }



        public void Init(GameObject roomGameObject)
        {
            PopulateTilemapVariables(roomGameObject);

            BlockOffUnconnectedDoorways();

            DisableCollisionTilemapRenderer();
        }



        public void PopulateTilemapVariables(GameObject roomGameObject)
        {
            grid = roomGameObject.GetComponentInChildren<Grid>();

            Tilemap[] tilemapArray = grid.GetComponentsInChildren<Tilemap>();

            foreach (Tilemap tilemap in tilemapArray)
            {
                if (tilemap.CompareTag("groundTilemap"))
                {
                    groundTilemap = tilemap;
                }
                else if (tilemap.CompareTag("decoration1Tilemap"))
                {
                    decorationTilemap1 = tilemap;
                }
                else if (tilemap.CompareTag("decoration2Tilemap"))
                {
                    decorationTilemap2 = tilemap;
                }
                else if (tilemap.CompareTag("frontTilemap"))
                {
                    frontTilemap = tilemap;
                }
                else if (tilemap.CompareTag("collisionTilemap"))
                {
                    collisionTilemap = tilemap;
                }
                else if (tilemap.CompareTag("minimapTilemap"))
                {
                    minimapTilemap = tilemap;
                }
            }
        }



        private void BlockOffUnconnectedDoorways()
        {
            foreach (Doorway doorway in room.doorwayList)
            {
                if (doorway.isConnected) continue;

                if (collisionTilemap != null)
                {
                    BlockDoorwayOnTilemapLayer(doorway, collisionTilemap);
                }

                if (groundTilemap != null)
                {
                    BlockDoorwayOnTilemapLayer(doorway, groundTilemap);
                }

                if (decorationTilemap1 != null)
                {
                    BlockDoorwayOnTilemapLayer(doorway, decorationTilemap1);
                }

                if (decorationTilemap2 != null)
                {
                    BlockDoorwayOnTilemapLayer(doorway, decorationTilemap2);
                }

                if (frontTilemap != null)
                {
                    BlockDoorwayOnTilemapLayer(doorway, frontTilemap);
                }

                if (minimapTilemap != null)
                {
                    BlockDoorwayOnTilemapLayer(doorway, minimapTilemap);
                }
            }
        }



        private void BlockDoorwayOnTilemapLayer(Doorway doorway, Tilemap tilemap)
        {
            switch (doorway.orientation)
            {
                case Orientation.NORTH:
                case Orientation.SOUTH:
                    BlockDoorwayHorizontally(doorway, tilemap);
                    break;

                case Orientation.EAST:
                case Orientation.WEST:
                    BlockDoorwayVertically(doorway, tilemap);
                    break;

                case Orientation.NONE:
                    break;
            }
        }



        private void BlockDoorwayHorizontally(Doorway doorway, Tilemap tilemap)
        {
            Vector2Int startPosition = doorway.startCopyPosition;

            for (int xPosition = 0; xPosition < doorway.copyTileWidth; xPosition++)
            {
                for (int yPosition = 0; yPosition < doorway.copyTileHeight; yPosition++)
                {
                    Vector3Int tileToCopyPosition = new Vector3Int(startPosition.x + xPosition, startPosition.y - yPosition, 0);

                    Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(tileToCopyPosition);

                    TileBase tileToCopy = tilemap.GetTile(tileToCopyPosition);

                    Vector3Int tileToPastePosition = new Vector3Int(startPosition.x + 1 + xPosition, startPosition.y - yPosition, 0);
                    tilemap.SetTile(tileToPastePosition, tileToCopy);
                    tilemap.SetTransformMatrix(tileToPastePosition, transformMatrix);
                }
            }
        }



        private void BlockDoorwayVertically(Doorway doorway, Tilemap tilemap)
        {
            Vector2Int startPosition = doorway.startCopyPosition;

            for (int xPosition = 0; xPosition < doorway.copyTileWidth; xPosition++)
            {
                for (int yPosition = 0; yPosition < doorway.copyTileHeight; yPosition++)
                {
                    Vector3Int tileToCopyPosition = new Vector3Int(startPosition.x + xPosition, startPosition.y - yPosition, 0);

                    Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(tileToCopyPosition);

                    TileBase tileToCopy = tilemap.GetTile(tileToCopyPosition);

                    Vector3Int tileToPastePosition = new Vector3Int(startPosition.x + xPosition, startPosition.y - 1 - yPosition, 0);
                    tilemap.SetTile(tileToPastePosition, tileToCopy);
                    tilemap.SetTransformMatrix(tileToPastePosition, transformMatrix);
                }
            }
        }



        public void DisableCollisionTilemapRenderer()
        {
            collisionTilemap.GetComponent<TilemapRenderer>().enabled = false;
        }
    }
}

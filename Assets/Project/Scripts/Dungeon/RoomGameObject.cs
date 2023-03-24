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



        public void DisableCollisionTilemapRenderer()
        {
            collisionTilemap.GetComponent<TilemapRenderer>().enabled = false;
        }
    }
}

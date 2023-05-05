using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(BoxCollider2D))]
    #endregion
    public class RoomGameObject : MonoBehaviour {
        [HideInInspector] public Room room;
        [HideInInspector] public Grid grid;

        [HideInInspector] public int[,] aStarMovementPenaltyArray;
        [HideInInspector] public int[,] aStarItemObstaclePenaltyArray;

        [HideInInspector] public Tilemap groundTilemap;
        [HideInInspector] public Tilemap decorationTilemap1;
        [HideInInspector] public Tilemap decorationTilemap2;
        [HideInInspector] public Tilemap frontTilemap;
        [HideInInspector] public Tilemap collisionTilemap;
        [HideInInspector] public Tilemap minimapTilemap;

        [HideInInspector] public Bounds roomColliderBounds;
        [HideInInspector] public List<MoveableItem> moveableItemList = new List<MoveableItem>();

        private BoxCollider2D roomCollider;

        [SerializeField] private GameObject environmentParentGameObject;



        private void Awake() {
            roomCollider = GetComponent<BoxCollider2D>();
            roomColliderBounds = roomCollider.bounds;
        }



        private void Start() {
            UpdateMoveableObstacle();
        }



        private void OnTriggerEnter2D(Collider2D _other) {
            if (_other.CompareTag(Settings.PlayerTag) && room != GameManager.Instance.GetCurrentRoom()) {
                room.isVisited = true;

                DungeonStaticEvent.CallOnRoomChanged(room);
            }
        }



        public void Init(GameObject _roomGameObject) {
            PopulateTilemapVariables(_roomGameObject);

            BlockOffUnconnectedDoorways();

            AddObstacleAndPreferredPathForAStar();

            CreateItemObstacleArray();

            AddDoorsToRoom();

            DisableCollisionTilemapRenderer();
        }



        public void PopulateTilemapVariables(GameObject _roomGameObject) {
            grid = _roomGameObject.GetComponentInChildren<Grid>();

            Tilemap[] tilemapArray = grid.GetComponentsInChildren<Tilemap>();

            foreach (Tilemap tilemap in tilemapArray) {
                if (tilemap.CompareTag("groundTilemap"))
                    groundTilemap = tilemap;

                else if (tilemap.CompareTag("decoration1Tilemap"))
                    decorationTilemap1 = tilemap;

                else if (tilemap.CompareTag("decoration2Tilemap"))
                    decorationTilemap2 = tilemap;

                else if (tilemap.CompareTag("frontTilemap"))
                    frontTilemap = tilemap;

                else if (tilemap.CompareTag("collisionTilemap"))
                    collisionTilemap = tilemap;

                else if (tilemap.CompareTag("minimapTilemap"))
                    minimapTilemap = tilemap;
            }
        }



        private void BlockOffUnconnectedDoorways() {
            foreach (Doorway doorway in room.doorwayList) {
                if (doorway.isConnected) continue;

                if (collisionTilemap != null)
                    BlockDoorwayOnTilemapLayer(doorway, collisionTilemap);

                if (groundTilemap != null)
                    BlockDoorwayOnTilemapLayer(doorway, groundTilemap);

                if (decorationTilemap1 != null)
                    BlockDoorwayOnTilemapLayer(doorway, decorationTilemap1);

                if (decorationTilemap2 != null)
                    BlockDoorwayOnTilemapLayer(doorway, decorationTilemap2);

                if (frontTilemap != null)
                    BlockDoorwayOnTilemapLayer(doorway, frontTilemap);

                if (minimapTilemap != null)
                    BlockDoorwayOnTilemapLayer(doorway, minimapTilemap);
            }
        }



        private void BlockDoorwayOnTilemapLayer(Doorway _doorway, Tilemap _tilemap) {
            switch (_doorway.orientation) {
                case Orientation.NORTH:
                case Orientation.SOUTH:
                    BlockDoorwayHorizontally(_doorway, _tilemap);
                    break;

                case Orientation.EAST:
                case Orientation.WEST:
                    BlockDoorwayVertically(_doorway, _tilemap);
                    break;

                case Orientation.NONE:
                    break;
            }
        }



        private void BlockDoorwayHorizontally(Doorway _doorway, Tilemap _tilemap) {
            Vector2Int startPosition = _doorway.startCopyPosition;

            for (int xPosition = 0; xPosition < _doorway.copyTileWidth; xPosition++) {
                for (int yPosition = 0; yPosition < _doorway.copyTileHeight; yPosition++) {
                    Vector3Int tileToCopyPosition = new Vector3Int(startPosition.x + xPosition, startPosition.y - yPosition, 0);

                    Matrix4x4 transformMatrix = _tilemap.GetTransformMatrix(tileToCopyPosition);

                    TileBase tileToCopy = _tilemap.GetTile(tileToCopyPosition);

                    Vector3Int tileToPastePosition = new Vector3Int(startPosition.x + 1 + xPosition, startPosition.y - yPosition, 0);
                    _tilemap.SetTile(tileToPastePosition, tileToCopy);
                    _tilemap.SetTransformMatrix(tileToPastePosition, transformMatrix);
                }
            }
        }



        private void BlockDoorwayVertically(Doorway _doorway, Tilemap _tilemap) {
            Vector2Int startPosition = _doorway.startCopyPosition;

            for (int xPosition = 0; xPosition < _doorway.copyTileWidth; xPosition++) {
                for (int yPosition = 0; yPosition < _doorway.copyTileHeight; yPosition++) {
                    Vector3Int tileToCopyPosition = new Vector3Int(startPosition.x + xPosition, startPosition.y - yPosition, 0);

                    Matrix4x4 transformMatrix = _tilemap.GetTransformMatrix(tileToCopyPosition);

                    TileBase tileToCopy = _tilemap.GetTile(tileToCopyPosition);

                    Vector3Int tileToPastePosition = new Vector3Int(startPosition.x + xPosition, startPosition.y - 1 - yPosition, 0);
                    _tilemap.SetTile(tileToPastePosition, tileToCopy);
                    _tilemap.SetTransformMatrix(tileToPastePosition, transformMatrix);
                }
            }
        }



        public void DisableCollisionTilemapRenderer() {
            collisionTilemap.GetComponent<TilemapRenderer>().enabled = false;
        }



        public void AddObstacleAndPreferredPathForAStar() {
            int xSize = room.templateUpperBounds.x - room.templateLowerBounds.x + 1;
            int ySize = room.templateUpperBounds.y - room.templateLowerBounds.y + 1;

            aStarMovementPenaltyArray = new int[xSize, ySize];

            for (int x = 0; x < xSize; x++) {
                for (int y = 0; y < ySize; y++) {
                    aStarMovementPenaltyArray[x, y] = Settings.AStarDefaultMovementPenalty;

                    TileBase tile = collisionTilemap.GetTile(new Vector3Int(x + room.templateLowerBounds.x, y + room.templateLowerBounds.y, 0));

                    foreach (TileBase collisionTile in GameResources.Instance.EnemyUnwalkableTileArray) {
                        if (tile == collisionTile) {
                            aStarMovementPenaltyArray[x, y] = 0;
                            break;
                        }
                    }

                    if (tile == GameResources.Instance.EnemyPreferredPathTile)
                        aStarMovementPenaltyArray[x, y] = Settings.AStarPreferredPathMovementPenalty;
                }
            }
        }



        public void AddDoorsToRoom() {
            if (room.roomNodeType.isCorridorEW || room.roomNodeType.isCorridorNS) return;

            foreach (Doorway doorway in room.doorwayList) {
                if (doorway.doorPrefab == null) continue;
                if (!doorway.isConnected) continue;

                float tileDistance = Settings.TileSizePixel / Settings.PixelPerUnit;

                GameObject doorGameObject = null;

                switch (doorway.orientation) {
                    case Orientation.NORTH:
                        doorGameObject = Instantiate(doorway.doorPrefab, gameObject.transform);
                        doorGameObject.transform.localPosition = new Vector3(
                            doorway.position.x + tileDistance / 2f,
                            doorway.position.y + tileDistance,
                            0
                        );
                        break;

                    case Orientation.SOUTH:
                        doorGameObject = Instantiate(doorway.doorPrefab, gameObject.transform);
                        doorGameObject.transform.localPosition = new Vector3(
                            doorway.position.x + tileDistance / 2f,
                            doorway.position.y,
                            0
                        );
                        break;

                    case Orientation.EAST:
                        doorGameObject = Instantiate(doorway.doorPrefab, gameObject.transform);
                        doorGameObject.transform.localPosition = new Vector3(
                            doorway.position.x + tileDistance,
                            doorway.position.y + tileDistance * 1.25f,
                            0
                        );
                        break;

                    case Orientation.WEST:
                        doorGameObject = Instantiate(doorway.doorPrefab, gameObject.transform);
                        doorGameObject.transform.localPosition = new Vector3(
                            doorway.position.x,
                            doorway.position.y + tileDistance * 1.25f,
                            0
                        );
                        break;

                    case Orientation.NONE:
                        break;
                }

                DoorGameObject door = doorGameObject.GetComponent<DoorGameObject>();
                if (room.roomNodeType.isBossRoom) {
                    door.isBossRoomDoor = true;
                    door.LockDoor();
                }
            }
        }



        public int GetAStarMovementPenalty(Vector3 _worldPosition) {
            Vector3Int tilePosition = collisionTilemap.WorldToCell(_worldPosition);

            int x = tilePosition.x - room.templateLowerBounds.x;
            int y = tilePosition.y - room.templateLowerBounds.y;

            return aStarMovementPenaltyArray[x, y];
        }



        public int GetAStarMovementPenalty(int _x, int _y) {
            return aStarMovementPenaltyArray[_x, _y];
        }


        public int GetAStarItemObstaclePenalty(Vector3 _worldPosition) {
            Vector3Int tilePosition = collisionTilemap.WorldToCell(_worldPosition);

            int x = tilePosition.x - room.templateLowerBounds.x;
            int y = tilePosition.y - room.templateLowerBounds.y;

            return aStarItemObstaclePenaltyArray[x, y];
        }



        public int GetAStarItemObstaclePenalty(int _x, int _y) {
            return aStarItemObstaclePenaltyArray[_x, _y];
        }



        public void LockDoors() {
            foreach (DoorGameObject door in GetComponentsInChildren<DoorGameObject>()) {
                door.LockDoor();
            }

            DisableRoomCollider();
        }



        public void UnlockDoors(float _delay) {
            StartCoroutine(UnlockDoorsCoroutine(_delay));
        }



        private IEnumerator UnlockDoorsCoroutine(float _delay) {
            yield return new WaitForSeconds(_delay);

            foreach (DoorGameObject door in GetComponentsInChildren<DoorGameObject>()) {
                door.UnlockDoor();
            }

            EnableRoomCollider();
        }



        public void DisableRoomCollider() {
            roomCollider.enabled = false;
        }



        public void EnableRoomCollider() {
            roomCollider.enabled = true;
        }



        public void ActivateEnvironment() {
            if (environmentParentGameObject != null)
                environmentParentGameObject.SetActive(true);
        }



        public void DeactivateEnvironment() {
            if (environmentParentGameObject != null)
                environmentParentGameObject.SetActive(false);
        }



        private void CreateItemObstacleArray() {
            aStarItemObstaclePenaltyArray = new int[room.templateUpperBounds.x - room.templateLowerBounds.x + 1, room.templateUpperBounds.y - room.templateLowerBounds.y + 1];
        }



        private void InitItemObstacleArray() {
            int xSize = room.templateUpperBounds.x - room.templateLowerBounds.x + 1;
            int ySize = room.templateUpperBounds.y - room.templateLowerBounds.y + 1;

            for (int x = 0; x < xSize; x++) {
                for (int y = 0; y < ySize; y++) {
                    aStarItemObstaclePenaltyArray[x, y] = Settings.AStarDefaultMovementPenalty;
                }
            }
        }



        public void UpdateMoveableObstacle() {
            InitItemObstacleArray();

            foreach (MoveableItem moveableItem in moveableItemList) {
                Vector3Int minColliderBounds = grid.WorldToCell(moveableItem.boxCollider2D.bounds.min);
                Vector3Int maxColliderBounds = grid.WorldToCell(moveableItem.boxCollider2D.bounds.max);

                for (int x = minColliderBounds.x; x <= maxColliderBounds.x; x++) {
                    for (int y = minColliderBounds.y; y <= maxColliderBounds.y; y++) {
                        aStarItemObstaclePenaltyArray[x - room.templateLowerBounds.x, y - room.templateLowerBounds.y] = 0;
                    }
                }
            }
        }



        // FIXME: comment if not needed
        // private void OnDrawGizmos() {
        //     int xSize = room.templateUpperBounds.x - room.templateLowerBounds.x + 1;
        //     int ySize = room.templateUpperBounds.y - room.templateLowerBounds.y + 1;

        //     for (int x = 0; x < xSize; x++) {
        //         for (int y = 0; y < ySize; y++) {
        //             if (aStarItemObstaclePenaltyArray[x, y] == 0) {
        //                 Vector3 worldCellPosition = grid.CellToWorld(new Vector3Int(x + room.templateLowerBounds.x, y + room.templateLowerBounds.y, 0));

        //                 Gizmos.DrawWireCube(new Vector3(worldCellPosition.x + 0.5f, worldCellPosition.y + 0.5f, 0), Vector3.one);
        //             }
        //         }
        //     }
        // }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            environmentParentGameObject = transform.Find("Environment").gameObject;
            HelperUtilities.CheckNullValue(this, nameof(environmentParentGameObject), environmentParentGameObject);
        }
#endif
        #endregion
    }
}

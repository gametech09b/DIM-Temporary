using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    public class Map : SingletonMonobehaviour<Map> {
        [SerializeField] private GameObject minimapUI;
        private Camera mainCamera;
        private Camera mapCamera;



        private void Start() {
            mainCamera = Camera.main;

            Transform playerTransform = GameManager.Instance.GetCurrentPlayer().transform;

            CinemachineVirtualCamera cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            cinemachineVirtualCamera.Follow = playerTransform;

            mapCamera = GetComponentInChildren<Camera>();
            mapCamera.gameObject.SetActive(false);
        }



        private void Update() {
            if (Input.GetMouseButtonDown(0)
            && GameManager.Instance.gameState == GameState.DUNGEON_OVERVIEW_MAP)
                HandleRoomClicked();
        }



        private void HandleRoomClicked() {
            Vector3 worldPosition = mapCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition = new Vector3(worldPosition.x, worldPosition.y, 0f);

            Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(new Vector2(worldPosition.x, worldPosition.y), 1f);

            foreach (Collider2D collider2D in collider2DArray) {
                if (collider2D.GetComponent<RoomGameObject>() != null) {
                    RoomGameObject roomGameObject = collider2D.GetComponent<RoomGameObject>();

                    if (roomGameObject.room.isCleared
                    && roomGameObject.room.isVisited)
                        StartCoroutine(MovePlayerToRoom(worldPosition, roomGameObject.room));
                }
            }
        }



        private IEnumerator MovePlayerToRoom(Vector3 _worldPosition, Room _room) {
            DungeonStaticEvent.CallOnRoomChanged(_room);

            yield return StartCoroutine(GameManager.Instance.FadeCoroutine(0f, 1f, 0f, Color.black));

            HideMap();

            GameManager.Instance.GetCurrentPlayer().controllerHandler.DisableController();

            Vector2 movePosition = HelperUtilities.GetNearestSpawnPoint(_worldPosition);

            GameManager.Instance.GetCurrentPlayer().transform.position = movePosition;

            yield return StartCoroutine(GameManager.Instance.FadeCoroutine(1f, 0f, 0f, Color.black));

            GameManager.Instance.GetCurrentPlayer().controllerHandler.EnableController();
        }



        public void DisplayMap() {
            GameManager.Instance.previousGameState = GameManager.Instance.gameState;
            GameManager.Instance.gameState = GameState.DUNGEON_OVERVIEW_MAP;

            GameManager.Instance.GetCurrentPlayer().controllerHandler.DisableController();

            mainCamera.gameObject.SetActive(false);
            mapCamera.gameObject.SetActive(true);

            ActivateRoomsForDisplay();

            minimapUI.SetActive(false);
        }



        public void HideMap() {
            GameManager.Instance.gameState = GameManager.Instance.previousGameState;
            GameManager.Instance.previousGameState = GameState.DUNGEON_OVERVIEW_MAP;

            GameManager.Instance.GetCurrentPlayer().controllerHandler.EnableController();

            mainCamera.gameObject.SetActive(true);
            mapCamera.gameObject.SetActive(false);

            minimapUI.SetActive(true);
        }



        private void ActivateRoomsForDisplay() {
            foreach (KeyValuePair<string, Room> roomDictionaryKVP in DungeonBuilder.Instance.roomDictionary) {
                Room room = roomDictionaryKVP.Value;

                room.roomGameObject.gameObject.SetActive(true);
            }
        }
    }
}

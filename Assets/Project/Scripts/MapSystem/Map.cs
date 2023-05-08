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



        public void DisplayMap() {
            GameManager.Instance.previousGameState = GameManager.Instance.gameState;
            GameManager.Instance.gameState = GameState.DUNGEON_OVERVIEW_MAP;

            GameManager.Instance.GetCurrentPlayer().controllerHandler.DisableController();

            mainCamera.gameObject.SetActive(false);
            mapCamera.gameObject.SetActive(true);

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

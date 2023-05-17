using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

using DIM.Environment;

namespace DIM.DungeonSystem {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(RoomGameObject))]
    #endregion
    public class RoomLightingController : MonoBehaviour {
        private RoomGameObject roomGameObject;

        // ===================================================================

        private void Awake() {
            roomGameObject = GetComponent<RoomGameObject>();
        }



        private void OnEnable() {
            DungeonStaticEvent.OnRoomChanged += DungeonStaticEvent_OnRoomChange;
        }



        private void OnDisable() {
            DungeonStaticEvent.OnRoomChanged -= DungeonStaticEvent_OnRoomChange;
        }



        private void DungeonStaticEvent_OnRoomChange(OnRoomChangedEventArgs _args) {
            if (_args.room == roomGameObject.room && !roomGameObject.room.isLit) {
                FadeInRoom();

                roomGameObject.ActivateEnvironment();
                FadeInEnvironment();

                FadeInAllDoorsOnRoom();

                roomGameObject.room.isLit = true;
            }
        }



        private void FadeInRoom() {
            StartCoroutine(FadeInRoomCoroutine(roomGameObject));
        }



        private IEnumerator FadeInRoomCoroutine(RoomGameObject _roomGameObject) {
            Material material = new Material(GameResources.Instance.VariableLitShader);

            ChangeRoomTilemapMaterials(_roomGameObject, material);

            for (float i = 0.05f; i <= 1f; i += Time.deltaTime / Settings.RoomFadeInTime) {
                material.SetFloat("Alpha_Slider", i);
                yield return null;
            }

            ChangeRoomTilemapMaterials(_roomGameObject, GameResources.Instance.LitMaterial);
        }



        private void ChangeTilemapMaterial(Tilemap _tilemap, Material _material) {
            _tilemap.GetComponent<TilemapRenderer>().material = _material;
        }



        private void ChangeRoomTilemapMaterials(RoomGameObject _roomGameObject, Material _material) {
            ChangeTilemapMaterial(_roomGameObject.groundTilemap, _material);
            ChangeTilemapMaterial(_roomGameObject.decorationTilemap1, _material);
            ChangeTilemapMaterial(_roomGameObject.decorationTilemap2, _material);
            ChangeTilemapMaterial(_roomGameObject.frontTilemap, _material);
            ChangeTilemapMaterial(_roomGameObject.minimapTilemap, _material);
        }



        private void FadeInEnvironment() {
            Material material = new Material(GameResources.Instance.VariableLitShader);

            EnvironmentGameObject[] environmentArray = GetComponentsInChildren<EnvironmentGameObject>();

            foreach (EnvironmentGameObject environment in environmentArray) {
                if (environment.spriteRenderer != null)
                    environment.spriteRenderer.material = material;
            }

            StartCoroutine(FadeInEnvironmentCoroutine(material, environmentArray));
        }



        private IEnumerator FadeInEnvironmentCoroutine(Material _material, EnvironmentGameObject[] _environmentArray) {
            for (float i = 0.05f; i <= 1f; i += Time.deltaTime / Settings.RoomFadeInTime) {
                _material.SetFloat("Alpha_Slider", i);
                yield return null;
            }

            foreach (EnvironmentGameObject environment in _environmentArray) {
                if (environment.spriteRenderer != null)
                    environment.spriteRenderer.material = GameResources.Instance.LitMaterial;
            }
        }



        private void FadeInAllDoorsOnRoom() {
            DoorGameObject[] doorGameObjectArray = GetComponentsInChildren<DoorGameObject>();

            foreach (DoorGameObject doorGameObject in doorGameObjectArray) {
                DoorLightingController doorLightingController = doorGameObject.GetComponentInChildren<DoorLightingController>();

                doorLightingController.FadeInDoor(doorGameObject);
            }
        }
    }
}

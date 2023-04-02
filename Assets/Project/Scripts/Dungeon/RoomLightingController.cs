using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(RoomGameObject))]
    #endregion
    public class RoomLightingController : MonoBehaviour {
        private RoomGameObject roomGameObject;



        private void Awake() {
            roomGameObject = GetComponent<RoomGameObject>();
        }



        private void OnEnable() {
            DungeonStaticEvent.OnRoomChange += DungeonStaticEvent_OnRoomChange;
        }



        private void OnDisable() {
            DungeonStaticEvent.OnRoomChange -= DungeonStaticEvent_OnRoomChange;
        }



        private void DungeonStaticEvent_OnRoomChange(OnRoomChangeEventArgs args) {
            if (args.room == roomGameObject.room && !roomGameObject.room.isLit) {
                FadeInRoom();

                FadeInAllDoorsOnRoom();

                roomGameObject.room.isLit = true;
            }
        }



        private void FadeInRoom() {
            StartCoroutine(FadeInRoomCoroutine(roomGameObject));
        }



        private IEnumerator FadeInRoomCoroutine(RoomGameObject roomGameObject) {
            Material material = new Material(GameResources.Instance.VariableLitShader);

            ChangeRoomTilemapMaterials(roomGameObject, material);

            for (float i = 0.05f; i <= 1f; i += Time.deltaTime / Settings.FadeInTime) {
                material.SetFloat("Alpha_Slider", i);
                yield return null;
            }

            ChangeRoomTilemapMaterials(roomGameObject, GameResources.Instance.LitMaterial);
        }



        private void ChangeTilemapMaterial(Tilemap tilemap, Material material) {
            tilemap.GetComponent<TilemapRenderer>().material = material;
        }



        private void ChangeRoomTilemapMaterials(RoomGameObject roomGameObject, Material material) {
            ChangeTilemapMaterial(roomGameObject.groundTilemap, material);
            ChangeTilemapMaterial(roomGameObject.decorationTilemap1, material);
            ChangeTilemapMaterial(roomGameObject.decorationTilemap2, material);
            ChangeTilemapMaterial(roomGameObject.frontTilemap, material);
            ChangeTilemapMaterial(roomGameObject.minimapTilemap, material);
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

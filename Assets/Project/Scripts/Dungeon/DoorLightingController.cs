using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class DoorLightingController : MonoBehaviour {
        private bool isLit = false;
        private DoorGameObject doorGameObject;



        private void Awake() {
            doorGameObject = GetComponentInParent<DoorGameObject>();
        }



        private void OnTriggerEnter2D(Collider2D other) {
            FadeInDoor(doorGameObject);
        }



        public void FadeInDoor(DoorGameObject doorGameObject) {
            Material material = new Material(GameResources.Instance.VariableLitShader);

            if (!isLit) {
                SpriteRenderer[] spriteRendererArray = GetComponentsInParent<SpriteRenderer>();

                foreach (SpriteRenderer spriteRenderer in spriteRendererArray) {
                    StartCoroutine(FadeInDoorCoroutine(spriteRenderer, material));
                }

                isLit = true;
            }
        }



        private IEnumerator FadeInDoorCoroutine(SpriteRenderer spriteRenderer, Material material) {
            spriteRenderer.material = material;

            for (float i = 0.05f; i <= 1f; i += Time.deltaTime / Settings.FadeInTime) {
                material.SetFloat("Alpha_Slider", i);
                yield return null;
            }

            spriteRenderer.material = GameResources.Instance.LitMaterial;
        }
    }
}

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



        private void OnTriggerEnter2D(Collider2D _other) {
            FadeInDoor(doorGameObject);
        }



        public void FadeInDoor(DoorGameObject _doorGameObject) {
            Material material = new Material(GameResources.Instance.VariableLitShader);

            if (!isLit) {
                SpriteRenderer[] spriteRendererArray = GetComponentsInParent<SpriteRenderer>();

                foreach (SpriteRenderer spriteRenderer in spriteRendererArray) {
                    StartCoroutine(FadeInDoorCoroutine(spriteRenderer, material));
                }

                isLit = true;
            }
        }



        private IEnumerator FadeInDoorCoroutine(SpriteRenderer _spriteRenderer, Material _material) {
            _spriteRenderer.material = _material;

            for (float i = 0.05f; i <= 1f; i += Time.deltaTime / Settings.FadeInTime) {
                _material.SetFloat("Alpha_Slider", i);
                yield return null;
            }

            _spriteRenderer.material = GameResources.Instance.LitMaterial;
        }
    }
}

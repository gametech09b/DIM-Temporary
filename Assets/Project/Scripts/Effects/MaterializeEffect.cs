using System.Collections;
using UnityEngine;

namespace DIM.Effect {
    public class MaterializeEffect : MonoBehaviour {
        public IEnumerator MaterializeCoroutine(Shader _shader, Color _color, float _duration, SpriteRenderer[] _spriteRendererArray, Material _normalMaterial) {
            Material materializeMaterial = new Material(_shader);

            materializeMaterial.SetColor("_EmissionColor", _color);

            foreach (SpriteRenderer spriteRenderer in _spriteRendererArray) {
                spriteRenderer.material = materializeMaterial;
            }

            float dissoveAmount = 0f;

            while (dissoveAmount < 1f) {
                dissoveAmount += Time.deltaTime / _duration;
                materializeMaterial.SetFloat("_DissolveAmount", dissoveAmount);
                yield return null;
            }

            foreach (SpriteRenderer spriteRenderer in _spriteRendererArray) {
                spriteRenderer.material = _normalMaterial;
            }
        }
    }
}

using System.Collections;
using TMPro;
using UnityEngine;

namespace DungeonGunner {
    #region Requirement Components
    [RequireComponent(typeof(MaterializeEffect))]
    #endregion
    public class ChestItem : MonoBehaviour {
        private SpriteRenderer spriteRenderer;
        private TextMeshPro textMP;
        private MaterializeEffect materializeEffect;

        [HideInInspector] public bool isMaterialized = false;



        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            textMP = GetComponentInChildren<TextMeshPro>();
            materializeEffect = GetComponent<MaterializeEffect>();
        }



        public void Init(Sprite _sprite, string _text, Vector3 _spawnPosition, Color _materializeColor) {
            spriteRenderer.sprite = _sprite;
            transform.position = _spawnPosition;

            StartCoroutine(MaterializeCoroutine(_materializeColor, _text));
        }



        private IEnumerator MaterializeCoroutine(Color _materializeColor, string _text) {
            SpriteRenderer[] spriteRendererArray = new SpriteRenderer[] { spriteRenderer };

            yield return StartCoroutine(materializeEffect.MaterializeCoroutine(GameResources.Instance.MaterializeShader, _materializeColor, 1f, spriteRendererArray, GameResources.Instance.LitMaterial));

            isMaterialized = true;

            textMP.text = _text;
        }
    }
}

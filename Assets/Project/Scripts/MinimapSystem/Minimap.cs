using Cinemachine;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class Minimap : MonoBehaviour {

        [SerializeField] private GameObject playerIcon;

        private Transform playerTransform;



        private void Start() {
            playerTransform = GameManager.Instance.GetCurrentPlayer().transform;

            CinemachineVirtualCamera minimapVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            minimapVirtualCamera.Follow = playerTransform;

            SpriteRenderer minimapPlayerIconSpriteRenderer = playerIcon.GetComponent<SpriteRenderer>();
            minimapPlayerIconSpriteRenderer.sprite = GameManager.Instance.GetCurrentPlayerMinimapIcon();
        }



        private void LateUpdate() {
            if (playerTransform == null) return;
            if (playerIcon == null) return;

            playerIcon.transform.position = playerTransform.position;
        }
    }
}

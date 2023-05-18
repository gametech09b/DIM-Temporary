using UnityEngine;

namespace DIM.HealthSystem {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(DestroyedEvent))]
    #endregion
    public class Destroyed : MonoBehaviour {
        private DestroyedEvent destroyedEvent;

        // ===================================================================

        private void Awake() {
            destroyedEvent = GetComponent<DestroyedEvent>();
        }



        private void OnEnable() {
            destroyedEvent.OnDestroyed += DestroyedEvent_OnDestroyed;
        }



        private void OnDisable() {
            destroyedEvent.OnDestroyed -= DestroyedEvent_OnDestroyed;
        }



        private void DestroyedEvent_OnDestroyed(DestroyedEvent _sender, OnDestroyedEventArgs _args) {
            if (_args.isPlayer)
                gameObject.SetActive(false);
            else
                Destroy(gameObject);
        }
    }
}

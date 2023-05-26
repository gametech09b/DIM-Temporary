using UnityEngine;

namespace DIM.EnemySystem {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(DeathEvent))]
    [RequireComponent(typeof(Rigidbody2D))]
    #endregion
    public class Death : MonoBehaviour {
        private Rigidbody2D rb2D;
        private DeathEvent deathEvent;

        // ===================================================================

        private void Awake() {
            rb2D = GetComponent<Rigidbody2D>();
            deathEvent = GetComponent<DeathEvent>();
        }



        private void OnEnable() {
            deathEvent.OnDeath += DeathEvent_OnDeath;
        }



        private void OnDisable() {
            deathEvent.OnDeath -= DeathEvent_OnDeath;
        }



        private void DeathEvent_OnDeath(DeathEvent sender) {
            ChangeToDeath();
            
        }



        private void ChangeToDeath() {
            rb2D.velocity = Vector2.zero;
        }
    }
}

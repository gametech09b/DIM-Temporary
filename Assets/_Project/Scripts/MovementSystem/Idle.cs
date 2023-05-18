using UnityEngine;

namespace DIM.MovementSystem {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(IdleEvent))]
    [RequireComponent(typeof(Rigidbody2D))]
    #endregion
    public class Idle : MonoBehaviour {
        private Rigidbody2D rb2D;
        private IdleEvent idleEvent;

        // ===================================================================

        private void Awake() {
            rb2D = GetComponent<Rigidbody2D>();
            idleEvent = GetComponent<IdleEvent>();
        }



        private void OnEnable() {
            idleEvent.OnIdle += IdleEvent_OnIdle;
        }



        private void OnDisable() {
            idleEvent.OnIdle -= IdleEvent_OnIdle;
        }



        private void IdleEvent_OnIdle(IdleEvent sender) {
            ChangeToIdle();
        }



        private void ChangeToIdle() {
            rb2D.velocity = Vector2.zero;
        }
    }
}

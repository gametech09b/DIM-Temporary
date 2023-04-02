using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(IdleEvent))]
    [RequireComponent(typeof(Rigidbody2D))]
    #endregion
    public class Idle : MonoBehaviour {
        private Rigidbody2D rigidbody2DComponent;
        private IdleEvent idleEvent;



        private void Awake() {
            rigidbody2DComponent = GetComponent<Rigidbody2D>();
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
            rigidbody2DComponent.velocity = Vector2.zero;
        }
    }
}

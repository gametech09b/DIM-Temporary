using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(MoveByVelocityEvent))]
    #endregion
    public class MoveByVelocity : MonoBehaviour {
        private Rigidbody2D rb2D;
        private MoveByVelocityEvent movementByVelocityEvent;



        private void Awake() {
            rb2D = GetComponent<Rigidbody2D>();
            movementByVelocityEvent = GetComponent<MoveByVelocityEvent>();
        }



        private void OnEnable() {
            movementByVelocityEvent.OnMoveByVelocity += MovementByVelocityEvent_OnMovementByVelocity;
        }



        private void OnDisable() {
            movementByVelocityEvent.OnMoveByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;
        }



        private void MovementByVelocityEvent_OnMovementByVelocity(MoveByVelocityEvent sender, MoveByVelocityEventArgs args) {
            Move(args.directionVector, args.speed);
        }



        private void Move(Vector2 _directionVector, float _speed) {
            rb2D.velocity = _directionVector * _speed;
        }
    }
}

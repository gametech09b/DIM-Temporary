using System;
using UnityEngine;

namespace DIM.MovementSystem {
    [DisallowMultipleComponent]
    public class MoveByVelocityEvent : MonoBehaviour {
        public event Action<MoveByVelocityEvent, MoveByVelocityEventArgs> OnMoveByVelocity;
        public void CallOnMoveByVelocity(Vector2 _directionVector, float _speed) {
            OnMoveByVelocity?.Invoke(
                this,
                new MoveByVelocityEventArgs() {
                    directionVector = _directionVector,
                    speed = _speed
                });
        }
    }


    public class MoveByVelocityEventArgs : EventArgs {
        public Vector2 directionVector;
        public float speed;
    }
}

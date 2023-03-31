using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class MoveByVelocityEvent : MonoBehaviour {
        public event Action<MoveByVelocityEvent, MoveByVelocityEventArgs> OnMoveByVelocity;



        public void CallOnMovementByVelocity(Vector2 directionVector, float speed) {
            OnMoveByVelocity?.Invoke(
                this,
                new MoveByVelocityEventArgs() {
                    directionVector = directionVector,
                    speed = speed
                });
        }
    }


    public class MoveByVelocityEventArgs : EventArgs {
        public Vector2 directionVector;
        public float speed;
    }
}

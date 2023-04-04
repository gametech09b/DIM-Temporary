using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class MoveToPositionEvent : MonoBehaviour {
        public event Action<MoveToPositionEvent, MoveToPositionEventArgs> OnMoveToPosition;

        public void CallOnMoveToPosition(Vector3 currentPosition, Vector3 targetPosition, Vector2 directionVector, float speed, bool isActive) {
            OnMoveToPosition?.Invoke(this,
                new MoveToPositionEventArgs {
                    currentPosition = currentPosition,
                    targetPosition = targetPosition,
                    directionVector = directionVector,
                    speed = speed,
                    isActive = isActive
                });
        }
    }



    public class MoveToPositionEventArgs {
        public Vector3 currentPosition;
        public Vector3 targetPosition;
        public Vector2 directionVector;
        public float speed;
        public bool isActive;
    }
}

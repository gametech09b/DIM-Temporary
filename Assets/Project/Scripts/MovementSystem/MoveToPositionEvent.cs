using System;
using UnityEngine;

namespace DIM.MovementSystem {
    [DisallowMultipleComponent]
    public class MoveToPositionEvent : MonoBehaviour {
        public event Action<MoveToPositionEvent, MoveToPositionEventArgs> OnMoveToPosition;

        public void CallOnMoveToPosition(Vector3 _currentPosition, Vector3 _targetPosition, Vector2 _directionVector, float _speed, bool _isActive = false) {
            OnMoveToPosition?.Invoke(this,
                new MoveToPositionEventArgs {
                    currentPosition = _currentPosition,
                    targetPosition = _targetPosition,
                    directionVector = _directionVector,
                    speed = _speed,
                    isActive = _isActive
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

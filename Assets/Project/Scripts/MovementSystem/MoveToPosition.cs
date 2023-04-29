using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(MoveToPositionEvent))]
    #endregion
    public class MoveToPosition : MonoBehaviour {
        private Rigidbody2D rigidbody2DComponent;
        private MoveToPositionEvent moveToPositionEvent;



        private void Awake() {
            rigidbody2DComponent = GetComponent<Rigidbody2D>();
            moveToPositionEvent = GetComponent<MoveToPositionEvent>();
        }



        private void OnEnable() {
            moveToPositionEvent.OnMoveToPosition += MoveToPositionEvent_OnMoveToPosition;
        }



        private void OnDisable() {
            moveToPositionEvent.OnMoveToPosition -= MoveToPositionEvent_OnMoveToPosition;
        }



        private void MoveToPositionEvent_OnMoveToPosition(MoveToPositionEvent _sender, MoveToPositionEventArgs _args) {
            MoveRigidbody2D(_args.currentPosition, _args.targetPosition, _args.speed);
        }



        private void MoveRigidbody2D(Vector3 _currentPosition, Vector3 _targetPosition, float _speed) {
            Vector2 directionVector = (_targetPosition - _currentPosition).normalized;

            rigidbody2DComponent.MovePosition(rigidbody2DComponent.position + (directionVector * _speed * Time.fixedDeltaTime));
        }
    }
}

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



        private void MoveToPositionEvent_OnMoveToPosition(MoveToPositionEvent sender, MoveToPositionEventArgs args) {
            MoveRigidbody2D(args.currentPosition, args.targetPosition, args.speed);
        }



        private void MoveRigidbody2D(Vector3 currentPosition, Vector3 targetPosition, float speed) {
            Vector2 directionVector = (targetPosition - currentPosition).normalized;

            rigidbody2DComponent.MovePosition(rigidbody2DComponent.position + (directionVector * speed * Time.fixedDeltaTime));
        }
    }
}

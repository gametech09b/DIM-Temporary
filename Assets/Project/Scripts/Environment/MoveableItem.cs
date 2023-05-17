using UnityEngine;

using DIM.AudioSystem;
using DIM.DungeonSystem;

namespace DIM.Environment {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    #endregion
    public class MoveableItem : MonoBehaviour {

        [SerializeField] private SoundEffectSO moveSoundEffect;

        [HideInInspector] public BoxCollider2D boxCollider2D;

        private Rigidbody2D rb2D;
        private RoomGameObject roomGameObject;
        private Vector3 previousPosition;

        // ===================================================================

        private void Awake() {
            boxCollider2D = GetComponent<BoxCollider2D>();
            rb2D = GetComponent<Rigidbody2D>();
            roomGameObject = GetComponentInParent<RoomGameObject>();

            roomGameObject.moveableItemList.Add(this);
        }



        private void OnCollisionStay2D(Collision2D other) {
            UpdateObstacle();
        }



        private void UpdateObstacle() {
            ConfineItemToRoomBounds();

            roomGameObject.UpdateMoveableObstacle();

            previousPosition = transform.position;


            if (Mathf.Abs(rb2D.velocity.x) > 0.001f || Mathf.Abs(rb2D.velocity.y) > 0.001f) {

                if (moveSoundEffect != null && Time.frameCount % 10 == 0)
                    SoundEffectManager.Instance.PlaySoundEffect(moveSoundEffect);
            }
        }



        private void ConfineItemToRoomBounds() {
            Bounds itemBounds = boxCollider2D.bounds;
            Bounds roomBounds = roomGameObject.roomColliderBounds;

            if (itemBounds.min.x <= roomBounds.min.x
            || itemBounds.max.x >= roomBounds.max.x
            || itemBounds.min.y <= roomBounds.min.y
            || itemBounds.max.y >= roomBounds.max.y)
                transform.position = previousPosition;
        }
    }
}

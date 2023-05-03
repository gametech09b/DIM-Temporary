using System;
using UnityEngine;

namespace DungeonGunner {
    #region Requirement Components
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    #endregion
    public class TableGameObject : MonoBehaviour, IUseable {
        [SerializeField] private float mass;

        private Animator animator;
        private BoxCollider2D boxCollider2D;
        private Rigidbody2D rb2D;

        private bool isUsed;



        private void Awake() {
            animator = GetComponent<Animator>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            rb2D = GetComponent<Rigidbody2D>();
        }



        public void Use() {
            if (!isUsed) {
                Bounds bounds = boxCollider2D.bounds;

                Vector3 closestPoint = bounds.ClosestPoint(GameManager.Instance.GetCurrentPlayer().GetPosition());

                if (closestPoint.x == bounds.max.x)
                    animator.SetBool(Settings.FlipLeft, true);

                else if (closestPoint.x == bounds.min.x)
                    animator.SetBool(Settings.FlipRight, true);

                else if (closestPoint.y == bounds.max.y)
                    animator.SetBool(Settings.FlipDown, true);

                else
                    animator.SetBool(Settings.FlipUp, true);

                gameObject.layer = LayerMask.NameToLayer("Environment");

                rb2D.mass = mass;

                SoundEffectManager.Instance.PlaySoundEffect(SoundEffectResources.Instance.TableFlipSoundEffect);

                isUsed = true;
            }
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckPositiveValue(this, nameof(mass), mass);
        }
#endif
        #endregion
    }
}

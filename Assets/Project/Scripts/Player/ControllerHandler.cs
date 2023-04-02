using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    public class ControllerHandler : MonoBehaviour {
        [Tooltip("The transform of the player's weapon shoot position")]
        [SerializeField] private Transform weaponShootPointTransform;

        [Tooltip("MovementDetailSO")]
        [SerializeField] private MovementDetailSO movementDetail;

        private Player player;
        private float moveSpeed;



        private Coroutine dashCoroutine;
        private WaitForFixedUpdate waitForFixedUpdate;
        private bool isDashing;
        private float dashCooldownTimer;



        private void Awake() {
            player = GetComponent<Player>();

            moveSpeed = movementDetail.GetMoveSpeed();
        }



        private void Start() {
            waitForFixedUpdate = new WaitForFixedUpdate();
        }



        private void Update() {
            if (isDashing) return;

            MovementInput();

            WeaponAimInput();

            ProcessDashCooldownTimer();
        }



        private void OnCollisionEnter2D(Collision2D other) {
            StopDashCoroutine();
        }



        private void OnCollisionStay2D(Collision2D other) {
            StopDashCoroutine();
        }



        private void MovementInput() {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            Vector2 directionVector = new Vector2(horizontalInput, verticalInput).normalized;

            if (directionVector != Vector2.zero) {
                if (Input.GetMouseButtonDown(1) && dashCooldownTimer <= 0) {
                    Roll((Vector3)directionVector);
                } else {
                    player.moveByVelocityEvent.CallOnMoveByVelocity(directionVector, moveSpeed);
                }
            } else {
                player.idleEvent.CallOnIdleEvent();
            }
        }



        private void WeaponAimInput() {
            Vector3 weaponDirectionVector;
            float weaponAngle, playerAngle;
            Direction playerDirection;

            HandleWeaponAimInput(out playerDirection, out playerAngle, out weaponAngle, out weaponDirectionVector);
        }



        private void HandleWeaponAimInput(out Direction playerDirection, out float playerAngle, out float weaponAngle, out Vector3 weaponDirectionVector) {
            Vector3 mousePosition = HelperUtilities.GetMouseWorldPosition();
            Vector3 playerPosition = transform.position;

            weaponDirectionVector = mousePosition - weaponShootPointTransform.position;

            Vector3 playerDirectionVector = mousePosition - playerPosition;

            weaponAngle = HelperUtilities.GetAngleFromVector(weaponDirectionVector);
            playerAngle = HelperUtilities.GetAngleFromVector(playerDirectionVector);

            playerDirection = HelperUtilities.GetDirectionFromAngle(playerAngle);

            player.aimEvent.CallOnAimEvent(playerDirection, playerAngle, weaponAngle, weaponDirectionVector);
        }



        private void Roll(Vector3 directionVector) {
            if (dashCoroutine != null) {
                StopCoroutine(dashCoroutine);
            }

            dashCoroutine = StartCoroutine(DashCoroutine(directionVector));
        }



        private IEnumerator DashCoroutine(Vector3 directionVector) {
            isDashing = true;

            float minimumDistance = 0.2f;

            Vector3 currentPosition = player.transform.position;
            Vector3 targetPosition = currentPosition + directionVector * movementDetail.dashDistance;

            while (Vector3.Distance(player.transform.position, targetPosition) > minimumDistance) {
                player.moveToPositionEvent.CallOnMoveToPosition(currentPosition, targetPosition, directionVector, movementDetail.dashSpeed, isDashing);

                yield return waitForFixedUpdate;
            }

            isDashing = false;

            dashCooldownTimer = movementDetail.dashCooldownTime;

            player.transform.position = targetPosition;
        }



        private void StopDashCoroutine() {
            if (dashCoroutine != null) {
                StopCoroutine(dashCoroutine);
            }

            isDashing = false;
        }



        private void ProcessDashCooldownTimer() {
            if (dashCooldownTimer >= 0) {
                dashCooldownTimer -= Time.deltaTime;
            }
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetail), movementDetail);
        }
#endif
        #endregion
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Player))]
    #endregion
    public class ControllerHandler : MonoBehaviour {

        [SerializeField] private MovementDetailSO movementDetail;

        private Player player;
        private int activeWeaponIndex = 1;
        private float moveSpeed;
        private bool isFiringPreviousFrame;



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

            SetupInitialWeapon();

            SetupPlayerAnimationSpeed();
        }



        private void Update() {
            if (isDashing) return;

            MovementInput();

            WeaponInput();

            ProcessDashCooldownTimer();
        }



        private void OnCollisionEnter2D(Collision2D other) {
            StopDashCoroutine();
        }



        private void OnCollisionStay2D(Collision2D other) {
            StopDashCoroutine();
        }



        private void SetupInitialWeapon() {
            int index = 1;

            foreach (Weapon weapon in player.weaponList) {
                if (weapon.weaponDetail == player.playerDetail.initialWeapon) {
                    SetWeaponByIndex(index);
                    break;
                }

                index++;
            }
        }



        private void SetWeaponByIndex(int index) {
            if (index - 1 < 0 || index - 1 > player.weaponList.Count) return;

            activeWeaponIndex = index;
            player.activeWeaponEvent.CallOnSetActiveWeapon(player.weaponList[index - 1]);
        }



        private void SetupPlayerAnimationSpeed() {
            player.animator.speed = moveSpeed / Settings.BaseSpeedForPlayer;
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



        private void WeaponInput() {
            Vector3 weaponDirectionVector;
            float weaponAngle, playerAngle;
            Direction playerDirection;

            HandleAimInput(out playerDirection, out playerAngle, out weaponAngle, out weaponDirectionVector);

            HandleFireInput(playerDirection, playerAngle, weaponAngle, weaponDirectionVector);

            HandleSwitchWeaponInput();

            HandleReloadInput();
        }



        private void HandleAimInput(out Direction playerDirection, out float playerAngle, out float weaponAngle, out Vector3 weaponDirectionVector) {
            Vector3 mousePosition = HelperUtilities.GetMouseWorldPosition();
            Vector3 playerPosition = transform.position;

            weaponDirectionVector = mousePosition - player.activeWeapon.GetShootPosition();

            Vector3 playerDirectionVector = mousePosition - playerPosition;

            weaponAngle = HelperUtilities.GetAngleFromVector(weaponDirectionVector);
            playerAngle = HelperUtilities.GetAngleFromVector(playerDirectionVector);

            playerDirection = HelperUtilities.GetDirectionFromAngle(playerAngle);

            player.aimEvent.CallOnAimAction(playerDirection, playerAngle, weaponAngle, weaponDirectionVector);
        }



        private void HandleFireInput(Direction playerDirection, float playerAngle, float weaponAngle, Vector3 weaponDirectionVector) {
            bool isFiring = Input.GetMouseButton(0);

            if (isFiring) {
                player.fireEvent.CallOnFireAction(isFiring, isFiringPreviousFrame, playerDirection, playerAngle, weaponAngle, weaponDirectionVector);
                isFiringPreviousFrame = true;
            } else {
                isFiringPreviousFrame = false;
            }
        }



        private void HandleSwitchWeaponInput() {
            if (Input.mouseScrollDelta.y < 0f) {
                PreviousWeapon();
            }
            if (Input.mouseScrollDelta.y > 0f) {
                NextWeapon();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                SetWeaponByIndex(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                SetWeaponByIndex(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                SetWeaponByIndex(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                SetWeaponByIndex(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                SetWeaponByIndex(5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6)) {
                SetWeaponByIndex(6);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7)) {
                SetWeaponByIndex(7);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8)) {
                SetWeaponByIndex(8);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9)) {
                SetWeaponByIndex(9);
            }
        }



        private void HandleReloadInput() {
            Weapon currentWeapon = player.activeWeapon.GetCurrentWeapon();

            if (currentWeapon.isReloading) return;

            if (currentWeapon.ammoPerClipRemaining == currentWeapon.weaponDetail.ammoPerClipCapacity)
                return;

            if (!currentWeapon.weaponDetail.isAmmoInfinite && currentWeapon.ammoRemaining < currentWeapon.weaponDetail.ammoPerClipCapacity)
                return;

            if (Input.GetKeyDown(KeyCode.R)) {
                player.reloadEvent.CallOnReloadAction(currentWeapon, 0);
            }
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



        private void PreviousWeapon() {
            activeWeaponIndex--;

            if (activeWeaponIndex < 1) {
                activeWeaponIndex = player.weaponList.Count;
            }

            SetWeaponByIndex(activeWeaponIndex);
        }



        private void NextWeapon() {
            activeWeaponIndex++;

            if (activeWeaponIndex > player.weaponList.Count) {
                activeWeaponIndex = 1;
            }

            SetWeaponByIndex(activeWeaponIndex);
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

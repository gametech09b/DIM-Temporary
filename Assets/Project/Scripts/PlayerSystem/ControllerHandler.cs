using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Player))]
    #endregion
    public class ControllerHandler : MonoBehaviour
    {

        [SerializeField] private MovementDetailSO movementDetail;

        private Player _player;
        private int _activeWeaponIndex = 1;
        private float _moveSpeed;
        private bool _isFiringPreviousFrame;

        private Coroutine _dashCoroutine;
        private WaitForFixedUpdate _waitForFixedUpdate;
        private bool _isDashing;
        private float _dashCooldownTimer;



        private void Awake()
        {
            _player = GetComponent<Player>();

            _moveSpeed = movementDetail.GetMoveSpeed();
        }



        private void Start()
        {
            _waitForFixedUpdate = new WaitForFixedUpdate();

            SetupInitialWeapon();

            SetupPlayerAnimationSpeed();
        }



        private void Update()
        {
            if (_isDashing) return;

            MovementInput();

            WeaponInput();

            ProcessDashCooldownTimer();
        }



        private void OnCollisionEnter2D(Collision2D other)
        {
            StopDashCoroutine();
        }



        private void OnCollisionStay2D(Collision2D other)
        {
            StopDashCoroutine();
        }



        private void SetupInitialWeapon()
        {
            int index = 1;

            foreach (Weapon weapon in _player.weaponList)
            {
                if (weapon.weaponDetail == _player.playerDetail.initialWeapon)
                {
                    SetWeaponByIndex(index);
                    break;
                }

                index++;
            }
        }



        private void SetWeaponByIndex(int index)
        {
            if (index - 1 < 0 || index - 1 > _player.weaponList.Count) return;

            _activeWeaponIndex = index;
            _player.activeWeaponEvent.CallOnSetActiveWeapon(_player.weaponList[index - 1]);
        }



        private void SetupPlayerAnimationSpeed()
        {
            _player.animator.speed = _moveSpeed / Settings.BaseSpeedForPlayerAnimation;
        }



        private void MovementInput()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            Vector2 directionVector = new Vector2(horizontalInput, verticalInput).normalized;

            if (directionVector != Vector2.zero)
            {
                if (Input.GetMouseButtonDown(1) && _dashCooldownTimer <= 0)
                {
                    Roll((Vector3)directionVector);
                }
                else
                {
                    _player.moveByVelocityEvent.CallOnMoveByVelocity(directionVector, _moveSpeed);
                }
            }
            else
            {
                _player.idleEvent.CallOnIdleEvent();
            }
        }



        private void WeaponInput()
        {
            Vector3 weaponDirectionVector;
            float weaponAngle, playerAngle;
            Direction playerDirection;

            HandleAimInput(out playerDirection, out playerAngle, out weaponAngle, out weaponDirectionVector);

            HandleFireInput(playerDirection, playerAngle, weaponAngle, weaponDirectionVector);

            HandleSwitchWeaponInput();

            HandleReloadInput();
        }



        private void HandleAimInput(out Direction playerDirection, out float playerAngle, out float weaponAngle, out Vector3 weaponDirectionVector)
        {
            Vector3 mousePosition = HelperUtilities.GetMouseWorldPosition();
            Vector3 playerPosition = transform.position;

            weaponDirectionVector = mousePosition - _player.activeWeapon.GetShootPosition();

            Vector3 playerDirectionVector = mousePosition - playerPosition;

            weaponAngle = HelperUtilities.GetAngleFromVector(weaponDirectionVector);
            playerAngle = HelperUtilities.GetAngleFromVector(playerDirectionVector);

            playerDirection = HelperUtilities.GetDirectionFromAngle(playerAngle);

            _player.aimEvent.CallOnAimAction(playerDirection, playerAngle, weaponAngle, weaponDirectionVector);
        }



        private void HandleFireInput(Direction playerDirection, float playerAngle, float weaponAngle, Vector3 weaponDirectionVector)
        {
            bool isFiring = Input.GetMouseButton(0);

            if (isFiring)
            {
                _player.fireEvent.CallOnFireAction(isFiring, _isFiringPreviousFrame, playerDirection, playerAngle, weaponAngle, weaponDirectionVector);
                _isFiringPreviousFrame = true;
            }
            else
            {
                _isFiringPreviousFrame = false;
            }
        }



        private void HandleSwitchWeaponInput()
        {
            if (Input.mouseScrollDelta.y < 0f)
            {
                PreviousWeapon();
            }
            if (Input.mouseScrollDelta.y > 0f)
            {
                NextWeapon();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetWeaponByIndex(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetWeaponByIndex(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetWeaponByIndex(3);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetWeaponByIndex(4);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SetWeaponByIndex(5);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                SetWeaponByIndex(6);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                SetWeaponByIndex(7);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                SetWeaponByIndex(8);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                SetWeaponByIndex(9);
            }
        }



        private void HandleReloadInput()
        {
            Weapon currentWeapon = _player.activeWeapon.GetCurrentWeapon();

            if (currentWeapon.isReloading) return;

            if (currentWeapon.ammoPerClipRemaining == currentWeapon.weaponDetail.ammoPerClipCapacity)
                return;

            if (!currentWeapon.weaponDetail.isAmmoInfinite && currentWeapon.ammoRemaining < currentWeapon.weaponDetail.ammoPerClipCapacity)
                return;

            if (Input.GetKeyDown(KeyCode.R))
            {
                _player.reloadEvent.CallOnReloadAction(currentWeapon, 0);
            }
        }



        private void Roll(Vector3 directionVector)
        {
            if (_dashCoroutine != null)
            {
                StopCoroutine(_dashCoroutine);
            }

            _dashCoroutine = StartCoroutine(DashCoroutine(directionVector));
        }



        private IEnumerator DashCoroutine(Vector3 directionVector)
        {
            _isDashing = true;

            float minimumDistance = 0.2f;

            Vector3 currentPosition = _player.transform.position;
            Vector3 targetPosition = currentPosition + directionVector * movementDetail.dashDistance;

            while (Vector3.Distance(_player.transform.position, targetPosition) > minimumDistance)
            {
                _player.moveToPositionEvent.CallOnMoveToPosition(currentPosition, targetPosition, directionVector, movementDetail.dashSpeed, _isDashing);

                yield return _waitForFixedUpdate;
            }

            _isDashing = false;

            _dashCooldownTimer = movementDetail.dashCooldownTime;

            _player.transform.position = targetPosition;
        }



        private void StopDashCoroutine()
        {
            if (_dashCoroutine != null)
            {
                StopCoroutine(_dashCoroutine);
            }

            _isDashing = false;
        }



        private void ProcessDashCooldownTimer()
        {
            if (_dashCooldownTimer >= 0)
            {
                _dashCooldownTimer -= Time.deltaTime;
            }
        }



        private void PreviousWeapon()
        {
            _activeWeaponIndex--;

            if (_activeWeaponIndex < 1)
            {
                _activeWeaponIndex = _player.weaponList.Count;
            }

            SetWeaponByIndex(_activeWeaponIndex);
        }



        private void NextWeapon()
        {
            _activeWeaponIndex++;

            if (_activeWeaponIndex > _player.weaponList.Count)
            {
                _activeWeaponIndex = 1;
            }

            SetWeaponByIndex(_activeWeaponIndex);
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckNullValue(this, nameof(movementDetail), movementDetail);
        }
#endif
        #endregion
    }
}

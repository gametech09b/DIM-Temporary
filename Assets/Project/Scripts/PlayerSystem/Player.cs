using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SortingGroup))]
    [RequireComponent(typeof(SpriteRenderer))]

    [RequireComponent(typeof(ActiveWeapon))]
    [RequireComponent(typeof(ActiveWeaponEvent))]
    [RequireComponent(typeof(AimAction))]
    [RequireComponent(typeof(AimEvent))]
    [RequireComponent(typeof(AnimatorHandler))]
    [RequireComponent(typeof(ControllerHandler))]
    [RequireComponent(typeof(DealContactDamage))]
    [RequireComponent(typeof(Destroyed))]
    [RequireComponent(typeof(DestroyedEvent))]
    [RequireComponent(typeof(FireAction))]
    [RequireComponent(typeof(FireEvent))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(HealthEvent))]
    [RequireComponent(typeof(Idle))]
    [RequireComponent(typeof(IdleEvent))]
    [RequireComponent(typeof(MoveByVelocity))]
    [RequireComponent(typeof(MoveByVelocityEvent))]
    [RequireComponent(typeof(MoveToPosition))]
    [RequireComponent(typeof(MoveToPositionEvent))]
    [RequireComponent(typeof(ReloadAction))]
    [RequireComponent(typeof(ReloadEvent))]
    [RequireComponent(typeof(TakeContactDamage))]
    #endregion
    public class Player : MonoBehaviour {
        [HideInInspector] public Animator animator;
        [HideInInspector] public SpriteRenderer spriteRenderer;

        [HideInInspector] public PlayerDetailSO playerDetail;
        [HideInInspector] public ControllerHandler controllerHandler;

        public List<Weapon> weaponList = new List<Weapon>();
        [HideInInspector] public ActiveWeapon activeWeapon;

        [HideInInspector] public AimEvent aimEvent;
        [HideInInspector] public IdleEvent idleEvent;
        [HideInInspector] public MoveByVelocityEvent moveByVelocityEvent;
        [HideInInspector] public MoveToPositionEvent moveToPositionEvent;
        [HideInInspector] public ActiveWeaponEvent activeWeaponEvent;
        [HideInInspector] public FireEvent fireEvent;
        [HideInInspector] public ReloadEvent reloadEvent;

        [HideInInspector] public Health health;
        [HideInInspector] public HealthEvent healthEvent;
        [HideInInspector] public DestroyedEvent destroyedEvent;




        private void Awake() {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            controllerHandler = GetComponent<ControllerHandler>();

            activeWeapon = GetComponent<ActiveWeapon>();

            activeWeaponEvent = GetComponent<ActiveWeaponEvent>();
            aimEvent = GetComponent<AimEvent>();
            fireEvent = GetComponent<FireEvent>();
            idleEvent = GetComponent<IdleEvent>();
            moveByVelocityEvent = GetComponent<MoveByVelocityEvent>();
            moveToPositionEvent = GetComponent<MoveToPositionEvent>();
            reloadEvent = GetComponent<ReloadEvent>();

            health = GetComponent<Health>();
            healthEvent = GetComponent<HealthEvent>();
            destroyedEvent = GetComponent<DestroyedEvent>();
        }



        private void OnEnable() {
            healthEvent.OnHealthChanged += HealthEvent_OnHealthChange;
        }



        private void OnDisable() {
            healthEvent.OnHealthChanged -= HealthEvent_OnHealthChange;
        }



        private void HealthEvent_OnHealthChange(HealthEvent _sender, OnHealthChangedEventArgs _args) {
            if (_args.healthAmount <= 0f) {
                destroyedEvent.CallOnDestroyed(true, 0);
            }
        }



        public void Init(PlayerDetailSO _playerDetail) {
            this.playerDetail = _playerDetail;

            SetupHealth();
            SetupInitialWeapon();
        }



        private void SetupHealth() {
            health.SetStartingAmount(playerDetail.startingHealthAmount);
        }



        private void SetupInitialWeapon() {
            weaponList.Clear();

            foreach (WeaponDetailSO weaponDetail in playerDetail.initialWeaponsList) {
                AddWeapon(weaponDetail);
            }
        }



        public Weapon AddWeapon(WeaponDetailSO _weaponDetail) {
            Weapon weapon = new Weapon(_weaponDetail);
            weaponList.Add(weapon);
            weapon.indexOnList = weaponList.Count;

            activeWeaponEvent.CallOnSetActiveWeapon(weapon);
            return weapon;
        }



        public bool IsWeaponInList(WeaponDetailSO _weaponDetail) {
            foreach (Weapon weapon in weaponList) {
                if (weapon.weaponDetail == _weaponDetail)
                return true;
            }

            return false;
        }



        public Vector3 GetPosition() {
            return transform.position;
        }
    }
}

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

    [RequireComponent(typeof(Aim))]
    [RequireComponent(typeof(AimEvent))]
    [RequireComponent(typeof(AnimatorHandler))]
    [RequireComponent(typeof(ControllerHandler))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Idle))]
    [RequireComponent(typeof(IdleEvent))]
    [RequireComponent(typeof(MoveByVelocity))]
    [RequireComponent(typeof(MoveByVelocityEvent))]
    [RequireComponent(typeof(MoveToPosition))]
    [RequireComponent(typeof(MoveToPositionEvent))]
    [RequireComponent(typeof(ActiveWeapon))]
    [RequireComponent(typeof(ActiveWeaponEvent))]
    #endregion
    public class Player : MonoBehaviour {
        [HideInInspector] public Animator animator;
        [HideInInspector] public Health health;
        [HideInInspector] public SpriteRenderer spriteRenderer;

        [HideInInspector] public PlayerDetailSO playerDetail;

        public List<Weapon> weaponList = new List<Weapon>();
        [HideInInspector] public ActiveWeapon activeWeapon;

        [HideInInspector] public AimEvent aimEvent;
        [HideInInspector] public IdleEvent idleEvent;
        [HideInInspector] public MoveByVelocityEvent moveByVelocityEvent;
        [HideInInspector] public MoveToPositionEvent moveToPositionEvent;
        [HideInInspector] public ActiveWeaponEvent setActiveWeaponEvent;




        private void Awake() {
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            activeWeapon = GetComponent<ActiveWeapon>();

            aimEvent = GetComponent<AimEvent>();
            idleEvent = GetComponent<IdleEvent>();
            moveByVelocityEvent = GetComponent<MoveByVelocityEvent>();
            moveToPositionEvent = GetComponent<MoveToPositionEvent>();
            setActiveWeaponEvent = GetComponent<ActiveWeaponEvent>();
        }



        public void Init(PlayerDetailSO playerDetail) {
            this.playerDetail = playerDetail;

            SetupPlayerHealth();
            SetupPlayerInitialWeapon();
        }



        private void SetupPlayerHealth() {
            health.SetStartingAmount(playerDetail.startingHealthAmount);
        }



        private void SetupPlayerInitialWeapon() {
            weaponList.Clear();

            foreach (WeaponDetailSO weaponDetail in playerDetail.initialWeaponsList) {
                AddWeapon(weaponDetail);
            }
        }



        public Weapon AddWeapon(WeaponDetailSO weaponDetail) {
            Weapon weapon = new Weapon(weaponDetail);
            weaponList.Add(weapon);
            weapon.indexOnList = weaponList.Count;

            setActiveWeaponEvent.CallOnSetActiveWeapon(weapon);
            return weapon;
        }
    }
}

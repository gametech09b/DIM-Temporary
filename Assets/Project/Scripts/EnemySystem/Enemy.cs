using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using DungeonGunner.EnemySystem;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SortingGroup))]
    [RequireComponent(typeof(SpriteRenderer))]

    [RequireComponent(typeof(ActiveWeapon))]
    [RequireComponent(typeof(ActiveWeaponEvent))]
    [RequireComponent(typeof(AimAction))]
    [RequireComponent(typeof(AimEvent))]
    [RequireComponent(typeof(Destroyed))]
    [RequireComponent(typeof(DestroyedEvent))]
    [RequireComponent(typeof(EnemyAnimatorHandler))]
    [RequireComponent(typeof(EnemyMovementAI))]
    [RequireComponent(typeof(EnemyWeaponAI))]
    [RequireComponent(typeof(FireAction))]
    [RequireComponent(typeof(FireEvent))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(HealthEvent))]
    [RequireComponent(typeof(Idle))]
    [RequireComponent(typeof(IdleEvent))]
    [RequireComponent(typeof(MaterializeEffect))]
    [RequireComponent(typeof(MoveToPosition))]
    [RequireComponent(typeof(MoveToPositionEvent))]
    [RequireComponent(typeof(ReloadAction))]
    [RequireComponent(typeof(ReloadEvent))]
    #endregion
    public class Enemy : MonoBehaviour
    {
        [HideInInspector] public Animator animator;
        private CircleCollider2D circleCollider2D;
        private PolygonCollider2D polygonCollider2D;
        private SpriteRenderer[] spriteRendererArray;

        [HideInInspector] public EnemyDetailSO enemyDetail;
        private EnemyMovementAI enemyMovementAI;
        private FireAction fireAction;

        [HideInInspector] public ActiveWeaponEvent activeWeaponEvent;
        [HideInInspector] public AimEvent aimEvent;
        [HideInInspector] public FireEvent fireEvent;
        [HideInInspector] public IdleEvent idleEvent;
        [HideInInspector] public MoveToPositionEvent moveToPositionEvent;

        private Health health;
        private HealthEvent healthEvent;

        private MaterializeEffect materializeEffect;



        private void Awake()
        {
            animator = GetComponent<Animator>();
            circleCollider2D = GetComponent<CircleCollider2D>();
            polygonCollider2D = GetComponent<PolygonCollider2D>();
            spriteRendererArray = GetComponentsInChildren<SpriteRenderer>();

            enemyMovementAI = GetComponent<EnemyMovementAI>();
            fireAction = GetComponent<FireAction>();

            activeWeaponEvent = GetComponent<ActiveWeaponEvent>();
            aimEvent = GetComponent<AimEvent>();
            fireEvent = GetComponent<FireEvent>();
            idleEvent = GetComponent<IdleEvent>();
            moveToPositionEvent = GetComponent<MoveToPositionEvent>();

            health = GetComponent<Health>();
            healthEvent = GetComponent<HealthEvent>();

            materializeEffect = GetComponent<MaterializeEffect>();
        }



        private void OnEnable()
        {
            healthEvent.OnHealthChange += HealthEvent_OnHealthChange;
        }



        private void OnDisable()
        {
            healthEvent.OnHealthChange -= HealthEvent_OnHealthChange;
        }



        private void HealthEvent_OnHealthChange(HealthEvent _sender, OnHealthChangeEventArgs _args)
        {
            if (_args.healthAmount <= 0)
            {
                DestroyGameObject();
            }
        }



        public void Init(EnemyDetailSO _enemyDetail, int _spawnedCount, DungeonLevelSO _dungeonLevel)
        {
            this.enemyDetail = _enemyDetail;

            SetupAnimationSpeed();

            SetMovePathfindingUpdateFrame(_spawnedCount);

            SetStartingHealth(_dungeonLevel);

            SetStartingWeapon();

            StartCoroutine(MaterializeCoroutine());
        }



        private void SetupAnimationSpeed()
        {
            animator.speed = enemyMovementAI.moveSpeed / Settings.BaseSpeedForEnemyAnimation;
        }



        private void SetMovePathfindingUpdateFrame(int _spawnedCount)
        {
            enemyMovementAI.SetUpdateAtFrame(_spawnedCount % Settings.AStarTargetFrameRate);
        }



        private void SetStartingHealth(DungeonLevelSO _dungeonLevel)
        {
            foreach (EnemyHealthDetail enemyHealthDetail in enemyDetail.enemyHealthDetailArray)
            {
                if (enemyHealthDetail.dungeonLevel == _dungeonLevel)
                {
                    health.SetStartingAmount(enemyHealthDetail.healthAmount);
                    break;
                }
            }

            health.SetStartingAmount(Settings.EnemyDefaultHealth);
        }



        private void SetStartingWeapon()
        {
            if (enemyDetail.weaponDetail != null)
            {
                Weapon weapon = new Weapon(enemyDetail.weaponDetail);

                activeWeaponEvent.CallOnSetActiveWeapon(weapon);
            }
        }



        private IEnumerator MaterializeCoroutine()
        {
            SetEnable(false);

            yield return StartCoroutine(materializeEffect.MaterializeCoroutine(enemyDetail.materializeShader, enemyDetail.materializeColor, enemyDetail.materializeDuration, spriteRendererArray, enemyDetail.standardMaterial));

            SetEnable(true);
        }



        private void SetEnable(bool _isEnable)
        {
            circleCollider2D.enabled = _isEnable;
            polygonCollider2D.enabled = _isEnable;

            enemyMovementAI.enabled = _isEnable;
            fireAction.enabled = _isEnable;
        }



        private void DestroyGameObject()
        {
            DestroyedEvent destroyedEvent = GetComponent<DestroyedEvent>();
            destroyedEvent.CallOnDestroyed();
        }
    }
}

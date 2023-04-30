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

    [RequireComponent(typeof(EnemyAnimatorHandler))]
    [RequireComponent(typeof(EnemyMovementAI))]
    [RequireComponent(typeof(Idle))]
    [RequireComponent(typeof(IdleEvent))]
    [RequireComponent(typeof(MoveToPosition))]
    [RequireComponent(typeof(MoveToPositionEvent))]
    #endregion
    public class Enemy : MonoBehaviour
    {
        [HideInInspector] public Animator animator;
        private CircleCollider2D circleCollider2D;
        private PolygonCollider2D polygonCollider2D;
        private SpriteRenderer[] spriteRendererArray;

        [HideInInspector] public EnemyDetailSO enemyDetail;
        private EnemyMovementAI enemyMovementAI;

        [HideInInspector] public IdleEvent idleEvent;
        [HideInInspector] public MoveToPositionEvent moveToPositionEvent;



        private void Awake()
        {
            animator = GetComponent<Animator>();
            circleCollider2D = GetComponent<CircleCollider2D>();
            polygonCollider2D = GetComponent<PolygonCollider2D>();
            spriteRendererArray = GetComponentsInChildren<SpriteRenderer>();

            enemyMovementAI = GetComponent<EnemyMovementAI>();

            idleEvent = GetComponent<IdleEvent>();
            moveToPositionEvent = GetComponent<MoveToPositionEvent>();
        }



        public void Init(EnemyDetailSO _enemyDetail, int _spawnedCount, DungeonLevelSO _dungeonLevel)
        {
            this.enemyDetail = _enemyDetail;

            SetupEnemyAnimationSpeed();

            SetEnemyMoveUpdateFrame(_spawnedCount);
        }



        private void SetupEnemyAnimationSpeed()
        {
            animator.speed = enemyMovementAI.moveSpeed / Settings.BaseSpeedForEnemyAnimation;
        }



        private void SetEnemyMoveUpdateFrame(int _spawnedCount)
        {
            enemyMovementAI.SetUpdateAtFrame(_spawnedCount % Settings.AStarTargetFrameRate);
        }
    }
}

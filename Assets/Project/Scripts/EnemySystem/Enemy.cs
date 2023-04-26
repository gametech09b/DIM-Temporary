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
        private CircleCollider2D _circleCollider2D;
        private PolygonCollider2D _polygonCollider2D;
        private SpriteRenderer[] _spriteRendererArray;

        /* [HideInInspector] */
        public EnemyDetailSO detail;
        private EnemyMovementAI _enemyMovementAI;

        [HideInInspector] public IdleEvent idleEvent;
        [HideInInspector] public MoveToPositionEvent moveToPositionEvent;



        private void Awake()
        {
            animator = GetComponent<Animator>();
            _circleCollider2D = GetComponent<CircleCollider2D>();
            _polygonCollider2D = GetComponent<PolygonCollider2D>();
            _spriteRendererArray = GetComponentsInChildren<SpriteRenderer>();

            _enemyMovementAI = GetComponent<EnemyMovementAI>();

            idleEvent = GetComponent<IdleEvent>();
            moveToPositionEvent = GetComponent<MoveToPositionEvent>();
        }
    }
}

using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(SortingGroup))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    #endregion
    public class Player : MonoBehaviour
    {
        [HideInInspector] public PlayerDetailSO playerDetail;
        [HideInInspector] public Health health;
        [HideInInspector] public SpriteRenderer spriteRenderer;
        [HideInInspector] public Animator animator;



        private void Awake()
        {
            health = GetComponent<Health>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }



        public void Init(PlayerDetailSO playerDetail)
        {
            this.playerDetail = playerDetail;

            SetPlayerHealth();
        }



        private void SetPlayerHealth()
        {
            health.SetStartingAmount(playerDetail.startingHealthAmount);
        }
    }
}

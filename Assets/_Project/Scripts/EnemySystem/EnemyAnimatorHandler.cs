using UnityEngine;
using DIM.CombatSystem;
using DIM.MovementSystem;
using System;
using System.Collections;

namespace DIM.EnemySystem {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Enemy))]
    #endregion
    public class EnemyAnimatorHandler : MonoBehaviour {
        private Enemy enemy;
        [HideInInspector] public SpriteRenderer[] spriteRendererArray;

        // ===================================================================

        private void Awake() {
            enemy = GetComponent<Enemy>();
            spriteRendererArray = GetComponentsInChildren<SpriteRenderer>();
        }



        private void OnEnable() {
            enemy.idleEvent.OnIdle += IdleEvent_OnIdle;
            enemy.moveToPositionEvent.OnMoveToPosition += MoveToPositionEvent_OnMoveToPosition;
            enemy.aimEvent.OnAimAction += AimEvent_OnAimAction;
            enemy.deathEvent.OnDeath += DeathEvent_OnDeath;
        }
       
        private void OnDisable() {
            enemy.idleEvent.OnIdle -= IdleEvent_OnIdle;
            enemy.moveToPositionEvent.OnMoveToPosition -= MoveToPositionEvent_OnMoveToPosition;
            enemy.aimEvent.OnAimAction -= AimEvent_OnAimAction;
            enemy.deathEvent.OnDeath -= DeathEvent_OnDeath;
        }



        private void IdleEvent_OnIdle(IdleEvent _sender) {
            SetIdleAnimationParameters();
        }

        private void DeathEvent_OnDeath(DeathEvent _sender) {
            enemy.isDeath = true;
            SetDeathAnimationParameters();
        }



        private void MoveToPositionEvent_OnMoveToPosition(MoveToPositionEvent _sender, MoveToPositionEventArgs _args) {
            SetMoveAnimationParameters();
        }



        private void AimEvent_OnAimAction(AimEvent _sender, AimEventArgs _args) {
            DisableAllAimAnimationParameters();

            SetAimAnimationParameters(_args.direction);
        }



        private void SetIdleAnimationParameters() {
            if(!enemy.isDeath)
            enemy.animator.SetBool(Settings.IsIdle, true);

            enemy.animator.SetBool(Settings.IsMoving, false);
        }
        public void SetDeathAnimationParameters(){
            enemy.animator.SetBool(Settings.IsIdle, false);
            enemy.animator.SetBool(Settings.IsMoving, false);
            enemy.animator.SetBool(Settings.IsDeath, true);
            Debug.Log("TOL");
            StartCoroutine(coroutineA());
            

        }

        IEnumerator coroutineA()
    {
        yield return new WaitForSeconds(enemy.animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }



        private void DisableAllAimAnimationParameters() {
            // enemy.animator.SetBool(Settings.AimUp, false);
            // enemy.animator.SetBool(Settings.AimUpRight, false);
            // enemy.animator.SetBool(Settings.AimUpLeft, false);
            enemy.animator.SetBool(Settings.AimRight, false);
            // enemy.animator.SetBool(Settings.AimDown, false);
            enemy.animator.SetBool(Settings.AimLeft, false);
        }



        private void SetAimAnimationParameters(Direction _direction) {
            switch (_direction) {
                case Direction.UP:
                    enemy.animator.SetBool(Settings.AimRight, true);
                    break;
                case Direction.UP_RIGHT:
                    enemy.animator.SetBool(Settings.AimRight, true);
                    break;
                case Direction.UP_LEFT:
                    enemy.animator.SetBool(Settings.AimLeft, true);
                    break;
                case Direction.RIGHT:
                    enemy.animator.SetBool(Settings.AimRight, true);
                    break;
                case Direction.DOWN:
                    enemy.animator.SetBool(Settings.AimLeft, true);
                    break;
                case Direction.LEFT:
                    enemy.animator.SetBool(Settings.AimLeft, true);
                    break;
            }
        }



        private void SetMoveAnimationParameters() {
            enemy.animator.SetBool(Settings.IsIdle, false);

            if (!enemy.isDeath)
                enemy.animator.SetBool(Settings.IsMoving, true);
        }
    }
}

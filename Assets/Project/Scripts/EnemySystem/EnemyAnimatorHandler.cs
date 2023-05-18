using UnityEngine;

using DIM.CombatSystem;
using DIM.MovementSystem;

namespace DIM.EnemySystem {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Enemy))]
    #endregion
    public class EnemyAnimatorHandler : MonoBehaviour {
        private Enemy enemy;

        // ===================================================================

        private void Awake() {
            enemy = GetComponent<Enemy>();
        }



        private void OnEnable() {
            enemy.idleEvent.OnIdle += IdleEvent_OnIdle;
            enemy.moveToPositionEvent.OnMoveToPosition += MoveToPositionEvent_OnMoveToPosition;
            enemy.aimEvent.OnAimAction += AimEvent_OnAimAction;
        }



        private void OnDisable() {
            enemy.idleEvent.OnIdle -= IdleEvent_OnIdle;
            enemy.moveToPositionEvent.OnMoveToPosition -= MoveToPositionEvent_OnMoveToPosition;
            enemy.aimEvent.OnAimAction -= AimEvent_OnAimAction;
        }



        private void IdleEvent_OnIdle(IdleEvent _sender) {
            SetIdleAnimationParameters();
        }



        private void MoveToPositionEvent_OnMoveToPosition(MoveToPositionEvent _sender, MoveToPositionEventArgs _args) {
            SetMoveAnimationParameters();
        }



        private void AimEvent_OnAimAction(AimEvent _sender, AimEventArgs _args) {
            DisableAllAimAnimationParameters();

            SetAimAnimationParameters(_args.direction);
        }



        private void SetIdleAnimationParameters() {
            enemy.animator.SetBool(Settings.IsIdle, true);
            enemy.animator.SetBool(Settings.IsMoving, false);
        }



        private void DisableAllAimAnimationParameters() {
            enemy.animator.SetBool(Settings.AimUp, false);
            enemy.animator.SetBool(Settings.AimUpRight, false);
            enemy.animator.SetBool(Settings.AimUpLeft, false);
            enemy.animator.SetBool(Settings.AimRight, false);
            enemy.animator.SetBool(Settings.AimDown, false);
            enemy.animator.SetBool(Settings.AimLeft, false);
        }



        private void SetAimAnimationParameters(Direction _direction) {
            switch (_direction) {
                case Direction.UP:
                    enemy.animator.SetBool(Settings.AimUp, true);
                    break;
                case Direction.UP_RIGHT:
                    enemy.animator.SetBool(Settings.AimUpRight, true);
                    break;
                case Direction.UP_LEFT:
                    enemy.animator.SetBool(Settings.AimUpLeft, true);
                    break;
                case Direction.RIGHT:
                    enemy.animator.SetBool(Settings.AimRight, true);
                    break;
                case Direction.DOWN:
                    enemy.animator.SetBool(Settings.AimDown, true);
                    break;
                case Direction.LEFT:
                    enemy.animator.SetBool(Settings.AimLeft, true);
                    break;
            }
        }



        private void SetMoveAnimationParameters() {
            enemy.animator.SetBool(Settings.IsIdle, false);
            enemy.animator.SetBool(Settings.IsMoving, true);
        }
    }
}

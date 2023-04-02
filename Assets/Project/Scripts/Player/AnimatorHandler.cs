using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Player))]
    #endregion
    public class AnimatorHandler : MonoBehaviour {
        private Player player;



        private void Awake() {
            player = GetComponent<Player>();
        }



        public void OnEnable() {
            player.idleEvent.OnIdle += IdleEvent_OnIdle;
            player.aimEvent.OnAim += AimEvent_OnAim;

            player.moveByVelocityEvent.OnMoveByVelocity += MoveByVelocityEvent_OnMoveByVelocity;
            player.moveToPositionEvent.OnMoveToPosition += MoveToPositionEvent_OnMoveToPosition;
        }



        public void OnDisable() {
            player.idleEvent.OnIdle -= IdleEvent_OnIdle;
            player.aimEvent.OnAim -= AimEvent_OnAim;

            player.moveByVelocityEvent.OnMoveByVelocity -= MoveByVelocityEvent_OnMoveByVelocity;
            player.moveToPositionEvent.OnMoveToPosition -= MoveToPositionEvent_OnMoveToPosition;
        }



        private void IdleEvent_OnIdle(IdleEvent sender) {
            DisableRollAnimationParameters();

            SetIdleAnimationParameters();
        }



        private void AimEvent_OnAim(AimEvent sender, AimEventArgs args) {
            DisableAllAimAnimationParameters();
            DisableRollAnimationParameters();

            SetAimAnimationParameters(args.direction);
        }



        private void MoveByVelocityEvent_OnMoveByVelocity(MoveByVelocityEvent sender, MoveByVelocityEventArgs args) {
            DisableRollAnimationParameters();

            SetMoveAnimationParameters();
        }



        private void MoveToPositionEvent_OnMoveToPosition(MoveToPositionEvent sender, MoveToPositionEventArgs args) {
            DisableAllAimAnimationParameters();
            DisableRollAnimationParameters();

            SetRollAnimationParameters(args);
        }



        private void SetIdleAnimationParameters() {
            player.animator.SetBool(Settings.IsIdle, true);
            player.animator.SetBool(Settings.IsMoving, false);
        }



        private void DisableAllAimAnimationParameters() {
            player.animator.SetBool(Settings.AimUp, false);
            player.animator.SetBool(Settings.AimUpRight, false);
            player.animator.SetBool(Settings.AimUpLeft, false);
            player.animator.SetBool(Settings.AimRight, false);
            player.animator.SetBool(Settings.AimDown, false);
            player.animator.SetBool(Settings.AimLeft, false);
        }



        private void SetAimAnimationParameters(Direction direction) {
            switch (direction) {
                case Direction.UP:
                    player.animator.SetBool(Settings.AimUp, true);
                    break;
                case Direction.UP_RIGHT:
                    player.animator.SetBool(Settings.AimUpRight, true);
                    break;
                case Direction.UP_LEFT:
                    player.animator.SetBool(Settings.AimUpLeft, true);
                    break;
                case Direction.RIGHT:
                    player.animator.SetBool(Settings.AimRight, true);
                    break;
                case Direction.DOWN:
                    player.animator.SetBool(Settings.AimDown, true);
                    break;
                case Direction.LEFT:
                    player.animator.SetBool(Settings.AimLeft, true);
                    break;
            }
        }



        private void SetMoveAnimationParameters() {
            player.animator.SetBool(Settings.IsIdle, false);
            player.animator.SetBool(Settings.IsMoving, true);
        }



        private void DisableRollAnimationParameters() {
            player.animator.SetBool(Settings.RollUp, false);
            player.animator.SetBool(Settings.RollRight, false);
            player.animator.SetBool(Settings.RollDown, false);
            player.animator.SetBool(Settings.RollLeft, false);
        }



        private void SetRollAnimationParameters(MoveToPositionEventArgs args) {
            if (args.isActive) {
                if(args.directionVector.x > 0) {
                    player.animator.SetBool(Settings.RollRight, true);
                } else if (args.directionVector.x < 0) {
                    player.animator.SetBool(Settings.RollLeft, true);
                } else if (args.directionVector.y > 0) {
                    player.animator.SetBool(Settings.RollUp, true);
                } else if (args.directionVector.y < 0) {
                    player.animator.SetBool(Settings.RollDown, true);
                }
            }
        }
    }
}

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
        }



        public void OnDisable() {
            player.idleEvent.OnIdle -= IdleEvent_OnIdle;
            player.aimEvent.OnAim -= AimEvent_OnAim;

            player.moveByVelocityEvent.OnMoveByVelocity -= MoveByVelocityEvent_OnMoveByVelocity;
        }



        private void IdleEvent_OnIdle(IdleEvent idleEvent) {
            SetIdleAnimationParameters();
        }



        private void AimEvent_OnAim(AimEvent aimEvent, AimEventArgs aimEventArgs) {
            DisableAllAimAnimationParameters();

            SetAimAnimationParameters(aimEventArgs.direction);
        }



        private void MoveByVelocityEvent_OnMoveByVelocity(MoveByVelocityEvent moveByVelocityEvent, MoveByVelocityEventArgs moveByVelocityEventArgs) {
            SetMovingAnimationParameters();
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



        private void SetMovingAnimationParameters() {
            player.animator.SetBool(Settings.IsIdle, false);
            player.animator.SetBool(Settings.IsMoving, true);
        }
    }
}

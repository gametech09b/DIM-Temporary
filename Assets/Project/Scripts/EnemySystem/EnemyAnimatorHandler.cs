using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner.EnemySystem
{
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Enemy))]
    #endregion
    public class EnemyAnimatorHandler : MonoBehaviour
    {
        private Enemy _enemy;



        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
        }



        private void OnEnable()
        {
            _enemy.idleEvent.OnIdle += IdleEvent_OnIdle;
            _enemy.moveToPositionEvent.OnMoveToPosition += MoveToPositionEvent_OnMoveToPosition;
        }



        private void OnDisable()
        {
            _enemy.idleEvent.OnIdle -= IdleEvent_OnIdle;
            _enemy.moveToPositionEvent.OnMoveToPosition -= MoveToPositionEvent_OnMoveToPosition;
        }



        private void IdleEvent_OnIdle(IdleEvent sender)
        {
            SetIdleAnimationParameters();
        }



        private void MoveToPositionEvent_OnMoveToPosition(MoveToPositionEvent sender, MoveToPositionEventArgs args)
        {
            DisableAllAimAnimationParameters();

            // FIXME: FIX LATER
            Vector3 position = _enemy.transform.position;
            Vector3 targetPosition = GameManager.Instance.GetCurrentPlayer().transform.position;
            if (position.x < targetPosition.x)
            {
                SetAimAnimationParameters(Direction.RIGHT);
            }
            else
            {
                SetAimAnimationParameters(Direction.LEFT);
            }

            SetMoveAnimationParameters();
        }



        private void SetIdleAnimationParameters()
        {
            _enemy.animator.SetBool(Settings.IsIdle, true);
            _enemy.animator.SetBool(Settings.IsMoving, false);
        }



        private void DisableAllAimAnimationParameters()
        {
            _enemy.animator.SetBool(Settings.AimUp, false);
            _enemy.animator.SetBool(Settings.AimUpRight, false);
            _enemy.animator.SetBool(Settings.AimUpLeft, false);
            _enemy.animator.SetBool(Settings.AimRight, false);
            _enemy.animator.SetBool(Settings.AimDown, false);
            _enemy.animator.SetBool(Settings.AimLeft, false);
        }



        private void SetAimAnimationParameters(Direction direction)
        {
            switch (direction)
            {
                case Direction.UP:
                    _enemy.animator.SetBool(Settings.AimUp, true);
                    break;
                case Direction.UP_RIGHT:
                    _enemy.animator.SetBool(Settings.AimUpRight, true);
                    break;
                case Direction.UP_LEFT:
                    _enemy.animator.SetBool(Settings.AimUpLeft, true);
                    break;
                case Direction.RIGHT:
                    _enemy.animator.SetBool(Settings.AimRight, true);
                    break;
                case Direction.DOWN:
                    _enemy.animator.SetBool(Settings.AimDown, true);
                    break;
                case Direction.LEFT:
                    _enemy.animator.SetBool(Settings.AimLeft, true);
                    break;
            }
        }



        private void SetMoveAnimationParameters()
        {
            _enemy.animator.SetBool(Settings.IsIdle, false);
            _enemy.animator.SetBool(Settings.IsMoving, true);
        }
    }
}

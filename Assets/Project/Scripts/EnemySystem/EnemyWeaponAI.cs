using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Enemy))]
    #endregion
    public class EnemyWeaponAI : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Transform shootPosition;

        private Enemy enemy;
        private EnemyDetailSO enemyDetail;

        private float fireIntervalTimer;
        private float fireDurationTimer;



        private void Awake()
        {
            enemy = GetComponent<Enemy>();
        }



        private void Start()
        {
            enemyDetail = enemy.enemyDetail;

            fireIntervalTimer = GetFireInterval();
            fireDurationTimer = GetFireDuration();
        }



        private void Update()
        {
            fireIntervalTimer -= Time.deltaTime;

            if (fireIntervalTimer < -0f)
            {
                if (fireDurationTimer >= 0)
                {
                    fireDurationTimer -= Time.deltaTime;
                    Fire();
                }
                else
                {
                    fireIntervalTimer = GetFireInterval();
                    fireDurationTimer = GetFireDuration();
                }
            }
        }



        private float GetFireInterval()
        {
            return Random.Range(enemyDetail.minFireInterval, enemyDetail.maxFireInterval);
        }



        private float GetFireDuration()
        {
            return Random.Range(enemyDetail.minFireDuration, enemyDetail.maxFireDuration);
        }



        private void Fire()
        {
            Vector3 playerDirectionVector = GameManager.Instance.GetCurrentPlayer().GetPosition() - transform.position;
            Vector3 weaponDirectionVector = GameManager.Instance.GetCurrentPlayer().GetPosition() - shootPosition.position;

            float angle = HelperUtilities.GetAngleFromVector(playerDirectionVector);
            float weaponAngle = HelperUtilities.GetAngleFromVector(weaponDirectionVector);

            Direction direction = HelperUtilities.GetDirectionFromAngle(angle);

            enemy.aimEvent.CallOnAimAction(direction, angle, weaponAngle, weaponDirectionVector);

            if (enemyDetail.weaponDetail != null)
            {
                float ammoRange = enemyDetail.weaponDetail.ammoDetail.range;

                if (playerDirectionVector.magnitude <= ammoRange)
                {
                    if (enemyDetail.isRequireTargetOnSight
                    && !IsTargetOnSight(weaponDirectionVector, ammoRange))
                        return;

                    enemy.fireEvent.CallOnFireAction(true, true, direction, angle, weaponAngle, weaponDirectionVector);
                }
            }
        }



        private bool IsTargetOnSight(Vector3 _weaponDirectionVector, float _ammoRange)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(shootPosition.position, (Vector2)_weaponDirectionVector, _ammoRange, layerMask);

            if (raycastHit2D
            && raycastHit2D.transform.CompareTag(Settings.PlayerTag))
                return true;

            return false;
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckNullValue(this, nameof(shootPosition), shootPosition);
        }
#endif
        #endregion
    }
}

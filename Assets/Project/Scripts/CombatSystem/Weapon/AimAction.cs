using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(AimEvent))]
    #endregion
    public class AimAction : MonoBehaviour
    {
        [Tooltip("The transform of the player's weapon")]
        [SerializeField] private Transform weaponRotationPointTransform;

        private AimEvent aimEvent;



        private void Awake()
        {
            aimEvent = GetComponent<AimEvent>();
        }



        private void OnEnable()
        {
            aimEvent.OnAimAction += AimEvent_OnAim;
        }



        private void OnDisable()
        {
            aimEvent.OnAimAction -= AimEvent_OnAim;
        }



        private void AimEvent_OnAim(AimEvent _aimEvent, AimEventArgs _aimEventArgs)
        {
            AimToMousePosition(_aimEventArgs.direction, _aimEventArgs.angle);
        }



        private void AimToMousePosition(Direction _direction, float _angle)
        {
            weaponRotationPointTransform.eulerAngles = new Vector3(0f, 0f, _angle);

            switch (_direction)
            {
                case Direction.UP_LEFT:
                case Direction.LEFT:
                    weaponRotationPointTransform.localScale = new Vector3(1f, -1f, 0);
                    break;
                case Direction.UP:
                case Direction.UP_RIGHT:
                case Direction.RIGHT:
                case Direction.DOWN:
                    weaponRotationPointTransform.localScale = new Vector3(1f, 1f, 0);
                    break;
            }
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckNullValue(this, nameof(weaponRotationPointTransform), weaponRotationPointTransform);
        }
#endif
        #endregion
    }
}

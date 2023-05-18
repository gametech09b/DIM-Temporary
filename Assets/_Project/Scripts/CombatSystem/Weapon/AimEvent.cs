using System;
using UnityEngine;

namespace DIM.CombatSystem {
    [DisallowMultipleComponent]
    public class AimEvent : MonoBehaviour {
        public event Action<AimEvent, AimEventArgs> OnAimAction;

        public void CallOnAimAction(Direction _direction, float _angle, float _weaponAngle, Vector3 _weaponDirectionVector) {
            OnAimAction?.Invoke(
                this,
                new AimEventArgs() {
                    direction = _direction,
                    angle = _angle,
                    weaponAngle = _weaponAngle,
                    weaponDirectionVector = _weaponDirectionVector
                });
        }
    }

    public class AimEventArgs : EventArgs {
        public Direction direction;
        public float angle;
        public float weaponAngle;
        public Vector3 weaponDirectionVector;
    }
}

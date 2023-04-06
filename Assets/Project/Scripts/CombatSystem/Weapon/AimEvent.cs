using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class AimEvent : MonoBehaviour {
        public event Action<AimEvent, AimEventArgs> OnAimAction;

        public void CallOnAimAction(Direction direction, float angle, float weaponAngle, Vector3 weaponDirectionVector) {
            OnAimAction?.Invoke(
                this,
                new AimEventArgs() {
                    direction = direction,
                    angle = angle,
                    weaponAngle = weaponAngle,
                    weaponDirectionVector = weaponDirectionVector
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

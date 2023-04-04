using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class AimEvent : MonoBehaviour {
        public event Action<AimEvent, AimEventArgs> OnAim;

        public void CallOnAimEvent(Direction direction, float angle, float weaponAngle, Vector3 weaponDirectionVector) {
            OnAim?.Invoke(
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

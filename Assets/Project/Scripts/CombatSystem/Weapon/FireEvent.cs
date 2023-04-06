using System;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class FireEvent : MonoBehaviour {
        public event Action<FireEvent, OnFireActionArgs> OnFireAction;

        public void CallOnFireAction(bool isFire, bool isFiringPreviousFrame, Direction direction, float angle, float weaponAngle, Vector3 weaponDirectionVector) {
            OnFireAction?.Invoke(
                this,
                new OnFireActionArgs() {
                    isFiring = isFire,
                    isFiringPreviousFrame = isFiringPreviousFrame,
                    direction = direction,
                    angle = angle,
                    weaponAngle = weaponAngle,
                    weaponDirectionVector = weaponDirectionVector
                });
        }



        public event Action<FireEvent, OnFired> OnFired;

        public void CallOnFired(Weapon weapon) {
            OnFired?.Invoke(
                this,
                new OnFired() {
                    weapon = weapon
                });
        }
    }



    public class OnFireActionArgs : EventArgs {
        public bool isFiring;
        public bool isFiringPreviousFrame;
        public Direction direction;
        public float angle;
        public float weaponAngle;
        public Vector3 weaponDirectionVector;
    }



    public class OnFired : EventArgs {
        public Weapon weapon;
    }
}

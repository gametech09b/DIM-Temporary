using System;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class FireEvent : MonoBehaviour {
        public event Action<FireEvent, OnFireActionArgs> OnFireAction;

        public void CallOnFireAction(bool _isFire, bool _isFiringPreviousFrame, Direction _direction, float _angle, float _weaponAngle, Vector3 _weaponDirectionVector) {
            OnFireAction?.Invoke(
                this,
                new OnFireActionArgs() {
                    isFiring = _isFire,
                    isFiringPreviousFrame = _isFiringPreviousFrame,
                    direction = _direction,
                    angle = _angle,
                    weaponAngle = _weaponAngle,
                    weaponDirectionVector = _weaponDirectionVector
                });
        }



        public event Action<FireEvent, OnFiredArgs> OnFired;

        public void CallOnFired(Weapon _weapon) {
            OnFired?.Invoke(
                this,
                new OnFiredArgs() {
                    weapon = _weapon
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



    public class OnFiredArgs : EventArgs {
        public Weapon weapon;
    }
}

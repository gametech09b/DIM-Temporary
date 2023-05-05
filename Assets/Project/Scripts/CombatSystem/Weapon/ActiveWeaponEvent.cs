using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class ActiveWeaponEvent : MonoBehaviour {
        public event Action<ActiveWeaponEvent, OnSetActiveWeaponArgs> OnSetActiveWeapon;
        public void CallOnSetActiveWeapon(Weapon _weapon) {
            OnSetActiveWeapon?.Invoke(this, new OnSetActiveWeaponArgs {
                weapon = _weapon
            });
        }
    }



    public class OnSetActiveWeaponArgs {
        public Weapon weapon;
    }
}

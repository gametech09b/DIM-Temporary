using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class ActiveWeaponEvent : MonoBehaviour {
        public event Action<ActiveWeaponEvent, SetActiveWeaponEventArgs> OnSetActiveWeapon;
        public void CallOnSetActiveWeapon(Weapon weapon) {
            OnSetActiveWeapon?.Invoke(this, new SetActiveWeaponEventArgs {
                weapon = weapon
            });
        }
    }



    public class SetActiveWeaponEventArgs {
        public Weapon weapon;
    }
}

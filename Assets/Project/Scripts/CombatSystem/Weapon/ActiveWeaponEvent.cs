using System;
using UnityEngine;

namespace DIM.CombatSystem {
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

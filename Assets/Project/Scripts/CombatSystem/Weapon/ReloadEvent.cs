using System;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class ReloadEvent : MonoBehaviour {
        public event Action<ReloadEvent, OnReloadActionArgs> OnReloadAction;

        public void CallOnReloadAction(Weapon weapon, int reloadAmmoPercent) {
            OnReloadAction?.Invoke(this, new OnReloadActionArgs {
                weapon = weapon,
                reloadAmmoPercent = reloadAmmoPercent
            });
        }



        public event Action<ReloadEvent, OnReloadedArgs> OnReloaded;

        public void CallOnReloaded(Weapon weapon) {
            OnReloaded?.Invoke(this, new OnReloadedArgs {
                weapon = weapon
            });
        }
    }



    public class OnReloadActionArgs : EventArgs {
        public Weapon weapon;
        public int reloadAmmoPercent;
    }



    public class OnReloadedArgs : EventArgs {
        public Weapon weapon;
    }
}

using System;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class ReloadEvent : MonoBehaviour {
        public event Action<ReloadEvent, OnReloadActionArgs> OnReloadAction;

        public void CallOnReloadAction(Weapon _weapon, int _reloadAmmoPercent) {
            OnReloadAction?.Invoke(this, new OnReloadActionArgs {
                weapon = _weapon,
                reloadAmmoPercent = _reloadAmmoPercent
            });
        }



        public event Action<ReloadEvent, OnReloadedArgs> OnReloaded;

        public void CallOnReloaded(Weapon _weapon) {
            OnReloaded?.Invoke(this, new OnReloadedArgs {
                weapon = _weapon
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

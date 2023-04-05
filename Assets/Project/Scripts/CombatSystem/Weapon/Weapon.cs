using UnityEngine;

namespace DungeonGunner {
    public class Weapon {
        public WeaponDetailSO weaponDetail;
        public int indexOnList;
        public int clipRemainingAmmo;
        public int remainingAmmo;
        public bool isReload;
        public float reloadTimer;



        public Weapon(WeaponDetailSO weaponDetail) {
            this.weaponDetail = weaponDetail;
            this.clipRemainingAmmo = weaponDetail.clipCapacity;
            this.remainingAmmo = weaponDetail.ammoCapacity;
        }
    }
}

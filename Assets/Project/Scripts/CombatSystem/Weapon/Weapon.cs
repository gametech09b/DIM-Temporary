using UnityEngine;

namespace DungeonGunner {
    public class Weapon {
        public WeaponDetailSO weaponDetail;
        public int indexOnList;
        public int ammoPerClipRemaining;
        public int ammoRemaining;
        public bool isReloading;
        public float reloadTimer;



        public Weapon(WeaponDetailSO weaponDetail) {
            this.weaponDetail = weaponDetail;
            this.ammoPerClipRemaining = weaponDetail.ammoPerClipCapacity;
            this.ammoRemaining = weaponDetail.ammoCapacity;
        }
    }
}

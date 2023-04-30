using UnityEngine;

namespace DungeonGunner
{
    public class Weapon
    {
        public WeaponDetailSO weaponDetail;
        public int indexOnList;
        public int ammoPerClipRemaining;
        public int ammoRemaining;
        public bool isReloading;
        public float reloadTimer;



        public Weapon(WeaponDetailSO _weaponDetail)
        {
            this.weaponDetail = _weaponDetail;
            this.ammoPerClipRemaining = _weaponDetail.ammoPerClipCapacity;
            this.ammoRemaining = _weaponDetail.ammoCapacity;
        }
    }
}

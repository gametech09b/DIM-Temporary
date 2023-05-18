namespace DIM.CombatSystem {
    public class Weapon {
        public WeaponDetailSO weaponDetail;
        public int indexOnList;
        public int ammoRemaining;
        public int ammoPerClipRemaining;
        public bool isReloading;
        public float reloadTimer;

        // ===================================================================

        public Weapon(WeaponDetailSO _weaponDetail) {
            this.weaponDetail = _weaponDetail;
            this.ammoRemaining = _weaponDetail.ammoCapacity;
            this.ammoPerClipRemaining = _weaponDetail.ammoPerClipCapacity;
        }
    }
}

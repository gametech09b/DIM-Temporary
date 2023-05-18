using UnityEngine;

using DIM.AudioSystem;

namespace DIM.CombatSystem {
    [CreateAssetMenu(fileName = "WeaponDetail_", menuName = "Scriptable Objects/Combat/Weapon Detail")]
    public class WeaponDetailSO : ScriptableObject {

        [Space(10)]
        [Header("Weapon Base Detail")]

        public string weaponName;

        [Tooltip("The sprite weapon - sprite should have the 'generate physics shape' option checked")]
        public Sprite sprite;


        [Space(10)]
        [Header("Weapon Configuration")]

        public Vector3 shootPosition;
        public AmmoDetailSO ammoDetail;
        public ShootEffectSO shootEffect;


        [Space(10)]
        [Header("Weapon Operating Value")]

        public bool isAmmoInfinite = false;
        public int ammoCapacity = 100;
        public bool isAmmoPerClipInfinite = false;
        public int ammoPerClipCapacity = 6;
        public float fireRate = 0.2f;
        public float prefireDelay = 0f;
        public float reloadTime = 0f;


        [Space(10)]
        [Header("Weapon Sound Effect")]

        public SoundEffectSO fireSoundEffect;
        public SoundEffectSO reloadSoundEffect;

        // ===================================================================

        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckEmptyString(this, nameof(weaponName), weaponName);
            HelperUtilities.CheckNullValue(this, nameof(ammoDetail), ammoDetail);
            HelperUtilities.CheckPositiveValue(this, nameof(fireRate), fireRate);
            HelperUtilities.CheckPositiveValue(this, nameof(prefireDelay), prefireDelay, true);

            if (!isAmmoInfinite)
                HelperUtilities.CheckPositiveValue(this, nameof(ammoCapacity), ammoCapacity);

            if (!isAmmoPerClipInfinite)
                HelperUtilities.CheckPositiveValue(this, nameof(ammoPerClipCapacity), ammoPerClipCapacity);
        }
#endif
        #endregion
    }
}
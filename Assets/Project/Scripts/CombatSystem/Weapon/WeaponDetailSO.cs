using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [CreateAssetMenu(fileName = "WeaponDetail_", menuName = "Scriptable Objects/Combat/Weapon Detail")]
    public class WeaponDetailSO : ScriptableObject {

        [Space(10)]
        [Header("Weapon Base Detail")]


        [Tooltip("Weapon Name")]
        public string weaponName;

        [Tooltip("The sprite weapon - sprite should have the 'generate physics shape' option checked")]
        public Sprite sprite;



        [Space(10)]
        [Header("Weapon Configuration")]


        [Tooltip("Weapon shoot position - offset position from the weapon sprite pivot point")]
        public Vector3 shootPosition;

        [Tooltip("Weapon current ammo")]
        public AmmoDetailSO ammoDetail;



        [Space(10)]
        [Header("Weapon Operating Value")]


        [Tooltip("Set if the weapon has infinite ammo")]
        public bool isAmmoInfinite = false;

        [Tooltip("Weapon ammo capacity per clip")]
        public int ammoCapacity = 100;

        [Tooltip("Set if the weapon has infinite clip capacity")]
        public bool isClipInfinite = false;

        [Tooltip("Weapon clip capacity")]
        public int clipCapacity = 6;

        [Tooltip("Weapon fire rate - 0.2 means 5 shots per second")]
        public float fireRate = 0.2f;

        [Tooltip("Weapon prefire delay - time in seconds to holds fire button down before firing")]
        public float prefireDelay = 0f;

        [Tooltip("Weapon reload time - time in seconds to reload the weapon")]
        public float reloadTime = 0f;



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.ValidateCheckEmptyString(this, nameof(weaponName), weaponName);
            HelperUtilities.ValidateCheckNullValue(this, nameof(ammoDetail), ammoDetail);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(fireRate), fireRate);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(prefireDelay), prefireDelay, true);

            if (!isAmmoInfinite)
                HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoCapacity), ammoCapacity);

            if (!isClipInfinite)
                HelperUtilities.ValidateCheckPositiveValue(this, nameof(clipCapacity), clipCapacity);
        }
#endif
        #endregion
    }
}
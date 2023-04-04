using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [CreateAssetMenu(fileName = "AmmoDetail_", menuName = "Scriptable Objects/Combat/Ammo Detail")]
    public class AmmoDetailSO : ScriptableObject {
        [Space(10)]
        [Header("Ammo Base Detail")]


        [Tooltip("Ammo Name")]
        public string ammoName;

        public bool isPlayerAmmo;

        [Tooltip("The sprite ammo - sprite should have the 'generate physics shape' option checked")]
        public Sprite sprite;

        [Tooltip("The ammo prefab array")]
        public GameObject[] prefabArray;

        [Tooltip("The ammo material")]
        public Material material;



        [Space(10)]
        [Header("Ammo Configuration")]


        [Tooltip("Ammo damage")]
        public int damage = 1;

        [Tooltip("Ammo minimum speed")]
        public float minSpeed = 20f;

        [Tooltip("Ammo maximum speed")]
        public float maxSpeed = 20f;

        [Tooltip("Ammo range")]
        public float range = 20f;

        [Tooltip("Ammo rotation speed")]
        public float rotationSpeed = 1f;



        [Space(10)]
        [Header("Ammo Charge Detail")]


        [Tooltip("Ammo chargeTime - time in seconds to charge the ammo")]
        public float chargeTime = 0.1f;

        [Tooltip("The ammo charge material")]
        public Material chargeMaterial;



        [Space(10)]
        [Header("Ammo Spread Detail")]


        [Tooltip("Ammo minimum spread angle - angle in degrees to spread the ammo")]
        public float minSpreadAngle = 0f;

        [Tooltip("Ammo maximum spread angle - angle in degrees to spread the ammo")]
        public float maxSpreadAngle = 0f;



        [Space(10)]
        [Header("Ammo Spawn Detail")]


        [Tooltip("Minimum ammo spawn count per shot")]
        public int minSpawnCount = 1;

        [Tooltip("Maximum ammo spawn count per shot")]
        public int maxSpawnCount = 1;

        [Tooltip("Ammo minimum spawn interval")]
        public float minSpawnInterval = 0f;

        [Tooltip("Ammo maximum spawn interval")]
        public float maxSpawnInterval = 0f;



        [Space(10)]
        [Header("Ammo Trail Detail")]


        [Tooltip("Set if ammo has trail")]
        public bool isTrailEnabled = false;

        [Tooltip("Ammo trail lifetime in seconds")]
        public float trailLifetime = 3f;

        [Tooltip("Ammo trail material")]
        public Material trailMaterial;

        [Tooltip("Ammo trail start width")]
        [Range(0f, 1f)] public float trailStartWidth;

        [Tooltip("Ammo trail end width")]
        [Range(0f, 1f)] public float trailEndWidth;



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.ValidateCheckEmptyString(this, nameof(ammoName), ammoName);
            HelperUtilities.ValidateCheckNullValue(this, nameof(sprite), sprite);
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(prefabArray), prefabArray);
            HelperUtilities.ValidateCheckNullValue(this, nameof(material), material);

            if (chargeTime > 0) {
                HelperUtilities.ValidateCheckNullValue(this, nameof(chargeMaterial), chargeMaterial);
            }

            HelperUtilities.ValidateCheckPositiveValue(this, nameof(damage), damage);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(minSpeed), minSpeed);
            HelperUtilities.ValidateCheckPositiveRange(this, nameof(minSpeed), nameof(maxSpeed), minSpeed, maxSpeed);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(range), range);
            HelperUtilities.ValidateCheckPositiveRange(this, nameof(minSpreadAngle), nameof(maxSpreadAngle), minSpreadAngle, maxSpreadAngle, true);
            HelperUtilities.ValidateCheckPositiveRange(this, nameof(minSpawnCount), nameof(maxSpawnCount), minSpawnCount, maxSpawnCount);
            HelperUtilities.ValidateCheckPositiveRange(this, nameof(minSpawnInterval), nameof(maxSpawnInterval), minSpawnInterval, maxSpawnInterval, true);

            if (isTrailEnabled) {
                HelperUtilities.ValidateCheckPositiveValue(this, nameof(trailLifetime), trailLifetime);
                HelperUtilities.ValidateCheckNullValue(this, nameof(trailMaterial), trailMaterial);
                HelperUtilities.ValidateCheckPositiveValue(this, nameof(trailStartWidth), trailStartWidth);
                HelperUtilities.ValidateCheckPositiveValue(this, nameof(trailEndWidth), trailEndWidth);
            }

        }
#endif
        #endregion
    }
}

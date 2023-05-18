using UnityEngine;

namespace DIM.CombatSystem {
    [CreateAssetMenu(fileName = "AmmoDetail_", menuName = "Scriptable Objects/Combat/Ammo Detail")]
    public class AmmoDetailSO : ScriptableObject {
        [Space(10)]
        [Header("Ammo Base Detail")]

        public string ammoName;
        public bool isPlayerAmmo;

        [Tooltip("The sprite ammo - sprite should have the 'generate physics shape' option checked")]
        public Sprite sprite;

        public GameObject[] prefabArray;
        public Material material;

        public HitEffectSO hitEffect;


        [Space(10)]
        [Header("Ammo Configuration")]

        public int damage = 1;
        public float minSpeed = 20f;
        public float maxSpeed = 20f;
        public float range = 20f;
        public float rotationSpeed = 1f;


        [Space(10)]
        [Header("Ammo Charge Detail")]

        public float chargeTime = 0.1f;
        public Material chargeMaterial;


        [Space(10)]
        [Header("Ammo Spread Detail")]

        public float minSpread = 0f;
        public float maxSpread = 0f;


        [Space(10)]
        [Header("Ammo Spawn Detail")]

        public int minSpawnCount = 1;
        public int maxSpawnCount = 1;
        public float minSpawnInterval = 0f;
        public float maxSpawnInterval = 0f;


        [Space(10)]
        [Header("Ammo Trail Detail")]

        public bool isTrailEnabled = false;
        public float trailLifetime = 3f;
        public Material trailMaterial;
        [Range(0f, 1f)] public float trailStartWidth;
        [Range(0f, 1f)] public float trailEndWidth;

        // ===================================================================

        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckEmptyString(this, nameof(ammoName), ammoName);
            HelperUtilities.CheckNullValue(this, nameof(sprite), sprite);
            HelperUtilities.CheckEnumerableValue(this, nameof(prefabArray), prefabArray);
            HelperUtilities.CheckNullValue(this, nameof(material), material);

            if (chargeTime > 0) {
                HelperUtilities.CheckNullValue(this, nameof(chargeMaterial), chargeMaterial);
            }

            HelperUtilities.CheckPositiveValue(this, nameof(damage), damage);
            HelperUtilities.CheckPositiveValue(this, nameof(minSpeed), minSpeed);
            HelperUtilities.CheckPositiveRange(this, nameof(minSpeed), nameof(maxSpeed), minSpeed, maxSpeed);
            HelperUtilities.CheckPositiveValue(this, nameof(range), range);
            HelperUtilities.CheckPositiveRange(this, nameof(minSpread), nameof(maxSpread), minSpread, maxSpread, true);
            HelperUtilities.CheckPositiveRange(this, nameof(minSpawnCount), nameof(maxSpawnCount), minSpawnCount, maxSpawnCount);
            HelperUtilities.CheckPositiveRange(this, nameof(minSpawnInterval), nameof(maxSpawnInterval), minSpawnInterval, maxSpawnInterval, true);

            if (isTrailEnabled) {
                HelperUtilities.CheckPositiveValue(this, nameof(trailLifetime), trailLifetime);
                HelperUtilities.CheckNullValue(this, nameof(trailMaterial), trailMaterial);
                HelperUtilities.CheckPositiveValue(this, nameof(trailStartWidth), trailStartWidth);
                HelperUtilities.CheckPositiveValue(this, nameof(trailEndWidth), trailEndWidth);
            }

        }
#endif
        #endregion
    }
}

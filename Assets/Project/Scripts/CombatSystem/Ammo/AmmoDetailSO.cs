using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [CreateAssetMenu(fileName = "AmmoDetail_", menuName = "Scriptable Objects/Combat/Ammo Detail")]
    public class AmmoDetailSO : ScriptableObject
    {
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

        [Tooltip("The ammo particle system")]
        public HitEffectSO hitEffect;



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
        public float minSpread = 0f;

        [Tooltip("Ammo maximum spread angle - angle in degrees to spread the ammo")]
        public float maxSpread = 0f;



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
        private void OnValidate()
        {
            HelperUtilities.CheckEmptyString(this, nameof(ammoName), ammoName);
            HelperUtilities.CheckNullValue(this, nameof(sprite), sprite);
            HelperUtilities.CheckEnumerableValue(this, nameof(prefabArray), prefabArray);
            HelperUtilities.CheckNullValue(this, nameof(material), material);

            if (chargeTime > 0)
            {
                HelperUtilities.CheckNullValue(this, nameof(chargeMaterial), chargeMaterial);
            }

            HelperUtilities.CheckPositiveValue(this, nameof(damage), damage);
            HelperUtilities.CheckPositiveValue(this, nameof(minSpeed), minSpeed);
            HelperUtilities.CheckPositiveRange(this, nameof(minSpeed), nameof(maxSpeed), minSpeed, maxSpeed);
            HelperUtilities.CheckPositiveValue(this, nameof(range), range);
            HelperUtilities.CheckPositiveRange(this, nameof(minSpread), nameof(maxSpread), minSpread, maxSpread, true);
            HelperUtilities.CheckPositiveRange(this, nameof(minSpawnCount), nameof(maxSpawnCount), minSpawnCount, maxSpawnCount);
            HelperUtilities.CheckPositiveRange(this, nameof(minSpawnInterval), nameof(maxSpawnInterval), minSpawnInterval, maxSpawnInterval, true);

            if (isTrailEnabled)
            {
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

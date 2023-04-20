using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [CreateAssetMenu(fileName = "ShootEffect_", menuName = "Scriptable Objects/Effect/Shoot Effect")]
    public class ShootEffectSO : ScriptableObject
    {
        [Space(10)]
        [Header("Shoot Effect Detail")]


        public Gradient colorGradient;
        public float duration = 0.5f;
        public float startParticleSize = 0.25f;
        public float startParticleSpeed = 3f;
        public float startLifetime = 0.5f;
        public int maxParticleNumber = 100;
        public int maxEmissionRate = 100;
        public int burstParticleNumber = 20;
        public float effectGravity = -0.01f;

        public Sprite sprite;
        public GameObject prefab;

        public Vector3 minVelocityOverLifetime;
        public Vector3 maxVelocityOverLifetime;



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(duration), duration);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(startParticleSize), startParticleSize);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(startParticleSpeed), startParticleSpeed);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(startLifetime), startLifetime);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(maxParticleNumber), maxParticleNumber);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(maxEmissionRate), maxEmissionRate);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(burstParticleNumber), burstParticleNumber);
            HelperUtilities.ValidateCheckNullValue(this, nameof(prefab), prefab);
        }
#endif
        #endregion
    }
}

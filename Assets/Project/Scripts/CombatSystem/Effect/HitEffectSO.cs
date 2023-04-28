using UnityEngine;

namespace DungeonGunner
{
    [CreateAssetMenu(fileName = "HitEffect_", menuName = "Scriptable Objects/Effect/Hit Effect")]
    public class HitEffectSO : ScriptableObject
    {
        [Space(10)]
        [Header("Hit Effect Detail")]


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
            HelperUtilities.CheckPositiveValue(this, nameof(duration), duration);
            HelperUtilities.CheckPositiveValue(this, nameof(startParticleSize), startParticleSize);
            HelperUtilities.CheckPositiveValue(this, nameof(startParticleSpeed), startParticleSpeed);
            HelperUtilities.CheckPositiveValue(this, nameof(startLifetime), startLifetime);
            HelperUtilities.CheckPositiveValue(this, nameof(maxParticleNumber), maxParticleNumber);
            HelperUtilities.CheckPositiveValue(this, nameof(burstParticleNumber), burstParticleNumber);
            HelperUtilities.CheckNullValue(this, nameof(prefab), prefab);
        }
#endif
        #endregion
    }
}


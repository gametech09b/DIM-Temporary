using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(ParticleSystem))]
    #endregion
    public class HitEffect : MonoBehaviour
    {
        private ParticleSystem _particleSystem;



        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }



        public void Init(HitEffectSO hitEffect)
        {
            SetColorGradient(hitEffect.colorGradient);

            SetParticleStartingValues(hitEffect.duration, hitEffect.startParticleSize, hitEffect.startParticleSpeed, hitEffect.startLifetime, hitEffect.effectGravity, hitEffect.maxParticleNumber);

            SetEmissionRate(hitEffect.maxEmissionRate, hitEffect.burstParticleNumber);

            SetParticleSprite(hitEffect.sprite);

            SetVelocityOverLifetime(hitEffect.minVelocityOverLifetime, hitEffect.maxVelocityOverLifetime);
        }



        private void SetColorGradient(Gradient colorGradient)
        {
            ParticleSystem.ColorOverLifetimeModule colorOverLifetime = _particleSystem.colorOverLifetime;
            colorOverLifetime.color = colorGradient;
        }



        private void SetParticleStartingValues(float duration, float startParticleSize, float startParticleSpeed, float startLifetime, float effectGravity, int maxParticles)
        {
            ParticleSystem.MainModule mainModule = _particleSystem.main;

            mainModule.duration = duration;
            mainModule.startSize = startParticleSize;
            mainModule.startSpeed = startParticleSpeed;
            mainModule.startLifetime = startLifetime;
            mainModule.maxParticles = maxParticles;
        }



        private void SetEmissionRate(int maxEmissionRate, int burstParticleNumber)
        {
            ParticleSystem.EmissionModule emissionModule = _particleSystem.emission;

            emissionModule.rateOverTime = maxEmissionRate;

            ParticleSystem.Burst burst = new ParticleSystem.Burst(0, burstParticleNumber);
            emissionModule.SetBurst(0, burst);
        }



        private void SetParticleSprite(Sprite sprite)
        {
            ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = _particleSystem.textureSheetAnimation;

            textureSheetAnimationModule.SetSprite(0, sprite);
        }



        private void SetVelocityOverLifetime(Vector3 minVelocityOverLifetime, Vector3 maxVelocityOverLifetime)
        {
            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = _particleSystem.velocityOverLifetime;

            ParticleSystem.MinMaxCurve x = new ParticleSystem.MinMaxCurve();
            x.mode = ParticleSystemCurveMode.TwoConstants;
            x.constantMin = minVelocityOverLifetime.x;
            x.constantMax = maxVelocityOverLifetime.x;
            velocityOverLifetimeModule.x = x;

            ParticleSystem.MinMaxCurve y = new ParticleSystem.MinMaxCurve();
            y.mode = ParticleSystemCurveMode.TwoConstants;
            y.constantMin = minVelocityOverLifetime.y;
            y.constantMax = maxVelocityOverLifetime.y;
            velocityOverLifetimeModule.y = y;

        }
    }
}

using UnityEngine;

namespace DIM.CombatSystem {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(ParticleSystem))]
    #endregion
    public class HitEffect : MonoBehaviour {
        private ParticleSystem particleSystemComponent;

        // ===================================================================

        private void Awake() {
            particleSystemComponent = GetComponent<ParticleSystem>();
        }



        public void Init(HitEffectSO _hitEffect) {
            SetColorGradient(_hitEffect.colorGradient);

            SetParticleStartingValues(_hitEffect.duration, _hitEffect.startParticleSize, _hitEffect.startParticleSpeed, _hitEffect.startLifetime, _hitEffect.effectGravity, _hitEffect.maxParticleNumber);

            SetEmissionRate(_hitEffect.maxEmissionRate, _hitEffect.burstParticleNumber);

            SetParticleSprite(_hitEffect.sprite);

            SetVelocityOverLifetime(_hitEffect.minVelocityOverLifetime, _hitEffect.maxVelocityOverLifetime);
        }



        private void SetColorGradient(Gradient _colorGradient) {
            ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particleSystemComponent.colorOverLifetime;
            colorOverLifetime.color = _colorGradient;
        }



        private void SetParticleStartingValues(float _duration, float _startParticleSize, float _startParticleSpeed, float _startLifetime, float _effectGravity, int _maxParticles) {
            ParticleSystem.MainModule mainModule = particleSystemComponent.main;

            mainModule.duration = _duration;
            mainModule.startSize = _startParticleSize;
            mainModule.startSpeed = _startParticleSpeed;
            mainModule.startLifetime = _startLifetime;
            mainModule.maxParticles = _maxParticles;
        }



        private void SetEmissionRate(int _maxEmissionRate, int _burstParticleNumber) {
            ParticleSystem.EmissionModule emissionModule = particleSystemComponent.emission;

            emissionModule.rateOverTime = _maxEmissionRate;

            ParticleSystem.Burst burst = new ParticleSystem.Burst(0, _burstParticleNumber);
            emissionModule.SetBurst(0, burst);
        }



        private void SetParticleSprite(Sprite _sprite) {
            ParticleSystem.TextureSheetAnimationModule textureSheetAnimationModule = particleSystemComponent.textureSheetAnimation;

            textureSheetAnimationModule.SetSprite(0, _sprite);
        }



        private void SetVelocityOverLifetime(Vector3 _minVelocityOverLifetime, Vector3 _maxVelocityOverLifetime) {
            ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = particleSystemComponent.velocityOverLifetime;

            ParticleSystem.MinMaxCurve x = new ParticleSystem.MinMaxCurve();
            x.mode = ParticleSystemCurveMode.TwoConstants;
            x.constantMin = _minVelocityOverLifetime.x;
            x.constantMax = _maxVelocityOverLifetime.x;
            velocityOverLifetimeModule.x = x;

            ParticleSystem.MinMaxCurve y = new ParticleSystem.MinMaxCurve();
            y.mode = ParticleSystemCurveMode.TwoConstants;
            y.constantMin = _minVelocityOverLifetime.y;
            y.constantMax = _maxVelocityOverLifetime.y;
            velocityOverLifetimeModule.y = y;

        }
    }
}

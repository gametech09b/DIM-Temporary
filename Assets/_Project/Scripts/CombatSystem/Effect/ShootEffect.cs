using UnityEngine;

namespace DIM.CombatSystem {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(ParticleSystem))]
    #endregion
    public class ShootEffect : MonoBehaviour {
        private ParticleSystem particleSystemComponent;

        // ===================================================================

        private void Awake() {
            particleSystemComponent = GetComponent<ParticleSystem>();
        }



        public void Init(ShootEffectSO _shootEffect, float _angle) {
            SetColorGradient(_shootEffect.colorGradient);

            SetParticleStartingValues(_shootEffect.duration, _shootEffect.startParticleSize, _shootEffect.startParticleSpeed, _shootEffect.startLifetime, _shootEffect.effectGravity, _shootEffect.maxParticleNumber);

            SetEmissionRate(_shootEffect.maxEmissionRate, _shootEffect.burstParticleNumber);

            SetEmitterRotation(_angle);

            SetParticleSprite(_shootEffect.sprite);

            SetVelocityOverLifetime(_shootEffect.minVelocityOverLifetime, _shootEffect.maxVelocityOverLifetime);
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



        private void SetEmitterRotation(float _angle) {
            transform.eulerAngles = new Vector3(0, 0, _angle);
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

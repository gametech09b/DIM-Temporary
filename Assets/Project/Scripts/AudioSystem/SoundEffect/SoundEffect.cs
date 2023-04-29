using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(AudioSource))]
    #endregion
    public class SoundEffect : MonoBehaviour {

        private AudioSource audioSourceComponent;



        private void Awake() {
            audioSourceComponent = GetComponent<AudioSource>();
        }



        private void OnEnable() {
            if (audioSourceComponent.clip == null) return;

            audioSourceComponent.Play();
        }



        private void OnDisable() {
            audioSourceComponent.Stop();
        }



        public void SetSoundEffect(SoundEffectSO _soundEffect) {
            audioSourceComponent.clip = _soundEffect.audioClip;
            audioSourceComponent.volume = _soundEffect.volume;
            audioSourceComponent.pitch = Random.Range(_soundEffect.minPitchVariation, _soundEffect.maxPitchVariation);
        }
    }
}

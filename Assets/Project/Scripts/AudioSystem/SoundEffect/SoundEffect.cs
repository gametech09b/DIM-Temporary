using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(AudioSource))]
    #endregion
    public class SoundEffect : MonoBehaviour {

        private AudioSource audioSource;



        private void Awake() {
            audioSource = GetComponent<AudioSource>();
        }



        private void OnEnable() {
            if (audioSource.clip == null) return;

            audioSource.Play();
        }



        private void OnDisable() {
            audioSource.Stop();
        }



        public void SetSoundEffect(SoundEffectSO _soundEffect) {
            audioSource.clip = _soundEffect.audioClip;
            audioSource.volume = _soundEffect.volume;
            audioSource.pitch = Random.Range(_soundEffect.minPitchVariation, _soundEffect.maxPitchVariation);
        }
    }
}

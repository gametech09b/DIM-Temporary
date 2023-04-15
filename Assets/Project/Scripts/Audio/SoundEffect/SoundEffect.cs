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



        public void SetSoundEffect(SoundEffectSO soundEffect) {
            audioSourceComponent.clip = soundEffect.audioClip;
            audioSourceComponent.volume = soundEffect.volume;
            audioSourceComponent.pitch = Random.Range(soundEffect.minPitchVariation, soundEffect.maxPitchVariation);
        }
    }
}

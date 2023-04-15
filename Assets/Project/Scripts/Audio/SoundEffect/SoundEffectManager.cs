using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class SoundEffectManager : SingletonMonobehaviour<SoundEffectManager> {

        public int masterVolume = 8;



        private void Start() {
            SetSoundVolume(masterVolume);
        }



        public void PlaySoundEffect(SoundEffectSO soundEffect) {
            SoundEffect soundEffectInstance = (SoundEffect)PoolManager.Instance.ReuseComponent(soundEffect.prefab, Vector3.zero, Quaternion.identity);
            soundEffectInstance.SetSoundEffect(soundEffect);
            soundEffectInstance.gameObject.SetActive(true);

            StartCoroutine(DisableSoundEffect(soundEffectInstance, soundEffect.audioClip.length));
        }



        private IEnumerator DisableSoundEffect(SoundEffect soundEffect, float delay) {
            yield return new WaitForSeconds(delay);
            soundEffect.gameObject.SetActive(false);
        }



        public void SetSoundVolume(int volume) {
            float muteDecibel = -80f;

            if (volume == 0) {
                GameResources.Instance.AudioMixerGroup_Master.audioMixer.SetFloat("soundVolume", muteDecibel);
            } else {
                float decibel = HelperUtilities.ConvertLinearToDecibel(volume);
                GameResources.Instance.AudioMixerGroup_Master.audioMixer.SetFloat("soundVolume", decibel);
            }
        }
    }
}

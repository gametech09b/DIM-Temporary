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



        public void PlaySoundEffect(SoundEffectSO _soundEffect) {
            SoundEffect soundEffectInstance = (SoundEffect)PoolManager.Instance.ReuseComponent(_soundEffect.prefab, Vector3.zero, Quaternion.identity);
            soundEffectInstance.SetSoundEffect(_soundEffect);
            soundEffectInstance.gameObject.SetActive(true);

            StartCoroutine(DisableSoundEffect(soundEffectInstance, _soundEffect.audioClip.length));
        }



        private IEnumerator DisableSoundEffect(SoundEffect _soundEffect, float _delayTime) {
            yield return new WaitForSeconds(_delayTime);
            _soundEffect.gameObject.SetActive(false);
        }



        public void SetSoundVolume(int _volume) {
            float muteDecibel = -80f;

            if (_volume == 0) {
                AudioResources.Instance.AudioMixerGroup_Master.audioMixer.SetFloat("soundVolume", muteDecibel);
            } else {
                float decibel = HelperUtilities.ConvertLinearToDecibel(_volume);
                AudioResources.Instance.AudioMixerGroup_Master.audioMixer.SetFloat("soundVolume", decibel);
            }
        }
    }
}

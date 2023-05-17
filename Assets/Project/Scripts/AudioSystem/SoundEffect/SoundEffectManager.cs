using System.Collections;
using UnityEngine;

namespace DIM.AudioSystem {
    [DisallowMultipleComponent]
    public class SoundEffectManager : SingletonMonobehaviour<SoundEffectManager> {
        public int volume = 8;

        // ===================================================================

        private void Start() {
            if (PlayerPrefs.HasKey("soundVolume"))
                volume = PlayerPrefs.GetInt("soundVolume");

            SetSoundVolume(volume);
        }



        private void OnDisable() {
            PlayerPrefs.SetInt("soundVolume", volume);
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



        public void IncreaseSoundVolume() {
            int maxSoundVolume = 20;

            if (volume >= maxSoundVolume)
                return;

            volume++;

            SetSoundVolume(volume);
        }



        public void DecreaseSoundVolume() {
            int minSoundVolume = 0;

            if (volume <= minSoundVolume)
                return;

            volume--;

            SetSoundVolume(volume);
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

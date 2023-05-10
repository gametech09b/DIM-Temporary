using System.Collections;
using UnityEngine;

namespace DungeonGunner {
    public class MusicManager : SingletonMonobehaviour<MusicManager> {
        private AudioSource audioSource;
        private AudioClip currentAudioClip;
        private Coroutine fadeOutCoroutine;
        private Coroutine fadeInCoroutine;
        public int volume = 10;



        protected override void Awake() {
            base.Awake();

            audioSource = GetComponent<AudioSource>();

            AudioResources.Instance.MusicSnapshot_Off.TransitionTo(0f);
        }



        private void Start() {
            if (PlayerPrefs.HasKey("musicVolume"))
                volume = PlayerPrefs.GetInt("musicVolume");

            SetMusicVolume(volume);
        }



        private void OnDisable() {
            PlayerPrefs.SetInt("musicVolume", volume);
        }



        public void PlayMusic(MusicTrackSO _musicTrack, float _fadeOutTime = Settings.MusicTrackFadeOutTime, float _fadeInTime = Settings.MusicTrackFadeInTime) {
            StartCoroutine(PlayMusicCoroutine(_musicTrack, _fadeOutTime, _fadeInTime));
        }



        private IEnumerator PlayMusicCoroutine(MusicTrackSO _musicTrack, float _fadeOutTime, float _fadeInTime) {
            if (fadeOutCoroutine != null)
                StopCoroutine(fadeOutCoroutine);

            if (fadeInCoroutine != null)
                StopCoroutine(fadeInCoroutine);

            if (_musicTrack.audioClip != currentAudioClip) {
                currentAudioClip = _musicTrack.audioClip;

                fadeOutCoroutine = StartCoroutine(FadeOutCoroutine(_fadeOutTime));
                yield return fadeOutCoroutine;

                fadeInCoroutine = StartCoroutine(FadeInCoroutine(_musicTrack, _fadeInTime));
                yield return fadeInCoroutine;
            }

            yield return null;
        }



        private IEnumerator FadeOutCoroutine(float _fadeOutTime) {
            AudioResources.Instance.MusicSnapshot_OnLow.TransitionTo(_fadeOutTime);

            yield return new WaitForSeconds(_fadeOutTime);
        }



        private IEnumerator FadeInCoroutine(MusicTrackSO _musicTrack, float _fadeInTime) {
            audioSource.clip = _musicTrack.audioClip;
            audioSource.volume = _musicTrack.volume;
            audioSource.Play();

            AudioResources.Instance.MusicSnapshot_OnFull.TransitionTo(_fadeInTime);

            yield return new WaitForSeconds(_fadeInTime);
        }



        public void IncreaseMusicVolume() {
            int maxMusicVolume = 20;

            if (volume >= maxMusicVolume)
                return;

            volume++;

            SetMusicVolume(volume);
        }



        public void DecreaseMusicVolume() {
            int minMusicVolume = 0;

            if (volume <= minMusicVolume)
                return;

            volume--;

            SetMusicVolume(volume);
        }



        public void SetMusicVolume(int _volume) {
            float muteDecibel = -80f;

            if (_volume == 0) {
                AudioResources.Instance.AudioMixerGroup_Master.audioMixer.SetFloat("musicVolume", muteDecibel);
            } else {
                float decibel = HelperUtilities.ConvertLinearToDecibel(_volume);
                AudioResources.Instance.AudioMixerGroup_Master.audioMixer.SetFloat("musicVolume", decibel);
            }
        }
    }
}

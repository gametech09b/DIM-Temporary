using System.Collections;
using TMPro;
using UnityEngine;

namespace DungeonGunner {
    public class PauseMenuUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI musicVolumeText;
        [SerializeField] private TextMeshProUGUI soundVolumeText;



        private void Start() {
            gameObject.SetActive(false);
        }



        private void OnEnable() {
            Time.timeScale = 0;

            StartCoroutine(InitUICoroutine());
        }



        private void OnDisable() {
            Time.timeScale = 1;
        }



        private IEnumerator InitUICoroutine() {
            yield return null;

            musicVolumeText.SetText(MusicManager.Instance.volume.ToString());
            soundVolumeText.SetText(SoundEffectManager.Instance.volume.ToString());
        }



        public void IncreaseMusicVolume() {
            MusicManager.Instance.IncreaseMusicVolume();
            musicVolumeText.SetText(MusicManager.Instance.volume.ToString());
        }



        public void DecreaseMusicVolume() {
            MusicManager.Instance.DecreaseMusicVolume();
            musicVolumeText.SetText(MusicManager.Instance.volume.ToString());
        }



        public void IncreaseSoundVolume() {
            SoundEffectManager.Instance.IncreaseSoundVolume();
            soundVolumeText.SetText(SoundEffectManager.Instance.volume.ToString());
        }



        public void DecreaseSoundVolume() {
            SoundEffectManager.Instance.DecreaseSoundVolume();
            soundVolumeText.SetText(SoundEffectManager.Instance.volume.ToString());
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckNullValue(this, nameof(musicVolumeText), musicVolumeText);
            HelperUtilities.CheckNullValue(this, nameof(soundVolumeText), soundVolumeText);
        }
#endif
        #endregion
    }
}

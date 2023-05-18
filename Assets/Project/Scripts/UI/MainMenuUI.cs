using UnityEngine;
using UnityEngine.SceneManagement;

using DIM.AudioSystem;

namespace DIM.UI {
    public class MainMenuUI : MonoBehaviour {
        [SerializeField] private GameObject playButton;
        [SerializeField] private GameObject highScoreButton;
        [SerializeField] private GameObject returnToMainMenuButton;
        private bool isHighScoresSceneLoaded = false;

        // ===================================================================

        private void Start() {
            MusicManager.Instance.PlayMusic(AudioResources.Instance.MainMenuMusicTrack, 0f, 2f);

            SceneManager.LoadScene("CharacterSelectorScene", LoadSceneMode.Additive);

            returnToMainMenuButton.SetActive(false);
        }



        public void PlayGame() {
            SceneManager.LoadScene("MainGameScene");
        }



        public void LoadHighScore() {
            playButton.SetActive(false);
            highScoreButton.SetActive(false);
            returnToMainMenuButton.SetActive(true);

            SceneManager.UnloadSceneAsync("CharacterSelectorScene");

            isHighScoresSceneLoaded = true;

            SceneManager.LoadScene("HighScoreScene", LoadSceneMode.Additive);
        }



        public void LoadCharacterSelector() {
            playButton.SetActive(true);
            highScoreButton.SetActive(true);
            returnToMainMenuButton.SetActive(false);

            if (isHighScoresSceneLoaded) {
                SceneManager.UnloadSceneAsync("HighScoreScene");
                isHighScoresSceneLoaded = false;
            }

            SceneManager.LoadScene("CharacterSelectorScene", LoadSceneMode.Additive);
        }
    }
}

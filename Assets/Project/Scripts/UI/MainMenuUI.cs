using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonGunner {
    public class MainMenuUI : MonoBehaviour {
        private void Start() {
            MusicManager.Instance.PlayMusic(AudioResources.Instance.MainMenuMusicTrack, 0f, 2f);
        }



        public void PlayGame() {
            SceneManager.LoadScene("MainGameScene");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    public class HighScoreManager : SingletonMonobehaviour<HighScoreManager> {
        private HighScore highScore = new HighScore();



        protected override void Awake() {
            base.Awake();

            LoadHighScore();
        }



        public void AddScore(Score score) {
            highScore.scoreList.Add(score);
            highScore.scoreList.Sort((x, y) => y.score.CompareTo(x.score));

            if (highScore.scoreList.Count > Settings.ScoreMaxEntries)
                highScore.scoreList.RemoveAt(Settings.ScoreMaxEntries);

            SaveHighScore();
        }



        public void SaveHighScore() {
            string json = JsonUtility.ToJson(highScore);
            PlayerPrefs.SetString("HighScore", json);
        }



        public void LoadHighScore() {
            string json = PlayerPrefs.GetString("HighScore");

            if (json == "")
                return;

            highScore = JsonUtility.FromJson<HighScore>(json);
        }



        public HighScore GetHighScore() {
            return highScore;
        }



        public int GetRank(long _gameScore) {
            if (highScore.scoreList.Count == 0)
                return 1;

            int index = 0;

            foreach (Score score in highScore.scoreList) {
                index++;

                if (_gameScore >= score.score)
                    return index;
            }

            if (highScore.scoreList.Count < Settings.ScoreMaxEntries)
                return index + 1;

            return 0;
        }
    }
}

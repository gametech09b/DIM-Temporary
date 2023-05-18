using UnityEngine;

namespace DIM.ScoreSystem {
    public class HighScoreUI : MonoBehaviour {
        [SerializeField] private Transform contentParentTransform;

        // ===================================================================

        private void Start() {
            DisplayHighScore();
        }



        private void DisplayHighScore() {
            HighScore highScore = HighScoreManager.Instance.GetHighScore();

            int rank = 0;

            foreach (Score score in highScore.scoreList) {
                rank++;

                GameObject scoreGameObject = Instantiate(UIResources.Instance.ScoreGameObjectPrefab, contentParentTransform);
                ScoreGameObject scoreGameObjectScript = scoreGameObject.GetComponent<ScoreGameObject>();

                scoreGameObjectScript.rankTextMP.text = rank.ToString();
                scoreGameObjectScript.nameTextMP.text = score.playerName;
                scoreGameObjectScript.levelTextMP.text = score.levelDescription.ToString();
                scoreGameObjectScript.scoreTextMP.text = score.score.ToString();
            }
        }
    }
}

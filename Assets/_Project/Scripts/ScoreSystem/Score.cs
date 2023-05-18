namespace DIM.ScoreSystem {
    [System.Serializable]
    public class Score {
        public string playerName;
        public string levelDescription;
        public long score;

        // ===================================================================

        public Score(string _playerName, string _levelDescription, long _score) {
            playerName = _playerName;
            levelDescription = _levelDescription;
            score = _score;
        }
    }
}

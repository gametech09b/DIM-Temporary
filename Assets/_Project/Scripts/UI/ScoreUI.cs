using UnityEngine;
using TMPro;

namespace DIM.UI {
    public class ScoreUI : MonoBehaviour {
        private TextMeshProUGUI scoreTextMP;

        // ===================================================================

        private void Awake() {
            scoreTextMP = GetComponentInChildren<TextMeshProUGUI>();
        }



        private void OnEnable() {
            DungeonStaticEvent.OnScoreChanged += DungeonStaticEvent_OnScoreChanged;
        }



        private void OnDisable() {
            DungeonStaticEvent.OnScoreChanged -= DungeonStaticEvent_OnScoreChanged;
        }



        private void DungeonStaticEvent_OnScoreChanged(OnScoreChangedEventArgs _args) {
            scoreTextMP.text = $"SCORE: {_args.score.ToString("###,###0")}\nMULTIPLIER: x{_args.multiplier}";
        }
    }
}

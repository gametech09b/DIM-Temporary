using UnityEngine;

namespace DIM.PlayerSystem {
    [CreateAssetMenu(fileName = "CurrentPlayer_", menuName = "Scriptable Objects/Player/Current Player")]
    public class CurrentPlayerSO : ScriptableObject {
        public PlayerDetailSO playerDetail;
        public string playerName;
    }
}

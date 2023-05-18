using System.Collections.Generic;
using UnityEngine;

namespace DIM.DungeonSystem {
    [CreateAssetMenu(fileName = "RoomNodeTypeListSO_", menuName = "Scriptable Objects/Dungeon/Room Node Type List")]
    public class RoomNodeTypeListSO : ScriptableObject {
        [Space(10)]
        [Header("Room Node Type List")]


        [Tooltip("This list should be populated with all the RoomNodeTypeSO for the game - it is used instead of an enum")]
        public List<RoomNodeTypeSO> list;

        // ===================================================================

        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckEnumerableValue(this, nameof(list), list);
        }
#endif
        #endregion
    }
}

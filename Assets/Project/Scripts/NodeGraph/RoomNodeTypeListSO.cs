using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [CreateAssetMenu(fileName = "RoomNodeTypeListSO_", menuName = "Scriptable Objects/Dungeon/Room Node Type List")]
    public class RoomNodeTypeListSO : ScriptableObject
    {
        #region Header RoomNodeTypeList
        [Space(10)]
        [Header("Room Node Type List")]
        #endregion



        #region Tooltip
        [Tooltip("This list should be populated with all the RoomNodeTypeSO for the game - it is used instead of an enum")]
        #endregion
        public List<RoomNodeTypeSO> list;



        #region Validation
        #region Editor
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(list), list);
        }
#endif
        #endregion
        #endregion
    }
}

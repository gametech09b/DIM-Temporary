using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [CreateAssetMenu(fileName = "RoomNodeTypeListSO_", menuName = "Scriptable Objects/Dungeon/Room Node Type List")]
    public class RoomNodeTypeListSO : ScriptableObject
    {
        #region Header ROOM NODE TYPE LIST
        [Space(10)]
        [Header("Room Node Type List")]
        #endregion



        #region Tooltip
        [Tooltip("This list should be populated with all the RoomNodeTypeSO for the game - it is used instead of an enum")]
        #endregion
        public List<RoomNodeTypeSO> roomNodeTypeList;



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomNodeTypeList), roomNodeTypeList);
        }
#endif
        #endregion
    }
}

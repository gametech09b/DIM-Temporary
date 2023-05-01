using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class RoomActivationHandler : MonoBehaviour
    {
        [SerializeField] private Camera minimapCamera;



        private void Start()
        {
            InvokeRepeating(nameof(EnableRooms), 0.5f, 0.75f);
        }



        private void EnableRooms()
        {
            foreach (KeyValuePair<string, Room> roomDictionaryKVP in DungeonBuilder.Instance.roomDictionary)
            {
                Room room = roomDictionaryKVP.Value;

                HelperUtilities.CameraWorldPositionBounds(minimapCamera, out Vector2Int minimapLowerBounds, out Vector2Int minimapUpperBounds);

                if ((room.lowerBounds.x <= minimapUpperBounds.x && room.lowerBounds.y <= minimapUpperBounds.y)
                && (room.upperBounds.x >= minimapLowerBounds.x && room.upperBounds.y >= minimapLowerBounds.y))
                    room.roomGameObject.gameObject.SetActive(true);
                else
                    room.roomGameObject.gameObject.SetActive(false);
            }
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckNullValue(this, nameof(minimapCamera), minimapCamera);
        }
#endif
        #endregion
    }
}

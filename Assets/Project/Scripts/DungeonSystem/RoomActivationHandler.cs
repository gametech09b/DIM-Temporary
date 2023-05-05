using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class RoomActivationHandler : MonoBehaviour
    {
        [SerializeField] private Camera minimapCamera;
        private Camera mainCamera;



        private void Start()
        {
            mainCamera = Camera.main;

            InvokeRepeating(nameof(EnableRooms), 0.5f, 0.75f);
        }



        private void EnableRooms()
        {
            HelperUtilities.CameraWorldPositionBounds(minimapCamera, out Vector2Int minimapCameraLowerBounds, out Vector2Int minimapCameraUpperBounds);
            HelperUtilities.CameraWorldPositionBounds(mainCamera, out Vector2Int mainCameraLowerBounds, out Vector2Int mainCameraUpperBounds);

            foreach (KeyValuePair<string, Room> roomDictionaryKVP in DungeonBuilder.Instance.roomDictionary)
            {
                Room room = roomDictionaryKVP.Value;

                if ((room.lowerBounds.x <= minimapCameraUpperBounds.x && room.lowerBounds.y <= minimapCameraUpperBounds.y)
                && (room.upperBounds.x >= minimapCameraLowerBounds.x && room.upperBounds.y >= minimapCameraLowerBounds.y))
                {
                    room.roomGameObject.gameObject.SetActive(true);

                    if ((room.lowerBounds.x <= mainCameraUpperBounds.x && room.lowerBounds.y <= mainCameraUpperBounds.y)
                    && (room.upperBounds.x >= mainCameraLowerBounds.x && room.upperBounds.y >= mainCameraLowerBounds.y))
                        room.roomGameObject.ActivateEnvironment();
                    else
                        room.roomGameObject.DeactivateEnvironment();
                }
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

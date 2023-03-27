using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    public static class HelperUtilities
    {
        /// <summary>
        /// Empty string validation
        /// </summary>
        /// <param name="thisObject"></param>
        /// <param name="fieldName"></param>
        /// <param name="stringToCheck"></param>
        /// <returns></returns>
        public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
        {
            if (stringToCheck == "")
            {
                Debug.Log($"{fieldName} is empty and must contain a value in object {thisObject.name.ToString()}");
                return true;
            }
            return false;
        }



        /// <summary>
        /// Null value validation
        /// </summary>
        /// <param name="thisObject"></param>
        /// <param name="fieldName"></param>
        /// <param name="objectToCheck"></param>
        /// <returns></returns>
        public static bool ValidateCheckNullValue(Object thisObject, string fieldName, Object objectToCheck)
        {
            if (objectToCheck == null)
            {
                Debug.Log($"{fieldName} is null in object {thisObject.name.ToString()}");
                return true;
            }
            return false;
        }



        /// <summary>
        /// Empty or contains null values IEnumerable validation
        /// </summary>
        /// <param name="thisObject"></param>
        /// <param name="fieldName"></param>
        /// <param name="enumerableToCheck"></param>
        /// <returns></returns>
        public static bool ValidateCheckEnumerableValues(Object thisObject, string fieldName, IEnumerable enumerableToCheck)
        {
            bool error = false;
            int count = 0;

            if (enumerableToCheck == null)
            {
                Debug.Log($"{fieldName} is null in object {thisObject.name.ToString()}");
                return true;
            }

            foreach (var item in enumerableToCheck)
            {
                if (item == null)
                {
                    Debug.Log($"{fieldName} contains a null value in object {thisObject.name.ToString()}");
                    error = true;
                }
                else
                {
                    count++;
                }
            }

            if (count == 0)
            {
                Debug.Log($"{fieldName} contains no values in object {thisObject.name.ToString()}");
                error = true;
            }

            return error;
        }



        public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, int valueToCheck, bool isZeroAllowed = false)
        {
            if (valueToCheck < 0)
            {
                Debug.Log($"{fieldName} is negative in object {thisObject.name.ToString()}");
                return true;
            }
            else if (valueToCheck == 0 && !isZeroAllowed)
            {
                Debug.Log($"{fieldName} is zero in object {thisObject.name.ToString()}");
                return true;
            }
            return false;
        }



        public static Vector3 GetNearestSpawnPoint(Vector3 position)
        {
            Room currentRoom = GameManager.Instance.GetCurrentRoom();

            Grid grid = currentRoom.roomGameObject.grid;

            Vector3 nearestSpawnPosition = new Vector3(10000f, 10000f, 0);

            foreach (Vector2Int spawnPositionGrid in currentRoom.spawnPositionArray)
            {
                Vector3 spawnPositionWorld = grid.CellToWorld((Vector3Int)spawnPositionGrid);

                if (Vector3.Distance(spawnPositionWorld, position) < Vector3.Distance(nearestSpawnPosition, position))
                {
                    nearestSpawnPosition = spawnPositionWorld;
                }
            }

            return nearestSpawnPosition;
        }
    }
}

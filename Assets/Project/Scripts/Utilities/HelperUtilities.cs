using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    public static class HelperUtilities
    {
        public static Camera mainCamera;

        public static Vector3 GetMouseWorldPosition()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            Vector3 mouseScreenPositon = Input.mousePosition;
            mouseScreenPositon.x = Mathf.Clamp(mouseScreenPositon.x, 0f, Screen.width);
            mouseScreenPositon.y = Mathf.Clamp(mouseScreenPositon.y, 0f, Screen.height);

            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPositon);
            mouseWorldPosition.z = 0;

            return mouseWorldPosition;
        }



        public static float GetAngleFromVector(Vector3 vector)
        {
            float radian = Mathf.Atan2(vector.y, vector.x);
            float angleDegree = radian * Mathf.Rad2Deg;

            return angleDegree;
        }



        public static Vector3 GetDirectionVectorFromAngle(float angleDegree)
        {
            float radian = angleDegree * Mathf.Deg2Rad;
            Vector3 directionVector = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0);

            return directionVector;
        }



        public static Direction GetDirectionFromAngle(float angleDegree)
        {
            Direction direction;

            if (angleDegree >= 22f && angleDegree <= 67f)
            {
                direction = Direction.UP_RIGHT;
            }
            else if (angleDegree > 67f && angleDegree <= 112f)
            {
                direction = Direction.UP;
            }
            else if (angleDegree > 112f && angleDegree <= 158f)
            {
                direction = Direction.UP_LEFT;
            }
            else if ((angleDegree <= 180f && angleDegree > 158f) || (angleDegree > -180f && angleDegree <= -135f))
            {
                direction = Direction.LEFT;
            }
            else if (angleDegree > -135f && angleDegree <= -45f)
            {
                direction = Direction.DOWN;
            }
            else if ((angleDegree > -45f && angleDegree <= 0f) || (angleDegree > 0 && angleDegree < 22f))
            {
                direction = Direction.RIGHT;
            }
            else
            {
                direction = Direction.NONE;
            }

            return direction;
        }



        public static float ConvertLinearToDecibel(int linear)
        {
            float linearScaleRange = 20f;

            return Mathf.Log10(linear / linearScaleRange) * 20f;
        }



        /// <summary>
        /// Empty string validation
        /// </summary>
        /// <param name="thisObject"></param>
        /// /// <param name="fieldName"></param>
        /// <param name="stringToCheck"></param>
        /// <returns></returns>
        public static bool CheckEmptyString(UnityEngine.Object thisObject, string fieldName, string stringToCheck)
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
        public static bool CheckNullValue(UnityEngine.Object thisObject, string fieldName, UnityEngine.Object objectToCheck)
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
        public static bool CheckEnumerableValue(UnityEngine.Object thisObject, string fieldName, IEnumerable enumerableToCheck)
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



        public static bool CheckPositiveValue(UnityEngine.Object thisObject, string fieldName, float value, bool isZeroAllowed = false)
        {
            if (value < 0)
            {
                Debug.Log($"{fieldName} is negative in object {thisObject.name.ToString()}");
                return true;
            }
            else if (value == 0 && !isZeroAllowed)
            {
                Debug.Log($"{fieldName} is zero in object {thisObject.name.ToString()}");
                return true;
            }
            return false;
        }



        public static bool CheckPositiveRange(UnityEngine.Object thisObject, string fieldNameMinimum, string fieldNameMaximum, float minimumValue, float maximumValue, bool isZeroAllowed = false)
        {
            bool error = false;

            if (minimumValue > maximumValue)
            {
                Debug.Log($"{fieldNameMinimum} is greater than {fieldNameMaximum} in object {thisObject.name.ToString()}");
                error = true;
            }

            if (CheckPositiveValue(thisObject, fieldNameMinimum, minimumValue, isZeroAllowed))
                error = true;

            if (CheckPositiveValue(thisObject, fieldNameMaximum, maximumValue, isZeroAllowed))
                error = true;

            return error;
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

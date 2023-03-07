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

            foreach (var item in enumerableToCheck)
            {
                if (item == null)
                {
                    Debug.Log($"{fieldName} contains a null value in object {thisObject.name.ToString()}");
                    error = true;
                }
                else {
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
    }
}

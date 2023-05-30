using System.Collections;
using UnityEngine;

using DIM.DungeonSystem;

namespace DIM {
    public static class HelperUtilities {
        public static Camera mainCamera;

        // ===================================================================

        public static Vector3 GetMouseWorldPosition() {
            if (mainCamera == null) {
                mainCamera = Camera.main;
            }

            Vector3 mouseScreenPositon = Input.mousePosition;
            mouseScreenPositon.x = Mathf.Clamp(mouseScreenPositon.x, 0f, Screen.width);
            mouseScreenPositon.y = Mathf.Clamp(mouseScreenPositon.y, 0f, Screen.height);

            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPositon);
            mouseWorldPosition.z = 0;

            return mouseWorldPosition;
        }



        public static float GetAngleFromVector(Vector3 _vector) {
            float radian = Mathf.Atan2(_vector.y, _vector.x);
            float angleDegree = radian * Mathf.Rad2Deg;

            return angleDegree;
        }



        public static Vector3 GetDirectionVectorFromAngle(float _angleDegree) {
            float radian = _angleDegree * Mathf.Deg2Rad;
            Vector3 directionVector = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0);

            return directionVector;
        }



        public static Direction GetDirectionFromAngle(float _angleDegree) {
            Direction direction;

           /* if (_angleDegree >= 22f
            && _angleDegree <= 89.5f)
                direction = Direction.UP_RIGHT;*/ 

            /*else if (_angleDegree > 67f
                 && _angleDegree <= 112f)
                direction = Direction.UP;*/

            /*else if (_angleDegree > 89.5f
                 && _angleDegree <= 158f)
                direction = Direction.UP_LEFT;*/
                
            // if ((_angleDegree > 90f && _angleDegree < 180f) || (_angleDegree < -1f && _angleDegree > 90f))
            //     direction = Direction.LEFT;

             if ((_angleDegree <= 180f && _angleDegree > 90f)
                 || (_angleDegree > -180f && _angleDegree <= -90f))
                direction = Direction.LEFT;

            /*else if (_angleDegree > -135f
                 && _angleDegree <= -45f)
                direction = Direction.DOWN;*/
            
            // else if ((_angleDegree > 1f && _angleDegree < 90f)|| (_angleDegree < -90f && _angleDegree > -180))
            //     direction = Direction.RIGHT;

            else if ((_angleDegree < -90f && _angleDegree <= 0f)
                 || (_angleDegree > 0 && _angleDegree > 90f))
                direction = Direction.RIGHT;

            else
                direction = Direction.RIGHT;

            return direction;
        }
            // public static Direction GetDirectionFromAngle(float _angleDegree) {
            //     Direction direction;

            //     if (_angleDegree >= 22f
            //     && _angleDegree <= 67f)
            //         direction = Direction.UP_RIGHT;

            //     else if (_angleDegree > 67f
            //         && _angleDegree <= 112f)
            //         direction = Direction.UP;

            //     else if (_angleDegree > 112f
            //         && _angleDegree <= 158f)
            //         direction = Direction.UP_LEFT;

            //     else if ((_angleDegree <= 180f && _angleDegree > 158f)
            //         || (_angleDegree > -180f && _angleDegree <= -135f))
            //         direction = Direction.LEFT;

            //     else if (_angleDegree > -135f
            //         && _angleDegree <= -45f)
            //         direction = Direction.DOWN;

            //     else if ((_angleDegree > -45f && _angleDegree <= 0f)
            //         || (_angleDegree > 0 && _angleDegree < 22f))
            //         direction = Direction.RIGHT;

            //     else
            //         direction = Direction.NONE;

            //     return direction;
            // }



        public static float ConvertLinearToDecibel(int _linear) {
            float linearScaleRange = 20f;

            return Mathf.Log10(_linear / linearScaleRange) * 20f;
        }



        /// <summary>
        /// Empty string validation
        /// </summary>
        /// <param name="_thisObject"></param>
        /// /// <param name="_fieldName"></param>
        /// <param name="_stringToCheck"></param>
        /// <returns></returns>
        public static bool CheckEmptyString(UnityEngine.Object _thisObject, string _fieldName, string _stringToCheck) {
            if (_stringToCheck == "") {
                Debug.Log($"{_fieldName} is empty and must contain a value in object {_thisObject.name.ToString()}");
                return true;
            }
            return false;
        }



        /// <summary>
        /// Null value validation
        /// </summary>
        /// <param name="_thisObject"></param>
        /// <param name="_fieldName"></param>
        /// <param name="_objectToCheck"></param>
        /// <returns></returns>
        public static bool CheckNullValue(UnityEngine.Object _thisObject, string _fieldName, UnityEngine.Object _objectToCheck) {
            if (_objectToCheck == null) {
                Debug.Log($"{_fieldName} is null in object {_thisObject.name.ToString()}");
                return true;
            }
            return false;
        }



        /// <summary>
        /// Empty or contains null values IEnumerable validation
        /// </summary>
        /// <param name="_thisObject"></param>
        /// <param name="_fieldName"></param>
        /// <param name="_enumerableToCheck"></param>
        /// <returns></returns>
        public static bool CheckEnumerableValue(UnityEngine.Object _thisObject, string _fieldName, IEnumerable _enumerableToCheck) {
            bool error = false;
            int count = 0;

            if (_enumerableToCheck == null) {
                Debug.Log($"{_fieldName} is null in object {_thisObject.name.ToString()}");
                return true;
            }

            foreach (var item in _enumerableToCheck) {
                if (item == null) {
                    Debug.Log($"{_fieldName} contains a null value in object {_thisObject.name.ToString()}");
                    error = true;
                } else {
                    count++;
                }
            }

            if (count == 0) {
                Debug.Log($"{_fieldName} contains no values in object {_thisObject.name.ToString()}");
                error = true;
            }

            return error;
        }



        public static bool CheckPositiveValue(UnityEngine.Object _thisObject, string _fieldName, float _value, bool _isZeroAllowed = false) {
            if (_value < 0) {
                Debug.Log($"{_fieldName} is negative in object {_thisObject.name.ToString()}");
                return true;
            } else if (_value == 0 && !_isZeroAllowed) {
                Debug.Log($"{_fieldName} is zero in object {_thisObject.name.ToString()}");
                return true;
            }
            return false;
        }



        public static bool CheckPositiveRange(UnityEngine.Object _thisObject, string _fieldNameMinimum, string _fieldNameMaximum, float _minimumValue, float _maximumValue, bool _isZeroAllowed = false) {
            bool error = false;

            if (_minimumValue > _maximumValue) {
                Debug.Log($"{_fieldNameMinimum} is greater than {_fieldNameMaximum} in object {_thisObject.name.ToString()}");
                error = true;
            }

            if (CheckPositiveValue(_thisObject, _fieldNameMinimum, _minimumValue, _isZeroAllowed))
                error = true;

            if (CheckPositiveValue(_thisObject, _fieldNameMaximum, _maximumValue, _isZeroAllowed))
                error = true;

            return error;
        }



        public static bool CheckPositiveRange(UnityEngine.Object _thisObject, string _fieldNameMinimum, string _fieldNameMaximum, int _minimumValue, int _maximumValue, bool _isZeroAllowed = false) {
            bool error = false;

            if (_minimumValue > _maximumValue) {
                Debug.Log($"{_fieldNameMinimum} is greater than {_fieldNameMaximum} in object {_thisObject.name.ToString()}");
                error = true;
            }

            if (CheckPositiveValue(_thisObject, _fieldNameMinimum, _minimumValue, _isZeroAllowed))
                error = true;

            if (CheckPositiveValue(_thisObject, _fieldNameMaximum, _maximumValue, _isZeroAllowed))
                error = true;

            return error;
        }



        public static Vector3 GetNearestSpawnPoint(Vector3 _position) {
            Room currentRoom = GameManager.Instance.GetCurrentRoom();

            Grid grid = currentRoom.roomGameObject.grid;

            Vector3 nearestSpawnPosition = new Vector3(10000f, 10000f, 0);

            foreach (Vector2Int spawnPositionGrid in currentRoom.spawnPositionArray) {
                Vector3 spawnPositionWorld = grid.CellToWorld((Vector3Int)spawnPositionGrid);

                if (Vector3.Distance(spawnPositionWorld, _position) < Vector3.Distance(nearestSpawnPosition, _position)) {
                    nearestSpawnPosition = spawnPositionWorld;
                }
            }

            return nearestSpawnPosition;
        }



        public static void CameraWorldPositionBounds(Camera _camera, out Vector2Int _lowerBounds, out Vector2Int _upperBounds) {
            Vector3 viewportBottomLeft = _camera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f));
            Vector3 viewportTopRight = _camera.ViewportToWorldPoint(new Vector3(1f, 1f, 0f));

            _lowerBounds = new Vector2Int((int)viewportBottomLeft.x, (int)viewportBottomLeft.y);
            _upperBounds = new Vector2Int((int)viewportTopRight.x, (int)viewportTopRight.y);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [CreateAssetMenu(fileName = "MovementDetail_", menuName = "Scriptable Objects/Movement/Movement Detail")]
    public class MovementDetailSO : ScriptableObject {
        [Space(10)]
        [Header("Movement Detail")]


        [Tooltip("The minimum speed of the player")]
        [SerializeField] private float minSpeed = 8f;

        [Tooltip("The maximum speed of the player")]
        [SerializeField] private float maxSpeed = 8f;



        public float GetMoveSpeed() {
            if (minSpeed == maxSpeed)
                return minSpeed;

            return Random.Range(minSpeed, maxSpeed);
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.ValidateCheckPositiveRange(this, nameof(minSpeed), nameof(maxSpeed), minSpeed, maxSpeed);
        }
#endif
        #endregion
    }
}

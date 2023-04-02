using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [CreateAssetMenu(fileName = "MovementDetail_", menuName = "Scriptable Objects/Movement/Movement Detail")]
    public class MovementDetailSO : ScriptableObject {
        [Space(10)]
        [Header("Movement Detail")]


        [Tooltip("The minimum speed")]
        [SerializeField] private float minSpeed = 8f;

        [Tooltip("The maximum speed")]
        [SerializeField] private float maxSpeed = 8f;



        [Space(10)]
        [Header("Player Only")]


        [Tooltip("The speed of the player when dash")]
        public float dashSpeed;

        [Tooltip("The dash distance of the player")]
        public float dashDistance;

        [Tooltip("The dash cooldown time of the player")]
        public float dashCooldownTime;



        public float GetMoveSpeed() {
            if (minSpeed == maxSpeed)
                return minSpeed;

            return Random.Range(minSpeed, maxSpeed);
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.ValidateCheckPositiveRange(this, nameof(minSpeed), nameof(maxSpeed), minSpeed, maxSpeed);

            if (dashSpeed != 0 || dashDistance != 0 || dashCooldownTime != 0) {
                HelperUtilities.ValidateCheckPositiveValue(this, nameof(dashSpeed), dashSpeed);
                HelperUtilities.ValidateCheckPositiveValue(this, nameof(dashDistance), dashDistance);
                HelperUtilities.ValidateCheckPositiveValue(this, nameof(dashCooldownTime), dashCooldownTime);
            }
#endif
            #endregion
        }
    }
}

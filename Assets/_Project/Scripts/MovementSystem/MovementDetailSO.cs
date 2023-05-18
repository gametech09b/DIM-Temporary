using UnityEngine;

namespace DIM.MovementSystem {
    [CreateAssetMenu(fileName = "MovementDetail_", menuName = "Scriptable Objects/Movement/Movement Detail")]
    public class MovementDetailSO : ScriptableObject {
        [Space(10)]
        [Header("Movement Detail")]

        [SerializeField] private float minSpeed = 8f;
        [SerializeField] private float maxSpeed = 8f;


        [Space(10)]

        public float dashSpeed;
        public float dashDistance;
        public float dashCooldownTime;

        // ===================================================================

        public float GetMoveSpeed() {
            if (minSpeed == maxSpeed)
                return minSpeed;

            return Random.Range(minSpeed, maxSpeed);
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckPositiveRange(this, nameof(minSpeed), nameof(maxSpeed), minSpeed, maxSpeed);

            if (dashSpeed != 0 || dashDistance != 0 || dashCooldownTime != 0) {
                HelperUtilities.CheckPositiveValue(this, nameof(dashSpeed), dashSpeed);
                HelperUtilities.CheckPositiveValue(this, nameof(dashDistance), dashDistance);
                HelperUtilities.CheckPositiveValue(this, nameof(dashCooldownTime), dashCooldownTime);
            }
#endif
            #endregion
        }
    }
}

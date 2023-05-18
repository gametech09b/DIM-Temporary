using UnityEngine;

using DIM.CombatSystem;

namespace DIM.EnemySystem {
    [CreateAssetMenu(fileName = "EnemyDetail_", menuName = "Scriptable Objects/Enemy/Enemy Detail")]
    public class EnemyDetailSO : ScriptableObject {
        [Space(10)]
        [Header("Base Enemy Detail")]

        public string enemyName;
        public GameObject prefab;

        public float chaseDistance = 50f;


        [Space(10)]
        [Header("Enemy Material")]

        public Material standardMaterial;


        [Space(10)]
        [Header("Enemy Materialize Setting")]

        public float materializeDuration;
        public Shader materializeShader;
        [ColorUsage(true, true)] public Color materializeColor;


        [Space(10)]
        [Header("Enemy Weapon Setting")]

        public WeaponDetailSO weaponDetail;
        public float minFireInterval = 0.1f;
        public float maxFireInterval = 1f;
        public float minFireDuration = 1f;
        public float maxFireDuration = 2f;
        public bool isRequireTargetOnSight = true;


        [Space(10)]
        [Header("Enemy Health")]

        public bool isHealthBarEnabled = false;
        public EnemyHealthDetail[] enemyHealthDetailArray;
        public bool isImmuneAfterHit = false;
        public float immuneDuration = 1f;



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckEmptyString(this, nameof(enemyName), enemyName);
            HelperUtilities.CheckNullValue(this, nameof(prefab), prefab);
            HelperUtilities.CheckPositiveValue(this, nameof(chaseDistance), chaseDistance);

            HelperUtilities.CheckNullValue(this, nameof(standardMaterial), standardMaterial);

            HelperUtilities.CheckPositiveValue(this, nameof(materializeDuration), materializeDuration);
            HelperUtilities.CheckNullValue(this, nameof(materializeShader), materializeShader);

            HelperUtilities.CheckPositiveValue(this, nameof(minFireInterval), minFireInterval);
            HelperUtilities.CheckPositiveValue(this, nameof(maxFireInterval), maxFireInterval);

            HelperUtilities.CheckEnumerableValue(this, nameof(enemyHealthDetailArray), enemyHealthDetailArray);
            if (isImmuneAfterHit)
                HelperUtilities.CheckPositiveValue(this, nameof(immuneDuration), immuneDuration);
        }
#endif
        #endregion
    }
}

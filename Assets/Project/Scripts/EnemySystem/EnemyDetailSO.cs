using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [CreateAssetMenu(fileName = "EnemyDetail_", menuName = "Scriptable Objects/Enemy/Enemy Detail")]
    public class EnemyDetailSO : ScriptableObject
    {
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


        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckEmptyString(this, nameof(enemyName), enemyName);
            HelperUtilities.CheckNullValue(this, nameof(prefab), prefab);
            HelperUtilities.CheckPositiveValue(this, nameof(chaseDistance), chaseDistance);

            HelperUtilities.CheckNullValue(this, nameof(standardMaterial), standardMaterial);

            HelperUtilities.CheckPositiveValue(this, nameof(materializeDuration), materializeDuration);
            HelperUtilities.CheckNullValue(this, nameof(materializeShader), materializeShader);
        }
#endif
        #endregion
    }
}

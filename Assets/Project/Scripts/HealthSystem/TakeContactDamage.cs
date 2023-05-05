using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Health))]
    #endregion
    public class TakeContactDamage : MonoBehaviour
    {
        [SerializeField] private int damageAmount;

        private Health health;



        private void Awake()
        {
            health = GetComponent<Health>();
        }



        public void TakeDamage(int _damageAmount = 0)
        {
            if (damageAmount > 0)
                _damageAmount = damageAmount;

            health.TakeDamage(_damageAmount);
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckPositiveValue(this, nameof(damageAmount), damageAmount, true);
        }
#endif
        #endregion
    }
}

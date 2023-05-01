using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(HealthEvent))]
    #endregion
    public class Health : MonoBehaviour
    {
        private int startingAmount;
        private int currentAmount;
        private HealthEvent healthEvent;

        [HideInInspector] public bool isDamageable = true;



        private void Awake()
        {
            healthEvent = GetComponent<HealthEvent>();
        }



        private void Start()
        {
            CallOnHealthChange(0);
        }



        private void CallOnHealthChange(int _damageAmount)
        {
            healthEvent.CallOnHealthChange(GetPercent(), currentAmount, _damageAmount);
        }



        public void TakeDamage(int _damageAmount)
        {
            if (isDamageable)
            {
                currentAmount -= _damageAmount;
                CallOnHealthChange(_damageAmount);
            }
        }



        public void SetStartingAmount(int _amount)
        {
            startingAmount = _amount;
            currentAmount = _amount;
        }



        public void SetCurrentAmount(int _amount)
        {
            currentAmount = _amount;
        }



        public int GetStartingAmount()
        {
            return startingAmount;
        }



        public int GetCurrentAmount()
        {
            return currentAmount;
        }



        private float GetPercent()
        {
            return (float)currentAmount / (float)startingAmount;
        }
    }
}

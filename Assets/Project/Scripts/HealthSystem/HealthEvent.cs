using System;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class HealthEvent : MonoBehaviour
    {
        public event Action<HealthEvent, OnHealthChangedEventArgs> OnHealthChanged;

        public void CallOnHealthChanged(float _healthPercent, int _healthAmount, int _damageAmount)
        {
            OnHealthChanged?.Invoke(
                this,
                new OnHealthChangedEventArgs
                {
                    healthPercent = _healthPercent,
                    healthAmount = _healthAmount,
                    damageAmount = _damageAmount
                });
        }
    }



    public class OnHealthChangedEventArgs : EventArgs
    {
        public float healthPercent;
        public int healthAmount;
        public int damageAmount;
    }
}

using System;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class HealthEvent : MonoBehaviour
    {
        public event Action<HealthEvent, OnHealthChangeEventArgs> OnHealthChange;

        public void CallOnHealthChange(float _healthPercent, int _healthAmount, int _damageAmount)
        {
            OnHealthChange?.Invoke(
                this,
                new OnHealthChangeEventArgs
                {
                    healthPercent = _healthPercent,
                    healthAmount = _healthAmount,
                    damageAmount = _damageAmount
                });
        }
    }



    public class OnHealthChangeEventArgs : EventArgs
    {
        public float healthPercent;
        public int healthAmount;
        public int damageAmount;
    }
}

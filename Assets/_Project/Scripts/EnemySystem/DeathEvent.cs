using System;
using UnityEngine;

namespace DIM.EnemySystem {
    [DisallowMultipleComponent]
    public class DeathEvent : MonoBehaviour {
        public event Action<DeathEvent> OnDeath;

        public void CallOnDeathEvent() {
            OnDeath?.Invoke(this);
        }
    }
}

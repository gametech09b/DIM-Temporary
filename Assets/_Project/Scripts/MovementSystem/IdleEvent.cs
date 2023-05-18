using System;
using UnityEngine;

namespace DIM.MovementSystem {
    [DisallowMultipleComponent]
    public class IdleEvent : MonoBehaviour {
        public event Action<IdleEvent> OnIdle;

        public void CallOnIdleEvent() {
            OnIdle?.Invoke(this);
        }
    }
}

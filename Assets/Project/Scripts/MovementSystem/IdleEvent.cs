using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class IdleEvent : MonoBehaviour
    {
        public event Action<IdleEvent> OnIdle;

        public void CallOnIdleEvent()
        {
            OnIdle?.Invoke(this);
        }
    }
}

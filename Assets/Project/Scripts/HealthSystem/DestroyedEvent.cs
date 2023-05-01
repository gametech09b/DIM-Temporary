using System;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class DestroyedEvent : MonoBehaviour
    {
        public event Action<DestroyedEvent> OnDestroyed;

        public void CallOnDestroyed()
        {
            OnDestroyed?.Invoke(this);
        }
    }
}

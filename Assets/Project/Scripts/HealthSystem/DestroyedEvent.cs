using System;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class DestroyedEvent : MonoBehaviour
    {
        public event Action<DestroyedEvent, OnDestroyedEventArgs> OnDestroyed;

        public void CallOnDestroyed(bool isPlayer)
        {
            OnDestroyed?.Invoke(
                this,
                new OnDestroyedEventArgs()
                {
                    isPlayer = isPlayer
                });
        }
    }



    public class OnDestroyedEventArgs : EventArgs
    {
        public bool isPlayer;
    }
}

using System;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class DestroyedEvent : MonoBehaviour
    {
        public event Action<DestroyedEvent, OnDestroyedEventArgs> OnDestroyed;

        public void CallOnDestroyed(bool _isPlayer, int _point)
        {
            OnDestroyed?.Invoke(
                this,
                new OnDestroyedEventArgs()
                {
                    isPlayer = _isPlayer,
                    point = _point
                });
        }
    }



    public class OnDestroyedEventArgs : EventArgs
    {
        public bool isPlayer;
        public int point;
    }
}

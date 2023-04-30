using System;

namespace DungeonGunner
{
    public static class DungeonStaticEvent
    {
        public static event Action<OnRoomChangeEventArgs> OnRoomChange;
        public static void CallOnRoomChange(Room _room)
        {
            OnRoomChange?.Invoke(new OnRoomChangeEventArgs()
            {
                room = _room
            });
        }
    }



    public class OnRoomChangeEventArgs : EventArgs
    {
        public Room room;
    }
}

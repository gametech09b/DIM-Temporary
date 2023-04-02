using System;

namespace DungeonGunner {
    public static class DungeonStaticEvent {
        public static event Action<OnRoomChangeEventArgs> OnRoomChange;
        public static void CallOnRoomChange(Room room) {
            OnRoomChange?.Invoke(new OnRoomChangeEventArgs() {
                room = room
            });
        }
    }



    public class OnRoomChangeEventArgs : EventArgs {
        public Room room;
    }
}

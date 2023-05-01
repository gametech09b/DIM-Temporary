using System;

namespace DungeonGunner
{
    public static class DungeonStaticEvent
    {
        public static event Action<OnRoomChangedEventArgs> OnRoomChanged;
        public static void CallOnRoomChanged(Room _room)
        {
            OnRoomChanged?.Invoke(new OnRoomChangedEventArgs()
            {
                room = _room
            });
        }



        public static event Action<OnRoomEnemiesDefeatedEventArgs> OnRoomEnemiesDefeated;
        public static void CallOnRoomEnemiesDefeated(Room _room)
        {
            OnRoomEnemiesDefeated?.Invoke(new OnRoomEnemiesDefeatedEventArgs()
            {
                room = _room
            });
        }



        public static event Action<OnPointScoredEventArgs> OnPointScored;
        public static void CallOnPointScored(int _point)
        {
            OnPointScored?.Invoke(new OnPointScoredEventArgs()
            {
                point = _point
            });
        }



        public static event Action<OnScoreChangedEventArgs> OnScoreChanged;
        public static void CallOnScoreChanged(long _score)
        {
            OnScoreChanged?.Invoke(new OnScoreChangedEventArgs()
            {
                score = _score
            });
        }
    }



    public class OnRoomChangedEventArgs : EventArgs
    {
        public Room room;
    }



    public class OnRoomEnemiesDefeatedEventArgs : EventArgs
    {
        public Room room;
    }



    public class OnPointScoredEventArgs : EventArgs
    {
        public int point;
    }



    public class OnScoreChangedEventArgs : EventArgs
    {
        public long score;
    }
}

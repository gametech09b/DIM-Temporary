using System;
using DIM.DungeonSystem;

namespace DIM {
    public static class DungeonStaticEvent {
        public static event Action<OnRoomChangedEventArgs> OnRoomChanged;
        public static void CallOnRoomChanged(Room _room) {
            OnRoomChanged?.Invoke(new OnRoomChangedEventArgs() {
                room = _room
            });
        }



        public static event Action<OnRoomEnemiesDefeatedEventArgs> OnRoomEnemiesDefeated;
        public static void CallOnRoomEnemiesDefeated(Room _room) {
            OnRoomEnemiesDefeated?.Invoke(new OnRoomEnemiesDefeatedEventArgs() {
                room = _room
            });
        }



        public static event Action<OnPointScoredEventArgs> OnPointScored;
        public static void CallOnPointScored(int _point) {
            OnPointScored?.Invoke(new OnPointScoredEventArgs() {
                point = _point
            });
        }



        public static event Action<OnScoreChangedEventArgs> OnScoreChanged;
        public static void CallOnScoreChanged(long _score, int _multiplier) {
            OnScoreChanged?.Invoke(new OnScoreChangedEventArgs() {
                score = _score,
                multiplier = _multiplier
            });
        }



        public static event Action<OnMultiplierChangedEventArgs> OnMultiplierChanged;
        public static void CallOnMultiplierChanged(bool _isMultiplierActive) {
            OnMultiplierChanged?.Invoke(new OnMultiplierChangedEventArgs() {
                isMultiplier = _isMultiplierActive
            });
        }
    }



    public class OnRoomChangedEventArgs : EventArgs {
        public Room room;
    }



    public class OnRoomEnemiesDefeatedEventArgs : EventArgs {
        public Room room;
    }



    public class OnPointScoredEventArgs : EventArgs {
        public int point;
    }



    public class OnScoreChangedEventArgs : EventArgs {
        public long score;
        public int multiplier;
    }



    public class OnMultiplierChangedEventArgs : EventArgs {
        public bool isMultiplier;
    }
}

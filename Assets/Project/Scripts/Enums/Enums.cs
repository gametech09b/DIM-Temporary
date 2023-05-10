namespace DungeonGunner {
    public enum Orientation {
        NORTH,
        EAST,
        SOUTH,
        WEST,
        NONE
    }



    public enum Direction {
        UP,
        UP_RIGHT,
        UP_LEFT,
        RIGHT,
        DOWN,
        LEFT,
        NONE
    }



    public enum GameState {
        GAME_STARTED,
        PLAYING_LEVEL,
        ENGAGING_ENEMY,
        BOSS_STAGE,
        ENGAGING_BOSS,
        LEVEL_COMPLETED,
        GAME_WON,
        GAME_LOST,
        GAME_PAUSED,
        DUNGEON_OVERVIEW_MAP,
        RESTART_GAME
    }



    public enum ChestSpawnEventType {
        ON_ROOM_ENTRY,
        ON_ENEMIES_DEFEATED,
    }



    public enum ChestSpawnPointType {
        SPAWNER_POSITION,
        PLAYER_POSITION,
    }



    public enum ChestState {
        CLOSED,
        AMMO_ITEM,
        HEALTH_ITEM,
        WEAPON_ITEM,
        EMPTY,
    }
}


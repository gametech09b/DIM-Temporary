using UnityEngine;

namespace DIM.DungeonSystem {
    [System.Serializable]
    public class Doorway {
        public Vector2Int position;
        public Orientation orientation;
        public GameObject doorPrefab;

        [Space(10)]
        [Header("The Upper Left Position To Start Copying From")]
        public Vector2Int startCopyPosition;

        [Space(10)]
        [Header("The width of tiles in the doorway to copy over")]
        public int copyTileWidth;

        [Space(10)]
        [Header("The height of tiles in the doorway to copy over")]
        public int copyTileHeight;

        [HideInInspector] public bool isConnected = false;
        [HideInInspector] public bool isUnavailable = false;
    }
}
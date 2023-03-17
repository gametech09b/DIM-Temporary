using UnityEngine;

namespace DungeonGunner
{
    [System.Serializable]
    public class Doorway
    {
        public Vector2Int position;
        public Orientation orientation;
        public GameObject doorPrefab;



        [Space(10)]
        [Header("The Upper Left Position To Start Copying From")]
        public Vector2Int doorwayStartCopyPosition;



        [Space(10)]
        [Header("The width of tiles in the doorway to copy over")]
        public int doorwayCopyTileWidth;



        [Space(10)]
        [Header("The height of tiles in the doorway to copy over")]
        public int doorwayCopyTileHeight;



        [HideInInspector] public bool isConnected = false;
        [HideInInspector] public bool isUnavailable = false;
    }
}
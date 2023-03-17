using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    public class GameResources : MonoBehaviour
    {
        #region Singleton GameResources
        private static GameResources instance;
        public static GameResources Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<GameResources>("GameResources");
                }
                return instance;
            }
        }
        #endregion



        [Space(10)]
        [Header("Dungeon")]


        [Tooltip("Populate with the dungeon RoomNodeTypeListSO")]
        public RoomNodeTypeListSO roomNodeTypeList;
    }
}

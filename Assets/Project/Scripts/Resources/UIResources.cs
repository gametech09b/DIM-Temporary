using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    public class UIResources : MonoBehaviour {
        #region Singleton UIResources
        private static UIResources instance;
        public static UIResources Instance {
            get
            {
                if (instance == null)
                    instance = Resources.Load<UIResources>("UIResources");

                return instance;
            }
        }
        #endregion



        [Space(10)]
        [Header("Weapon HUD")]

        public GameObject AmmoIconPrefab;



        [Space(10)]
        [Header("Health HUD")]


        public GameObject HealthIconPrefab;



        [Space(10)]
        [Header("Chest")]


        public GameObject ChestItemPrefab;
        public Sprite HeartIconSprite;
        public Sprite BulletIconSprite;
    }
}

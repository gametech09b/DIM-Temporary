using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    public class SoundEffectResources : MonoBehaviour
    {
        #region Singleton SoundEffectResources
        private static SoundEffectResources instance;
        public static SoundEffectResources Instance
        {
            get
            {
                if (instance == null)
                    instance = Resources.Load<SoundEffectResources>("SoundEffectResources");

                return instance;
            }
        }
        #endregion



        [Space(10)]
        [Header("Door SFX")]


        public SoundEffectSO DoorOpenCloseSoundEffect;
    }
}

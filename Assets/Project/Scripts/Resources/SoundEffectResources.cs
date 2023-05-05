using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    public class SoundEffectResources : MonoBehaviour {
        #region Singleton SoundEffectResources
        private static SoundEffectResources instance;
        public static SoundEffectResources Instance {
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



        [Space(10)]
        [Header("Table SFX")]


        public SoundEffectSO TableFlipSoundEffect;



        [Space(10)]
        [Header("Chest SFX")]


        public SoundEffectSO ChestOpenSoundEffect;
        public SoundEffectSO ChestAmmoPickupSoundEffect;
        public SoundEffectSO ChestHealthPickupSoundEffect;
        public SoundEffectSO ChestWeaponPickupSoundEffect;



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckNullValue(this, nameof(DoorOpenCloseSoundEffect), DoorOpenCloseSoundEffect);
            HelperUtilities.CheckNullValue(this, nameof(TableFlipSoundEffect), TableFlipSoundEffect);
        }
#endif
        #endregion
    }
}

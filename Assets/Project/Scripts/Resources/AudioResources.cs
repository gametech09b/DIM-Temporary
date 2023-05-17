using UnityEngine;
using UnityEngine.Audio;

using DIM.AudioSystem;

namespace DIM {
    public class AudioResources : MonoBehaviour {
        #region Singleton AudioResources
        private static AudioResources instance;
        public static AudioResources Instance {
            get
            {
                if (instance == null)
                    instance = Resources.Load<AudioResources>("AudioResources");

                return instance;
            }
        }
        #endregion



        [Space(10)]
        [Header("Audio Master")]

        public AudioMixerGroup AudioMixerGroup_Master;


        [Space(10)]
        [Header("Sound Effects")]

        public AudioMixerGroup AudioMixerGroup_Sound;


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



        [Space(10)]
        [Header("Music Track")]


        public AudioMixerGroup AudioMixerGroup_Music;
        public AudioMixerSnapshot MusicSnapshot_OnFull;
        public AudioMixerSnapshot MusicSnapshot_OnLow;
        public AudioMixerSnapshot MusicSnapshot_Off;


        [Space(10)]
        [Header("Main Menu Music")]

        public MusicTrackSO MainMenuMusicTrack;

        // ===================================================================

        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckNullValue(this, nameof(AudioMixerGroup_Master), AudioMixerGroup_Master);

            HelperUtilities.CheckNullValue(this, nameof(DoorOpenCloseSoundEffect), DoorOpenCloseSoundEffect);
            HelperUtilities.CheckNullValue(this, nameof(TableFlipSoundEffect), TableFlipSoundEffect);

            HelperUtilities.CheckNullValue(this, nameof(ChestOpenSoundEffect), ChestOpenSoundEffect);
            HelperUtilities.CheckNullValue(this, nameof(ChestAmmoPickupSoundEffect), ChestAmmoPickupSoundEffect);
            HelperUtilities.CheckNullValue(this, nameof(ChestHealthPickupSoundEffect), ChestHealthPickupSoundEffect);
            HelperUtilities.CheckNullValue(this, nameof(ChestWeaponPickupSoundEffect), ChestWeaponPickupSoundEffect);

            HelperUtilities.CheckNullValue(this, nameof(AudioMixerGroup_Music), AudioMixerGroup_Music);
            HelperUtilities.CheckNullValue(this, nameof(MusicSnapshot_OnFull), MusicSnapshot_OnFull);
            HelperUtilities.CheckNullValue(this, nameof(MusicSnapshot_OnLow), MusicSnapshot_OnLow);
            HelperUtilities.CheckNullValue(this, nameof(MusicSnapshot_Off), MusicSnapshot_Off);

        }
#endif
        #endregion
    }
}

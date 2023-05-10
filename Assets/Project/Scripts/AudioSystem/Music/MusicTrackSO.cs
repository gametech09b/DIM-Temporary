using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [CreateAssetMenu(fileName = "MusicTrack_", menuName = "Scriptable Objects/Audio/Music Track")]
    public class MusicTrackSO : ScriptableObject {
        [Space(10)]
        [Header("Music Track Detail")]


        public string musicTrackName;

        public AudioClip audioClip;

        [Range(0f, 1f)] public float volume = 1f;




        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckEmptyString(this, nameof(musicTrackName), musicTrackName);
            HelperUtilities.CheckNullValue(this, nameof(audioClip), audioClip);
            HelperUtilities.CheckPositiveValue(this, nameof(volume), volume);
        }
#endif
        #endregion
    }
}

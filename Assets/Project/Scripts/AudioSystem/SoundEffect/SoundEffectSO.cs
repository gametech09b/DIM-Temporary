using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [CreateAssetMenu(fileName = "SoundEffect_", menuName = "Scriptable Objects/Audio/Sound Effect")]
    public class SoundEffectSO : ScriptableObject
    {

        [Space(10)]
        [Header("Sound Effect Detail")]


        public string soundEffectName;
        public GameObject prefab;
        public AudioClip audioClip;



        [Range(0.1f, 1.5f)] public float minPitchVariation = 0.8f;
        [Range(0.1f, 1.5f)] public float maxPitchVariation = 1.2f;
        [Range(0f, 1f)] public float volume = 1f;



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckEmptyString(this, nameof(soundEffectName), soundEffectName);
            HelperUtilities.CheckNullValue(this, nameof(prefab), prefab);
            HelperUtilities.CheckNullValue(this, nameof(audioClip), audioClip);
            HelperUtilities.CheckPositiveRange(this, nameof(minPitchVariation), nameof(maxPitchVariation), minPitchVariation, maxPitchVariation);
            HelperUtilities.CheckPositiveValue(this, nameof(volume), volume);
        }
#endif
        #endregion
    }
}

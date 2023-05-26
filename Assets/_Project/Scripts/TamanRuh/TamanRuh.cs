using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIM.AudioSystem;

namespace DIM
{
    public class TamanRuh : MonoBehaviour
    {
        [SerializeField] private MusicTrackSO musicTrack;

        private void Start() 
        {
            MusicManager.Instance.PlayMusic(musicTrack);
        }

    }
}

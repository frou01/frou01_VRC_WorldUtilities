using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace frou01.util
{
    public class AutoRepeatSoundPlayer_UdonBehaviour : UdonSharpBehaviour
    {
        //リピート保証付きループ音源
        [SerializeField] public AudioSource targetAudioSource;

        private void OnEnable()
        {
            if(targetAudioSource != null) targetAudioSource.Play();
            SendCustomEventDelayedSeconds(nameof(CheckPlaying), 1);
        }

        public void CheckPlaying()
        {
            if (gameObject.activeInHierarchy) SendCustomEventDelayedSeconds(nameof(CheckPlaying), 1);
            else return;
            if (targetAudioSource.enabled == true && !targetAudioSource.isPlaying)
            {
                targetAudioSource.Play();
            }
        }
    }
}
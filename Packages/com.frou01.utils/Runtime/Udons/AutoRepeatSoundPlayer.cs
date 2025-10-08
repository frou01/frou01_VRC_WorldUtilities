
using UdonSharp;
using UnityEngine;

public class AutoRepeatSoundPlayer : UdonSharpBehaviour
{
    //リピート保証付きループ音源
    [SerializeField] AudioSource targetAudioSource;

    private void OnEnable()
    {
        targetAudioSource.Play();
        SendCustomEventDelayedSeconds(nameof(CheckPlaying), 1);
    }

    public void CheckPlaying()
    {
        if (targetAudioSource.enabled == true && !targetAudioSource.isPlaying)
        {
            targetAudioSource.Play();
        }
        if (gameObject.activeInHierarchy) SendCustomEventDelayedSeconds(nameof(CheckPlaying), 1);
    }
}
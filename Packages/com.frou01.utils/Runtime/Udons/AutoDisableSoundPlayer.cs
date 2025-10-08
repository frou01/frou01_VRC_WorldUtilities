
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AutoDisableSoundPlayer : UdonSharpBehaviour
{
    [SerializeField] AudioSource targetAudioSource;
    [SerializeField] float AutoDisableTime;
    float cnt;
    void Start()
    {
        
    }

    public void Update()
    {
        cnt -= Time.deltaTime;
        if (cnt < 0)
        {
            this.enabled = false;
            targetAudioSource.enabled = false;
        }
    }
    public void PlayOnshot(AudioClip clip)
    {
        this.enabled = true;
        targetAudioSource.enabled = true;
        targetAudioSource.PlayOneShot(clip == null ? targetAudioSource.clip : clip);
        cnt = AutoDisableTime;
    }
    public void PlayOnshot()
    {
        PlayOnshot(null);
    }
}

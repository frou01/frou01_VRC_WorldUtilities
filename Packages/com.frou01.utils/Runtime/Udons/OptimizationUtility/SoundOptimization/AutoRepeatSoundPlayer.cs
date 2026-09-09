using UdonSharp;
using UnityEngine;

namespace frou01.util
{
    public class AutoRepeatSoundPlayer : MonoBehaviour
    {
        //リピート保証付きループ音源
        [SerializeField] public AudioSource targetAudioSource;
    }
}
using frou01.util.editor;
using UdonSharp;
using UnityEngine;
using UnityEngine.Serialization;

namespace frou01.util.placeholder
{
    public class AutoRepeatSoundPlayer : MonoBehaviour, IUdonPlaceHolder
    {
        //リピート保証付きループ音源
        [FormerlySerializedAs("targetAudioSource")]
        [SerializeField] public AudioSource targetAudioSource;

        public void InstantiationUdon()
        {
            AutoRepeatSoundPlayer_UdonBehaviour repeaterBehaviour = gameObject.AddUdonSharpComponentAlignSync<AutoRepeatSoundPlayer_UdonBehaviour>();

            repeaterBehaviour.targetAudioSource = targetAudioSource;

            Object.Destroy(this);
        }
    }
}
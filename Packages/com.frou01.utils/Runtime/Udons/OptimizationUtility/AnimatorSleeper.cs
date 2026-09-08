using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace frou01.util
{
    public class AnimatorSleeper : MonoBehaviour
    {
        [SerializeField] public Animator animator;
        [SerializeField] public float waitTime = 2;
    }
}

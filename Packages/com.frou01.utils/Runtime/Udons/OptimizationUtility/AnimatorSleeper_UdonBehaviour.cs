
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace frou01.util
{
    public class AnimatorSleeper_UdonBehaviour : UdonSharpBehaviour
    {
        int isActiveHash;

        float delayer;

        [SerializeField] public Animator animator;
        [SerializeField] public float waitTime = 2;

        void Start()
        {
            isActiveHash = Animator.StringToHash("isActive");
        }
        public void Update()
        {
            //Debug.Log("debug animator enabled" + animator.enabled);
            if (animator.GetFloat(isActiveHash) == 0 && animator.enabled)
            {
                //Debug.Log("debug_disabling");
                if (delayer > waitTime)
                {
                    animator.enabled = false;
                    ResetCount();
                }
                delayer += Time.deltaTime;
            }
            else
            {
                ResetCount();
            }
            enabled = false;
        }
        public void ResetCount()
        {
            delayer = 0;
        }
    }
}

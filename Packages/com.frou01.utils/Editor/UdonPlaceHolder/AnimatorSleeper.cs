using frou01.util.editor;

using UnityEngine;
using UnityEngine.Serialization;
using VRC.Udon;

namespace frou01.util.placeholder
{
    public class AnimatorSleeper : MonoBehaviour, IUdonPlaceHolder
    {
        [FormerlySerializedAs("animator")]
        [SerializeField] public Animator animator;
        [FormerlySerializedAs("waitTime")]
        [SerializeField] public float waitTime = 2;

        public void InstantiationUdon()
        {
#if !COMPILER_UDONSHARP && UNITY_EDITOR
            AnimatorSleeper_UdonBehaviour udon_Sleeper = gameObject.AddUdonSharpComponentAlignSync<AnimatorSleeper_UdonBehaviour>();

            UdonBehaviour backingUB = UdonSharpEditor.UdonSharpEditorUtility.GetBackingUdonBehaviour(udon_Sleeper);
            while (UnityEditorInternal.ComponentUtility.MoveComponentUp(backingUB)) ;

            udon_Sleeper.animator = this.animator;
            udon_Sleeper.waitTime = this.waitTime;

            Object.Destroy(this);
#endif
        }
    }
}

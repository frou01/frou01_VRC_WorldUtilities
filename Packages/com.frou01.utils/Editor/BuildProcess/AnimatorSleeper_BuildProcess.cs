

using System.Collections.Generic;
using UdonSharpEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.Udon;

namespace frou01.util.editor
{
    public class AnimatorSleeper_BuildProcess : IProcessSceneWithReport
    {
        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            List<AnimatorSleeper> sleeperPlaceHolders = new List<AnimatorSleeper>();
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                sleeperPlaceHolders.AddRange(obj.GetComponentsInChildren<AnimatorSleeper>(true));
            }

            foreach(AnimatorSleeper sleeperPlaceHolder in sleeperPlaceHolders)
            {
                AnimatorSleeper_UdonBehaviour udon_Sleeper = sleeperPlaceHolder.gameObject.AddUdonSharpComponentAlignSync<AnimatorSleeper_UdonBehaviour>();

                UdonBehaviour backingUB = UdonSharpEditorUtility.GetBackingUdonBehaviour(udon_Sleeper);
                while (UnityEditorInternal.ComponentUtility.MoveComponentUp(backingUB)) ;

                udon_Sleeper.animator = sleeperPlaceHolder.animator;
                udon_Sleeper.waitTime = sleeperPlaceHolder.waitTime;

                Object.Destroy(sleeperPlaceHolder);
            }
        }
    }
}

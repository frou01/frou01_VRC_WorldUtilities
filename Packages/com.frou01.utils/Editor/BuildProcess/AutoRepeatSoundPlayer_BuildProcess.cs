using System.Collections;
using System.Collections.Generic;
using UdonSharpEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.Udon;

namespace frou01.util.editor
{
    public class AutoRepeatSoundPlayer_BuildProcess : IProcessSceneWithReport
    {
        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            List<AutoRepeatSoundPlayer> RepeaterPlaceHolders = new List<AutoRepeatSoundPlayer>();
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                RepeaterPlaceHolders.AddRange(obj.GetComponentsInChildren<AutoRepeatSoundPlayer>(true));
            }

            foreach (AutoRepeatSoundPlayer repeaterPlaceHolder in RepeaterPlaceHolders)
            {
                if (repeaterPlaceHolder.gameObject)
                {
                    AutoRepeatSoundPlayer_UdonBehaviour repeaterBehaviour = repeaterPlaceHolder.gameObject.AddUdonSharpComponentAlignSync<AutoRepeatSoundPlayer_UdonBehaviour>();

                    repeaterBehaviour.targetAudioSource = repeaterPlaceHolder.targetAudioSource;

                    Object.Destroy(repeaterPlaceHolder);
                }
            }
        }
    }
}

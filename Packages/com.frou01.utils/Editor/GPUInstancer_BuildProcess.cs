using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.SDKBase.Editor.BuildPipeline;

namespace frou01.util.editor
{
    public class GPUInstancer_BuildProcess : IProcessSceneWithReport, IVRCSDKBuildRequestedCallback
    {
        public int callbackOrder => 5;

        public List<GPUInstancer> target = new List<GPUInstancer>();


        public void OnProcessScene(Scene scene, BuildReport report)
        {
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                //    //Debug.Log("OK!");
                Proceed(obj.transform);
            }
            foreach (GPUInstancer obj in target)
            {
                obj.ProceedInstancing();
            }
        }

        void Proceed(Transform parent)
        {
            target.AddRange(parent.gameObject.GetComponentsInChildren<GPUInstancer>(true));
        }

        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            return true;
        }
    }
}
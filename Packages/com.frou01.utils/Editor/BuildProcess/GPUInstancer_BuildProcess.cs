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
                target.AddRange(obj.GetComponentsInChildren<GPUInstancer>(true));
            }
            foreach (GPUInstancer obj in target)
            {
                obj.ProceedInstancing();
            }
        }

        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            return true;
        }
    }
}
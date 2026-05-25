using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.SDKBase.Editor.BuildPipeline;

namespace frou01.util.editor
{
    public class Hierarchy_Optimizer_onEditor : IProcessSceneWithReport, IVRCSDKBuildRequestedCallback
    {
        public int callbackOrder => 5;

        public List<Hierarchy_Optimizer> target = new List<Hierarchy_Optimizer>();


        public void OnProcessScene(Scene scene, BuildReport report)
        {
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                //    //Debug.Log("OK!");
                Proceed(obj.transform);
            }
            foreach (Hierarchy_Optimizer obj in target)
            {
                if (obj != null && (obj.gameObject.activeInHierarchy || obj.forceProceed))
                {
                    //        Debug.Log("MoveToRoot" + obj.name);
                    obj.transform.parent = obj.target;
                }
            }
        }

        void Proceed(Transform parent)
        {
            target.AddRange(parent.gameObject.GetComponentsInChildren<Hierarchy_Optimizer>(true));
        }

        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            return true;
        }
    }
}
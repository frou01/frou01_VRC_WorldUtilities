using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.SDKBase.Editor.BuildPipeline;

namespace frou01.util.editor
{
    public class DynamicBatching_onEditor : IProcessSceneWithReport
    {
        public int callbackOrder => 1;



        public void OnProcessScene(Scene scene, BuildReport report)
        {
            List<DynamicBatching> target = new List<DynamicBatching>();
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                target.AddRange(obj.GetComponentsInChildren<DynamicBatching>(true));
            }
            foreach (DynamicBatching obj in target)
            {
                if (obj != null)
                {
                    GameObject[] gos = obj.batchingObjects;
                    Debug.Log("Bathcing!" + GetHierarchyPath(obj.gameObject));
                    StaticBatchingUtility.Combine(gos, obj.gameObject);
                }
            }
        }
        private static string GetHierarchyPath(GameObject targetObj)
        {
            List<GameObject> objPath = new List<GameObject>();
            objPath.Add(targetObj);
            for (int i = 0; objPath[i].transform.parent != null; i++)
                objPath.Add(objPath[i].transform.parent.gameObject);
            string path = objPath[objPath.Count - 1].gameObject.name;
            for (int i = objPath.Count - 2; i >= 0; i--)
                path += "/" + objPath[i].gameObject.name;

            return path;
        }

        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            return true;
        }
    }
}
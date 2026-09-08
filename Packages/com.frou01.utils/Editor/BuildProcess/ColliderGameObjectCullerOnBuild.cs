using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UdonSharpEditor;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.SDKBase.Editor.BuildPipeline;
using VRC.Udon;
using static VRC.SDKBase.Networking;

namespace frou01.util.editor
{
    public class ColliderGameObjectCullerOnBuild : IProcessSceneWithReport
    {
        public int callbackOrder => 0;  


        public List<ColliderGameObjectCuller> targetCGC = new List<ColliderGameObjectCuller>();
        public List<ColliderOcclusionPortal> targetCOP = new List<ColliderOcclusionPortal>();
        public List<ColliderBaseLOD> targetCBL = new List<ColliderBaseLOD>();


        public void OnProcessScene(Scene scene, BuildReport report)
        {
            targetCGC = new List<ColliderGameObjectCuller>();
            targetCOP = new List<ColliderOcclusionPortal>();
            targetCBL = new List<ColliderBaseLOD>();
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                targetCGC.AddRange(obj.GetComponentsInChildren<ColliderGameObjectCuller>(true));
                targetCOP.AddRange(obj.GetComponentsInChildren<ColliderOcclusionPortal>(true));
                targetCBL.AddRange(obj.GetComponentsInChildren<ColliderBaseLOD>(true));
            }
            string pattern = @"^(?=.*instanced).*$";//部分一致 instanced
            foreach (ColliderGameObjectCuller currentCGC in targetCGC)
            {
                if (currentCGC != null)
                {
                    //Debug.Log("SetUp " + currentCGC.name);
                    foreach (GameObject go in currentCGC.objects)
                    {
                        if (go == null)
                        {
                            Debug.LogError("Culler array has missing : " + GetPath(currentCGC.transform), currentCGC);
                        }
                        else
                        {
                            if (currentCGC.gameObject.activeInHierarchy) go.SetActive(false);
                        }
                    }
                    currentCGC.objects = currentCGC.objects.Where(val => val != null).ToArray();
                    if (currentCGC.isStaticMode)
                    {
                        List<GameObject> staticmeshes = new List<GameObject>();
                        foreach (GameObject go in currentCGC.objects)
                        {
                            bool isinstanced = Regex.IsMatch(go.name, pattern);
                            if (!isinstanced)
                            {
                                staticmeshes.Add(go);
                            }
                        }
                        StaticBatchingUtility.Combine(staticmeshes.ToArray(), null);
                    }
                    ColliderGameObjectCuller_UdonBehaviour CGCUB = currentCGC.gameObject.AddUdonSharpComponentAlignSync<ColliderGameObjectCuller_UdonBehaviour>();
                    CGCUB.objects = currentCGC.objects;
                }
            }
            List<GameObject> existMeshes_Near = new List<GameObject>();
            List<GameObject> existMeshes_Dist = new List<GameObject>();
            foreach (ColliderBaseLOD currentCBL in targetCBL)
            {
                if (currentCBL != null)
                {
                    foreach (GameObject go in currentCBL.NearObjects)
                    {
                        if (go == null)
                        {
                            Debug.Log("ColliderLOD Near array has missing : " + GetPath(currentCBL.transform), currentCBL);
                            continue;
                        }
                        existMeshes_Near.Add(go);
                        go.SetActive(false);
                        foreach(Hierarchy_Optimizer ho in go.GetComponentsInChildren<Hierarchy_Optimizer>(true))
                        {
                            ho.forceProceed = true;
                        }
                    }
                    currentCBL.NearObjects = existMeshes_Near.ToArray();
                    foreach (GameObject go in currentCBL.DistObjects)
                    {
                        if (go == null)
                        {
                            Debug.Log("ColliderLOD Dist array has missing : " + GetPath(currentCBL.transform), currentCBL);
                            continue;
                        }
                        existMeshes_Dist.Add(go);
                        go.SetActive(true);
                        foreach (Hierarchy_Optimizer ho in go.GetComponentsInChildren<Hierarchy_Optimizer>(true))
                        {
                            ho.forceProceed = true;
                        }
                    }
                    currentCBL.DistObjects = existMeshes_Dist.ToArray();
                    foreach (GameObject go_Near in existMeshes_Near)
                    {
                        foreach (GameObject go_Dist in existMeshes_Dist)
                        {
                            if (go_Near.transform.IsChildOf(go_Dist.transform))
                            {
                                go_Near.transform.parent = go_Dist.transform.parent;
                            }
                            if (go_Dist.transform.IsChildOf(go_Near.transform))
                            {
                                go_Dist.transform.parent = go_Near.transform.parent;
                            }
                        }
                    }
                    existMeshes_Near.Clear();
                    existMeshes_Dist.Clear();
                }
            }
            foreach (ColliderOcclusionPortal obj in targetCOP)
            {
                if (obj != null && obj.gameObject.activeInHierarchy)
                {
                    //Debug.Log("SetUp" + obj.name);
                    obj.GetComponent<OcclusionPortal>().open = false;
                }
            }
        }

        private string GetPath(Transform t)
        {
            string path = t.name;
            while (t.parent)
            {
                path = t.parent.name + " , " + path;
                t = t.parent;
            }
            return path;
        }

        void Proceed(Transform parent)
        {
        }

        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            return true;
        }
    }
}
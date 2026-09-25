using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.SDK3.Components;
using VRC.SDKBase.Editor.BuildPipeline;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace frou01.util.editor
{
    public class InteractActivator_HandSetter : IProcessSceneWithReport
    {
        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            Debug.Log("IA_onEditor");
            PlayerChaser playerChaser = null;
            List<InteractActivator_UdonBehaviour> interactAvtivators = new List<InteractActivator_UdonBehaviour>();
            List<ColliderUdonCuller> colliderUdonCullers = new List<ColliderUdonCuller>();
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                if (obj.GetComponentInChildren<PlayerChaser>() != null)
                {
                    playerChaser = obj.GetComponentInChildren<PlayerChaser>();
                }
                interactAvtivators.AddRange(obj.GetComponentsInChildren<InteractActivator_UdonBehaviour>(true));

                colliderUdonCullers.AddRange(obj.GetComponentsInChildren<ColliderUdonCuller>(true));
            }

            foreach (InteractActivator_UdonBehaviour IA in interactAvtivators)
            {
                IA.Head = playerChaser.transform;
                IA.handL = playerChaser.HandL;
                IA.handR = playerChaser.HandR;
            }

            foreach (ColliderUdonCuller CUC in colliderUdonCullers)
            {
                List<UdonBehaviour> targetUdons = new List<UdonBehaviour>();
                targetUdons.AddRange(CUC.targetUdons);
                foreach (GameObject targetOBJ in CUC.targetGameObject)
                {
                    if (targetOBJ == null)
                    {
                        Debug.LogError("Null element on " + GetPath(CUC.transform), CUC);
                        continue;
                    }
                    UdonBehaviour[] udonBehaviours = targetOBJ.GetComponents<UdonBehaviour>();
                    foreach (UdonBehaviour udon in udonBehaviours)
                    {
                        if (!udon) { Debug.LogError("Missing found ", CUC); continue; }
                        bool hasSyncVar = CSharpUdonSharpHelper.HasSyncVariable(udon);

                        if (!hasSyncVar)
                        {
                            udon.enabled = false;
                            targetUdons.Add(udon);
                        }
                    }
                }
                if (CUC.GetComponentInParent<Rigidbody>(true))
                {
                    if (!CUC.gameObject.GetComponent<Rigidbody>())
                    {
                        Rigidbody newrb = CUC.gameObject.AddComponent<Rigidbody>();
                        newrb.isKinematic = true;
                        newrb.useGravity = false;
                        newrb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    }
                }
                else
                {
                    foreach (Collider col in CUC.gameObject.GetComponents<Collider>())
                    {
                        col.providesContacts = true;
                    }
                }
                CUC.targetUdons = targetUdons.ToArray();
                CUC.playerChaser = playerChaser.gameObject;
                targetUdons.Clear();
            }
        }

        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            return true;
        }

        private string GetPath(Transform t)
        {
            string path = t.name;
            while (t.parent)
            {
                path = t.parent.name + path;
                t = t.parent;
            }
            return path;
        }
    }
}
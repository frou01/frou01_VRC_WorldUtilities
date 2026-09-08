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

        public PlayerChaser playerChaser;

        public List<InteractActivator> interactAvtivators = new List<InteractActivator>();
        public List<ColliderUdonCuller> colliderUdonCullers = new List<ColliderUdonCuller>();

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            Debug.Log("IA_onEditor");
            playerChaser = null;
            interactAvtivators = new List<InteractActivator>();
            colliderUdonCullers = new List<ColliderUdonCuller>();
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                if (obj.GetComponentInChildren<PlayerChaser>() != null)
                {
                    playerChaser = obj.GetComponentInChildren<PlayerChaser>();
                }
                interactAvtivators.AddRange(obj.GetComponentsInChildren<InteractActivator>(true));

                colliderUdonCullers.AddRange(obj.GetComponentsInChildren<ColliderUdonCuller>(true));
            }

            foreach (InteractActivator PlaceHolderIA in interactAvtivators)
            {
                InteractActivator_UdonBehaviour IA = PlaceHolderIA.gameObject.AddUdonSharpComponentAlignSync<InteractActivator_UdonBehaviour>();


                List<GameObject> targetUdonGameObj = new List<GameObject>();
                List<UdonBehaviour> targetUdons = new List<UdonBehaviour>();
                List<VRCPickup> targetPicks = new List<VRCPickup>();
                //Debug.Log(IA);
                IA.Head = playerChaser.transform;
                IA.handL = playerChaser.HandL;
                IA.handR = playerChaser.HandR;
                IA.colliders = PlaceHolderIA.colliders;
                IA.pickups = PlaceHolderIA.pickups;
                IA.udons = PlaceHolderIA.udons;
                IA.proximity = PlaceHolderIA.proximity;
                IA.BaseTransforms = PlaceHolderIA.BaseTransforms;
                IA.currentState = PlaceHolderIA.currentState;
                targetUdonGameObj.Add(IA.gameObject);
                foreach (UdonBehaviour udon in IA.udons)
                {
                    if (!targetUdonGameObj.Contains(udon.gameObject)) targetUdonGameObj.Add(udon.gameObject);
                }
                foreach (VRCPickup pickup in IA.pickups)
                {
                    if (!targetUdonGameObj.Contains(pickup.gameObject)) targetUdonGameObj.Add(pickup.gameObject);
                }
                foreach (GameObject targetOBJ in targetUdonGameObj)
                {
                    foreach (UdonBehaviour udon in targetOBJ.GetComponents<UdonBehaviour>())
                    {
                        udon.proximity = float.MaxValue;
                        targetUdons.Add(udon);
                    }
                }
                foreach (GameObject targetOBJ in targetUdonGameObj)
                {
                    foreach (VRCPickup pickup in targetOBJ.GetComponents<VRCPickup>())
                    {
                        pickup.proximity = float.MaxValue;
                        targetPicks.Add(pickup);
                    }
                }
                string interactionText = string.Empty;
                foreach (UdonBehaviour udon in targetUdons)
                {
                    if (interactionText.Length < udon.interactText.Length) interactionText = udon.interactText;
                }
                foreach (VRCPickup pick in targetPicks)
                {
                    if (interactionText.Length < pick.InteractionText.Length) interactionText = pick.InteractionText;
                }
                foreach (UdonBehaviour udon in targetUdons)
                {
                    udon.interactText = interactionText;
                }
                foreach (VRCPickup pick in targetPicks)
                {
                    pick.InteractionText = interactionText;
                }
                IA.udons = targetUdons.ToArray();
                IA.pickups = targetPicks.ToArray();
                IA.currentState = true;
                IA.changeColliderState(false);
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
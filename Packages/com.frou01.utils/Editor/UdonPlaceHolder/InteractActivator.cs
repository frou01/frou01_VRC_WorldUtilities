using frou01.util.editor;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using UnityEngine.Serialization;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

namespace frou01.util.placeholder
{
    public class InteractActivator : MonoBehaviour , IUdonPlaceHolder
    {
        public Transform Head;
        public Transform handL;
        public Transform handR;
        [Header("Interaction overrider.")]
        [Header("If Hand enter nearby, this udon activate Interaction.")]
        [Header("No assigned target, this udon control interaction flag of self GameObject")]
        [Header("Only assign collider, this udon control collider enable/disable state")]
        [SerializeField] public Collider[] colliders;
        [Header("Assigned below, this udon control interaction flag")]
        [SerializeField] public VRC_Pickup[] pickups;
        [SerializeField] public UdonBehaviour[] udons;
        [SerializeField] public float proximity;
        [SerializeField] public Transform[] BaseTransforms = new Transform[0];
        public bool currentState;
        Vector3 pos;

        public void InstantiationUdon()
        {
            InteractActivator_UdonBehaviour IA = this.gameObject.AddUdonSharpComponentAlignSync<InteractActivator_UdonBehaviour>();


            List<GameObject> targetUdonGameObj = new List<GameObject>();
            List<UdonBehaviour> targetUdons = new List<UdonBehaviour>();
            List<VRCPickup> targetPicks = new List<VRCPickup>();

            IA.colliders = this.colliders;
            IA.pickups = this.pickups;
            IA.udons = this.udons;
            IA.proximity = this.proximity;
            IA.BaseTransforms = this.BaseTransforms;
            IA.currentState = this.currentState;

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


#if !COMPILER_UDONSHARP && UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            if (BaseTransforms.Length == 0)
            {
                pos = transform.position;
                Gizmos.DrawWireSphere(pos, proximity);
                if (colliders.Length <= 0 && pickups.Length <= 0 && udons.Length <= 0)
                {
                    Gizmos.color = new Color(1, 0, 0, 0.4f);

                    Gizmos.DrawSphere(pos, proximity);
                }
                foreach (Component com in colliders)
                {
                    Gizmos.DrawLine(pos, com.transform.position);
                }
                foreach (Component com in pickups)
                {
                    Gizmos.DrawLine(pos, com.transform.position);
                }
                foreach (Component com in udons)
                {
                    Gizmos.DrawLine(pos, com.transform.position);
                }
            }
            else
            {
                foreach (Transform baseTransform in BaseTransforms)
                {
                    pos = baseTransform.position;
                    Gizmos.DrawWireSphere(pos, proximity);
                    if (colliders.Length <= 0 && pickups.Length <= 0 && udons.Length <= 0)
                    {
                        Gizmos.color = new Color(1, 0, 0, 0.4f);

                        Gizmos.DrawSphere(pos, proximity);
                    }
                    foreach (Component com in colliders)
                    {
                        Gizmos.DrawLine(pos, com.transform.position);
                    }
                    foreach (Component com in pickups)
                    {
                        Gizmos.DrawLine(pos, com.transform.position);
                    }
                    foreach (Component com in udons)
                    {
                        Gizmos.DrawLine(pos, com.transform.position);
                    }
                }
            }
        }
#endif
    }
}
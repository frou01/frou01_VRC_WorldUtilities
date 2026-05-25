using System;
using UdonSharp;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Serialization.OdinSerializer;

namespace frou01.util
{
    public class SyncedObjectSwitch : UdonSharpBehaviour
    {
        [Tooltip("Object changes will be applied.")]
        [SerializeField] GameObject[] targets;

        bool init;

        [Tooltip("Transform toggle targets. This udon copy position and rotation on World-Space.")]
        [OdinSerialize] public Transform[][] m_Target_Transforms = new Transform[0][];

        [UdonSynced] int SyncedState = 0;
        [Tooltip("Initial state. FALSE: keep hierarchy state. TRUE: inverse hierarchy state.")]
        [SerializeField] int LocalState = 0;
        [Tooltip("Ignore sync")]
        [SerializeField] bool LocalMode;


        protected virtual void Initialize()
        {
            ApplyState();
            init = true;
        }

        protected virtual void ApplyState()
        {
            for (int index = 0; index < targets.Length; index++)
            {
                targets[index].transform.SetPositionAndRotation(m_Target_Transforms[index][LocalState].position, m_Target_Transforms[index][LocalState].rotation);
            }
        }

        protected virtual void CheckInitAndApply()
        {
            if (!init)
            {
                Initialize();
            }
            else
            {
                ApplyState();
            }
        }
        private void Start()
        {
            if (!init)
            {
                Initialize();
            }
        }
        public override void Interact()
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            LocalState++;
            if (LocalState >= m_Target_Transforms[0].Length) LocalState = 0;
            if (!LocalMode) SyncedState = LocalState;
            CheckInitAndApply();
            RequestSerialization();
        }
        public void setState(int state)
        {
            LocalState = state;
            if (!LocalMode) SyncedState = LocalState;
            CheckInitAndApply();
        }

        public override void OnDeserialization()
        {
            base.OnDeserialization();
            if (!LocalMode) LocalState = SyncedState;
            CheckInitAndApply();
        }

#if !COMPILER_UDONSHARP && UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0, 1, 0);
            GameObject target;
            if (targets != null) return;
            for (int index = 0; index < targets.Length; index++)
            {
                target = targets[index];
                if (target)
                {
                    Gizmos.DrawLine(transform.position, target.transform.position);
                    Gizmos.DrawSphere(target.transform.position, 0.01f);
                    for (int transformStateIndex = 0; transformStateIndex < m_Target_Transforms[index].Length; transformStateIndex++)
                    {
                        Gizmos.color = new Color(1, 0, 1);
                        Gizmos.DrawLine(target.transform.position, m_Target_Transforms[index][transformStateIndex].position);
                    }
                }
            }
        }
#endif
    }
}
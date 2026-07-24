using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace frou01.util
{
    public class SyncEventLinker : UdonSharpBehaviour
    {
        [SerializeField] public UdonBehaviour[] targets;

        public override void OnDeserialization()
        {
            foreach (UdonBehaviour aTarget in targets)
            {
                aTarget.SendCustomEvent("OnDeserialization_");
            }
        }
        public override void OnOwnershipTransferred(VRCPlayerApi player)
        {
            if(player == Networking.LocalPlayer)
            {
                foreach (UdonBehaviour aTarget in targets)
                {
                    aTarget.SendCustomEvent("OnOwnershipTransferred_BecomeLocal");
                }
            }
            else
            {
                foreach (UdonBehaviour aTarget in targets)
                {
                    aTarget.SendCustomEvent("OnOwnershipTransferred_BecomeRemote");
                }
            }
        }
#if !COMPILER_UDONSHARP && UNITY_EDITOR
        void OnDrawGizmos()
        {
        }


        void OnDrawGizmosSelected()
        {
            if (targets != null)
            {
                for (int index = 0; index < targets.Length; index++)
                {
                    Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
                    Gizmos.DrawLine(transform.position, targets[index].transform.position);
                }
            }
        }
#endif
    }
}
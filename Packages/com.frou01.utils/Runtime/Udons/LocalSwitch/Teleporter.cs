using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace frou01.util
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Teleporter : UdonSharpBehaviour
    {
        public Transform target;
        public Vector3 Offset = new Vector3(0, 1, 0);

        public override void Interact()
        {
            Networking.LocalPlayer.TeleportTo(target.position + Offset, target.rotation);
        }
#if !COMPILER_UDONSHARP && UNITY_EDITOR
        void OnDrawGizmos()
        {
        }


        void OnDrawGizmosSelected()
        {
            if (target != null)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
                Gizmos.DrawLine(transform.position, target.transform.position + Offset);
            }
        }
#endif
    }
}
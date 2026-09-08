using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace frou01.util
{
    public class InteractActivator : MonoBehaviour
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
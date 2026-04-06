
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ColliderUdonCuller : UdonSharpBehaviour
{
    public GameObject playerChaser;

    [Header("If PlayerChaser enter collider, this udon activate Udons.")]
    [Header("This Array will be used for auto-set all udonbehaviour to targetUdons")]
    public GameObject[] targetGameObject;
    public UdonBehaviour[] targetUdons;
    public void OnTriggerEnter(Collider other)
    {
        if (playerChaser == other.gameObject)
        {
            foreach (UdonBehaviour targetUdon in targetUdons)
            {
                targetUdon.enabled = true;
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (playerChaser == other.gameObject)
        {
            foreach (UdonBehaviour targetUdon in targetUdons)
            {
                targetUdon.enabled = false;
            }
        }
    }
#if !COMPILER_UDONSHARP && UNITY_EDITOR
    void OnDrawGizmos()
    {
    }
    void OnDrawGizmosSelected()
    {
        if (targetGameObject != null)
        {
            for (int index = 0; index < targetGameObject.Length; index++)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
                Gizmos.DrawLine(transform.position, targetGameObject[index].transform.position);
            }
        }
        if (targetUdons != null)
        {
            for (int index = 0; index < targetUdons.Length; index++)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
                Gizmos.DrawLine(transform.position, targetUdons[index].transform.position);
            }
        }
    }
#endif
}

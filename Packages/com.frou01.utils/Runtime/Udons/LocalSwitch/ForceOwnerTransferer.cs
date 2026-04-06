
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class ForceOwnerTransferer : UdonSharpBehaviour
{
    [SerializeField]GameObject targetObject;
    [SerializeField] GameObject[] targetObjects;
    void Start()
    {
        
    }

    public override void Interact()
    {
        if(targetObject != null) Networking.SetOwner(Networking.LocalPlayer, targetObject);
        foreach(GameObject go in targetObjects)
        {
            Networking.SetOwner(Networking.LocalPlayer, go);
        }
    }
#if !COMPILER_UDONSHARP && UNITY_EDITOR
    void OnDrawGizmos()
    {
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        if (targetObjects != null)
        {
            for (int index = 0; index < targetObjects.Length; index++)
            {
                Gizmos.DrawLine(transform.position, targetObjects[index].transform.position);
            }
        }
        if (targetObject != null)
        {
            Gizmos.DrawLine(transform.position, targetObject.transform.position);
        }
    }
#endif
}

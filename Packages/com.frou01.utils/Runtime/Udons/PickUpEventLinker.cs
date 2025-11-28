
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PickUpEventLinker : UdonSharpBehaviour
{
    [SerializeField] public UdonBehaviour[] targets;

    public override void OnPickup()
    {
        foreach(UdonBehaviour aTarget in targets)
        {
            aTarget.SendCustomEvent("OnPickup_");
        }
    }
    public override void OnDrop()
    {
        foreach (UdonBehaviour aTarget in targets)
        {
            aTarget.SendCustomEvent("OnDrop_");
        }
    }
    void OnDrawGizmos()
    {
    }


    void OnDrawGizmosSelected()
    {
        if(targets != null)
        {
            for (int index = 0; index < targets.Length; index++)
            {
                Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
                Gizmos.DrawLine(transform.position, targets[index].transform.position);
            }
        }
    }
}

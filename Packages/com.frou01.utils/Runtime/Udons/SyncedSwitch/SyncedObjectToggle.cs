
using UdonSharp;
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[RequireComponent(typeof(Collider))]
public class SyncedObjectToggle : UdonSharpBehaviour
{
    [Tooltip("Object changes will be applied. If there is no toggle transform, this udon toggle active-state of GameObject.")]
    [SerializeField] GameObject[] targets;

    bool init;

    bool transformToggleMode = false;
    [Tooltip("Transform toggle targets. This udon copy position and rotation on World-Space.")]
    [SerializeField] Transform[] target_ToggleFalseTransform = new Transform[] { };
    [SerializeField] Transform[] target_ToggleTrueTransform = new Transform[] { };
    bool[] GameObjectInitialActive = new bool[0];

    [UdonSynced]bool SyncedToggleState = false;
    [Tooltip("Initial state. FALSE: keep hierarchy state. TRUE: inverse hierarchy state.")]
    [SerializeField] bool toggleState = false;
    [Tooltip("Ignore sync")]
    [SerializeField] bool LocalMode;


    protected virtual void Initialize()
    {
        if(targets.Length != target_ToggleTrueTransform.Length || target_ToggleFalseTransform.Length != target_ToggleTrueTransform.Length)
        {
            transformToggleMode = false;
            GameObjectInitialActive = new bool[targets.Length];
            for(int index = 0; index < targets.Length; index++)
            {
                GameObject target = targets[index];
                GameObjectInitialActive[index] = target.activeSelf;
            }
        }
        else
        {
            transformToggleMode = true;
        }
        ApplyToggleState();
        init = true;
    }

    protected virtual void ApplyToggleState()
    {
        for (int index = 0; index < targets.Length; index++)
        {
            GameObject target = targets[index];
            if (transformToggleMode)
            {
                if (toggleState)
                {
                    target.transform.SetPositionAndRotation(target_ToggleTrueTransform[index].position, target_ToggleTrueTransform[index].rotation);
                }
                else
                {
                    target.transform.SetPositionAndRotation(target_ToggleFalseTransform[index].position, target_ToggleFalseTransform[index].rotation);
                }
            }
            else
            {
                target.SetActive(GameObjectInitialActive[index] ^ toggleState);
            }
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
            ApplyToggleState();
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
        Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
        toggleState = !toggleState;
        if (!LocalMode) SyncedToggleState = toggleState;
        CheckInitAndApply();
        RequestSerialization();
    }
    public void setTRUE()
    {
        toggleState = true;
        if (!LocalMode) SyncedToggleState = toggleState;
        CheckInitAndApply();
    }
    public void setFALSE()
    {
        toggleState = false;
        if (!LocalMode) SyncedToggleState = toggleState;
        CheckInitAndApply();
    }
    public void setState(bool state)
    {
        toggleState = state;
        if (!LocalMode) SyncedToggleState = toggleState;
        CheckInitAndApply();
    }

    public override void OnDeserialization()
    {
        base.OnDeserialization();
        if (!LocalMode) toggleState = SyncedToggleState;
        CheckInitAndApply();
    }

    public void OnDrawGizmosSelected()
    {
        bool gizmo_transformToggleMode = true;
        if (targets.Length != target_ToggleTrueTransform.Length || target_ToggleFalseTransform.Length != target_ToggleTrueTransform.Length)
        {
            gizmo_transformToggleMode = false;
        }
        Gizmos.color = new Color(0, 1, 0);
        GameObject target;
        for (int index = 0; index < targets.Length; index++)
        {
            target = targets[index];
            if (target)
            {
                Gizmos.DrawLine(this.transform.position, target.transform.position);
                Gizmos.DrawSphere(target.transform.position, 0.01f);
                if (gizmo_transformToggleMode && target_ToggleTrueTransform[index] && target_ToggleFalseTransform[index])
                {
                    Gizmos.color = new Color(1, 0, 0);
                    Gizmos.DrawLine(target.transform.position, target_ToggleFalseTransform[index].position);
                    Gizmos.color = new Color(0, 0, 1);
                    Gizmos.DrawLine(target.transform.position, target_ToggleTrueTransform[index].position);
                }
            }
        }
    }
}

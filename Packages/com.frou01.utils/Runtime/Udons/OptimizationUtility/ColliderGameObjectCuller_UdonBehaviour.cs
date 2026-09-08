using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace frou01.util
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    [RequireComponent(typeof(ColliderGameObjectCuller))]
    public class ColliderGameObjectCuller_UdonBehaviour : UdonSharpBehaviour
    {
        [HideInInspector] public GameObject[] objects;

        public void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerChaser>() != null)
            {
                foreach (GameObject go in objects)
                {
                    go.SetActive(true);
                }
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerChaser>() != null)
            {
                foreach (GameObject go in objects)
                {
                    go.SetActive(false);
                }
            }
        }
    }
}
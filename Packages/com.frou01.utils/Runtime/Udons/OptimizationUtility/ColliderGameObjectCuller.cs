using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace frou01.util
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    [RequireComponent(typeof(Collider))]
    public class ColliderGameObjectCuller : UdonSharpBehaviour
    {
        [Header("If PlayerChaser enter collider, this udon activate assigned GameObjects.")]
        public GameObject[] objects;
        public bool isStaticMode;

        public void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerChaser>() != null)
            {
                foreach (GameObject go in objects)
                {
                    if(go != null) go.SetActive(true);
                }
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerChaser>() != null)
            {
                foreach (GameObject go in objects)
                {
                    if (go != null) go.SetActive(false);
                }
            }
        }
    }
}
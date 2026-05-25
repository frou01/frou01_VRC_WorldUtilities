using UdonSharp;
using UnityEngine;

namespace frou01.util
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ObjectSwitch : UdonSharpBehaviour
    {
        public GameObject targetObj;
        public GameObject[] targetObjs;
        void Start()
        {

        }
        public override void Interact()
        {
            if (targetObj != null) targetObj.SetActive(!targetObj.activeSelf);
            foreach (GameObject obj in targetObjs)
            {
                if (obj != null)
                {
                    obj.SetActive(!obj.activeSelf);
                }
            }
        }
    }
}
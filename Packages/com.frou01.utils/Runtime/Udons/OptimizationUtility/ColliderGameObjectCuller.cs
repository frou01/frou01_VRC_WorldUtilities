using UnityEngine;
using VRC.Udon;

namespace frou01.util
{
    [RequireComponent(typeof(Collider))]
    public class ColliderGameObjectCuller : MonoBehaviour
    {
        //TODO UdonBehaviourを通常使わず、EditorScriptで設定を引き継いで代わりのUdonBehaviourを付けるようにする。VRCはこんなもんにまでNetworkIDを振りやがるので。
        [Header("If PlayerChaser enter collider, this udon activate assigned GameObjects.")]
        public GameObject[] objects;
        [Header("MoveableStatic Batching. \nIf object.name include \"instanced\", ignored.")]
        public bool isStaticMode;
        void OnTriggerEnter(Collider other)
        {
            this.GetComponent<ColliderGameObjectCuller_UdonBehaviour>().OnTriggerEnter(other);
        }
        void OnTriggerExit(Collider other)
        {
            this.GetComponent<ColliderGameObjectCuller_UdonBehaviour>().OnTriggerExit(other);
        }
    }
}
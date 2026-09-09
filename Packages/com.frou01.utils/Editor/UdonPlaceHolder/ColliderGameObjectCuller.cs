using frou01.util.editor;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UdonSharp;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using VRC.Udon;

namespace frou01.util.placeholder
{
    [RequireComponent(typeof(Collider))]
    public class ColliderGameObjectCuller : MonoBehaviour, IUdonPlaceHolder
    {
        string pattern = @"^(?=.*instanced).*$";//部分一致 instanced
        //TODO UdonBehaviourを通常使わず、EditorScriptで設定を引き継いで代わりのUdonBehaviourを付けるようにする。VRCはこんなもんにまでNetworkIDを振りやがるので。
        [Header("If PlayerChaser enter collider, this udon activate assigned GameObjects.")]
        [FormerlySerializedAs("objects")]
        public GameObject[] objects;
        [Header("MoveableStatic Batching. \nIf object.name include \"instanced\", ignored.")]
        public bool isStaticMode;

        [HideInInspector][SerializeField]ColliderGameObjectCuller_UdonBehaviour CGCUB;

        public void InstantiationUdon()
        {
            CGCUB = this.gameObject.AddUdonSharpComponentAlignSync<ColliderGameObjectCuller_UdonBehaviour>();
            //Debug.Log("SetUp " + this.name);
            foreach (GameObject go in this.objects)
            {
                if (go == null)
                {
                    Debug.LogError("Culler array has missing : " + GetPath(this.transform), this);
                }
                else
                {
                    if (this.gameObject.activeInHierarchy) go.SetActive(false);
                }
            }
            CGCUB.objects = this.objects.Where(val => val != null).ToArray();
            if (this.isStaticMode)
            {
                List<GameObject> staticmeshes = new List<GameObject>();
                foreach (GameObject go in CGCUB.objects)
                {
                    bool isinstanced = Regex.IsMatch(go.name, pattern);
                    if (!isinstanced)
                    {
                        staticmeshes.Add(go);
                    }
                }
                StaticBatchingUtility.Combine(staticmeshes.ToArray(), null);
            }

#if !COMPILER_UDONSHARP && UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                GameObject separating = Instantiate(this.gameObject, this.transform.parent,true);
                foreach (Transform child in separating.transform)
                {
                    Destroy(child.gameObject);
                }
                foreach (UdonSharpBehaviour udonsharpbh in separating.GetComponents<UdonSharpBehaviour>())
                {
                    Destroy(udonsharpbh);
                }
                foreach (UdonBehaviour udon in separating.GetComponents<UdonBehaviour>())
                {
                    Destroy(udon);
                }
            }
#endif
        }
        private string GetPath(Transform t)
        {
            string path = t.name;
            while (t.parent)
            {
                path = t.parent.name + "/" + path;
                t = t.parent;
            }
            return path;
        }

        void OnTriggerEnter(Collider other)
        {
            CGCUB.OnTriggerEnter(other);
        }
        void OnTriggerExit(Collider other)
        {
            CGCUB.OnTriggerExit(other);
        }
    }
}
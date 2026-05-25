using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace frou01.util
{
    public class GPUInstancer : MonoBehaviour
    {
        [SerializeField] MeshRenderer[] tryToInstancing;
        public void ProceedInstancing()
        {
            if (tryToInstancing == null || tryToInstancing.Length == 0) return;
            if (tryToInstancing[0] == null) return;
            MeshFilter instancedFilter = tryToInstancing[0].GetComponent<MeshFilter>();
            if (!instancedFilter)return;
            foreach (MeshRenderer renderer in tryToInstancing)
            {
                if(renderer == null) continue;
                MeshFilter instancingFilter = renderer.GetComponent<MeshFilter>();
                instancingFilter.sharedMesh = instancedFilter.sharedMesh;
            }
        }
    }
}

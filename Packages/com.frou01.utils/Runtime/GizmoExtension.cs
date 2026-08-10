using UnityEditor;
using UnityEngine;

namespace frou01.util
{
    public class GizmoExtension : MonoBehaviour
    {
#if !COMPILER_UDONSHARP && UNITY_EDITOR
        public static Vector3 getCenter(Transform transform)
        {
            MeshRenderer[] meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
            if (meshRenderers.Length > 0)
            {
                Vector3 center = Vector3.zero;
                foreach (MeshRenderer renderer in meshRenderers)
                {
                    center += renderer.bounds.center / meshRenderers.Length;
                }
                return center;
            }
            else
            {
                return transform.position;
            }
        }

        public static void DrawArrow(Vector3 start, Vector3 end, float size_len,float size_wide)
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            Gizmos.DrawLine(start, end);
            float length = (end - start).magnitude;
            Vector3 dir = (end - start).normalized;
            Vector3 look = sceneView.rotation * Vector3.forward;
            Vector3 wide = Vector3.Cross(dir, look).normalized;

            Gizmos.DrawLine(end - dir * length * size_len + wide * length * size_wide, end);
            Gizmos.DrawLine(end - dir * length * size_len - wide * length * size_wide, end);
        }
#endif
    }
}



#if (UNITY_EDITOR) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using VRC.Udon;

namespace frou01.util
{
    public class Revert_SerializedUdonProgramSource : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }


        static string path = "serializedProgramAsset";
        public void Revert()
        {
            GameObject[] rootObjects =
            gameObject.scene.GetRootGameObjects();

            foreach (GameObject anrootObject in rootObjects)
            {
                limitter = 0;
                debugString = string.Empty;
                Proceed(anrootObject.transform);
                Debug.Log(debugString, anrootObject);
            }
        }

        static int limitter = 0;
        static string debugString;
        static string indent;
        public static void Proceed(Transform parent)
        {
            limitter++;
            if (limitter > 10000)
            {
                Debug.LogError(debugString, parent);
                return;
            }
            foreach (UdonBehaviour anUdon in parent.gameObject.GetComponentsInChildren<UdonBehaviour>(true))
            {
                indent = string.Empty;
                revertOverride(anUdon);
                if (PrefabUtility.IsPartOfPrefabInstance(anUdon))
                {
                    GameObject nextprefabObject = AssetDatabase.LoadAssetAtPath(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(anUdon), typeof(GameObject)) as GameObject;
                    GameObject currentprefabObject;
                    if (nextprefabObject)
                    {
                        do
                        {
                            currentprefabObject = nextprefabObject;
                            indent += "    ";
                            debugString += string.Format($"\n{indent}PrefabAssets({currentprefabObject.name})\n");
                            foreach (UdonBehaviour prefabUdon in currentprefabObject.gameObject.GetComponentsInChildren<UdonBehaviour>(true))
                            {
                                revertOverride(prefabUdon);
                            }
                            nextprefabObject = getNextPrefab(currentprefabObject);
                        } while (nextprefabObject && currentprefabObject != nextprefabObject);
                    }
                }
            }
            debugString += "\n";
        }

        public static GameObject getNextPrefab(GameObject currentprefabObject)
        {
            GameObject nextprefabObject = AssetDatabase.LoadAssetAtPath(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(currentprefabObject), typeof(GameObject)) as GameObject;
            if(nextprefabObject == currentprefabObject)
            {
                nextprefabObject = PrefabUtility.GetCorrespondingObjectFromSource(currentprefabObject);
            }
            return nextprefabObject;
        }

        public static void revertOverride(UdonBehaviour targetUdon)
        {
            if (PrefabUtility.IsPartOfPrefabInstance(targetUdon))
            {
                var serializedObject = new SerializedObject(targetUdon);
                SerializedProperty anProperty = serializedObject.FindProperty(path);
                if (anProperty != null && anProperty.prefabOverride)
                {
                    debugString += indent;
                    debugString += GetHierarchyPath(targetUdon.gameObject);
                    debugString += "    Revert\n";
                    PrefabUtility.RevertPropertyOverride(anProperty, InteractionMode.AutomatedAction);
                }
            }
            else
            {
                if(targetUdon.ProgramId != targetUdon.programSource.SerializedProgramAsset.GetInstanceID())
                {
                    debugString += indent;
                    debugString += GetHierarchyPath(targetUdon.gameObject);
                    debugString += "    Re-Assign\n";
                    targetUdon.AssignProgramAndVariables(targetUdon.programSource.SerializedProgramAsset, targetUdon.publicVariables);
                }
            }
        }

        //from: https://qiita.com/Milcia/items/d6412d4c07cc2e9ffe5a
        private static string GetHierarchyPath(GameObject targetObj)
        {
            List<GameObject> objPath = new List<GameObject>();
            objPath.Add(targetObj);
            for (int i = 0; objPath[i].transform.parent != null; i++)
                objPath.Add(objPath[i].transform.parent.gameObject);
            string path = objPath[objPath.Count - 1].gameObject.name;
            for (int i = objPath.Count - 2; i >= 0; i--)
                path += "/" + objPath[i].gameObject.name;

            return path;
        }

        [MenuItem("GameObject/Revert serializedProgramAsset Override", false, 0)]
        public static void Hierarchy_Revert(MenuCommand command)
        {
            GameObject[] selectObjects = Selection.gameObjects;
            foreach (GameObject anrootObject in selectObjects)
            {
                limitter = 0;
                debugString = string.Empty;
                Proceed(anrootObject.transform);
                Debug.Log(debugString, anrootObject);
            }
        }
        [MenuItem("Assets/Revert serializedProgramAsset Override", false, 0)]
        public static void Assets_Revert(MenuCommand command)
        {
            string[] selectObjects = Selection.assetGUIDs;
            foreach (string anGUID in selectObjects)
            {
                Debug.Log("selectedGUID   " + anGUID);
                GameObject target = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(anGUID), typeof(GameObject)) as GameObject;
                Debug.Log("selectedPrefab " + target, target);
                if (target)
                {
                    limitter = 0;
                    debugString = string.Empty;
                    Proceed(target.transform);
                    Debug.Log(debugString, target);
                }
            }
        }
    }
}


#endif
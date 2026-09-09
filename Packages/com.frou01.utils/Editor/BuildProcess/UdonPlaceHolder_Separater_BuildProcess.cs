using frou01.util.placeholder;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace frou01.util.editor
{
    public class UdonPlaceHolder_Separater_BuildProcess : IProcessSceneWithReport
    {
        public int callbackOrder => -1000;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            List<IUdonPlaceHolder> PlaceHolders = new List<IUdonPlaceHolder>();
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                PlaceHolders.AddRange(obj.GetComponentsInChildren<IUdonPlaceHolder>(true));
            }

            foreach (IUdonPlaceHolder placeholder in PlaceHolders)
            {
                placeholder.InstantiationUdon();
            }
        }
    }
}

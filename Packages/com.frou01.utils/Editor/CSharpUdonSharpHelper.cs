using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UdonSharp;
using UdonSharpEditor;
using UnityEngine;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using static VRC.SDKBase.Networking;

namespace frou01.util.editor
{
    public static class CSharpUdonSharpHelper
    {

        public static bool HasSyncVariable(UdonBehaviour udon)
        {
            if (udon.SyncMethod != VRC.SDKBase.Networking.SyncType.None)
            {
                var type = udon.GetType();
                FieldInfo memberinfo = type.GetField("serializedProgramAsset",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                IUdonProgram _program = ((AbstractSerializedUdonProgramAsset)memberinfo.GetValue(udon))?.RetrieveProgram();
                if (_program != null && _program.SyncMetadataTable != null)
                {
                    IEnumerable<IUdonSyncMetadata> SyncMetadatas = _program.SyncMetadataTable.GetAllSyncMetadata();
                    foreach (IUdonSyncMetadata metas in SyncMetadatas)
                    {
                        return true;
                    }
                }
                else { Debug.Log("fail get SyncMetadataTable"); }
            }
            return false;
        }

        #region AddComponent
        [PublicAPI]
        public static T AddUdonSharpComponentAlignSync<T>(this GameObject gameObject) where T : UdonSharpBehaviour =>
            (T)AddUdonSharpComponentAlignSync(gameObject, SyncType.None, typeof(T));
        [PublicAPI]
        public static T AddUdonSharpComponentAlignSync<T>(this GameObject gameObject, SyncType DefaultSyncType) where T : UdonSharpBehaviour =>
            (T)AddUdonSharpComponentAlignSync(gameObject, DefaultSyncType, typeof(T));
        public static UdonSharpBehaviour AddUdonSharpComponentAlignSync(this GameObject gameObject, SyncType DefaultSyncType, Type type)
        {
            UdonBehaviour multiAttachedUdonBehaviour = gameObject.GetComponent<UdonBehaviour>();

            UdonSharpBehaviour newSharpBeh = UdonSharpComponentExtensions.AddUdonSharpComponent(gameObject, type);

            if (multiAttachedUdonBehaviour)
            {
                UdonSharpEditorUtility.GetBackingUdonBehaviour(newSharpBeh).SyncMethod = multiAttachedUdonBehaviour.SyncMethod;
            }
            else
            {
                UdonSharpEditorUtility.GetBackingUdonBehaviour(newSharpBeh).SyncMethod = DefaultSyncType;
            }
            return newSharpBeh;
        }
        #endregion
    }
}

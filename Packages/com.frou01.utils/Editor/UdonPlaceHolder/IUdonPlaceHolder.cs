using System.Collections;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;

namespace frou01.util.placeholder
{
    public interface IUdonPlaceHolder
    {
#if !COMPILER_UDONSHARP && UNITY_EDITOR
        public void InstantiationUdon();
#endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiProduction.SceneContainer
{
    [System.Serializable]
    public struct Container<T>
    {
        [SceneSelector]
        public string Scene;
        public T Data;
    } 
}
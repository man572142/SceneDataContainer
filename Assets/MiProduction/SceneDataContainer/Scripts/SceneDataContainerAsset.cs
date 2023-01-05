using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

namespace MiProduction.SceneContainer
{
    public abstract class SceneDataContainerAsset<T> : ScriptableObject
    {
        public Container<T>[] SceneContainers;

        public bool TryGetSceneData(out T data)
        {
            try
            {
                data = (T)GetSceneData();
                return true;
            }
            catch (InvalidCastException)
            {
                data = default;
                return false;
            }
        }

        public object GetSceneData()
        {
            return SceneContainers.Where(x => x.Scene == SceneManager.GetActiveScene().path).Select(x => x.Data).First();
        }

        
        
    } 
}

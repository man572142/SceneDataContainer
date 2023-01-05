using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiProduction.SceneContainer
{
    [CustomEditor(typeof(SceneDataContainerAsset<>),true)]
    public class SceneDataContainerAssetEditor : Editor
    {
        private void Awake()
        {
            SerializedProperty sceneConfigs = serializedObject.FindProperty("SceneContainers");
            if (sceneConfigs.isArray && sceneConfigs.arraySize == 0)
            {
                
                sceneConfigs.arraySize = SceneManager.sceneCountInBuildSettings;
                for (int i = 0; i < sceneConfigs.arraySize; i++)
                {
                    sceneConfigs.GetArrayElementAtIndex(i).FindPropertyRelative("Scene").stringValue = EditorBuildSettings.scenes[i].path;
                }
                serializedObject.ApplyModifiedProperties();
            }
            
        }
    }

}
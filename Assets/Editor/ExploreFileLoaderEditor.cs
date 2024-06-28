using System.Collections;
using System.Collections.Generic;
using Explore;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ExploreFileLoader))]
public class ExploreFileLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ExploreFileLoader loader = (ExploreFileLoader)target;
        if (GUILayout.Button("載入檔案"))
        {
            loader.Load();
        }
    }
}

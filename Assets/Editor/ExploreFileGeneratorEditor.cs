using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Explore;

[CustomEditor(typeof(ExploreFileGenerator))]
public class ExploreFileGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ExploreFileGenerator exploreFileGenerator = (ExploreFileGenerator)target;
        if (GUILayout.Button("建立檔案"))
        {
            exploreFileGenerator.BuildFile();
        }
        if (GUILayout.Button("載入檔案"))
        {
            //exploreFileGenerator.LoadFile();
        }
    }
}

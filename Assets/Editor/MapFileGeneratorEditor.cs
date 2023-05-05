using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapFileGenerator))]
public class MapFileGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapFileGenerator nodeGenerator = (MapFileGenerator)target;
        if (GUILayout.Button("建立檔案"))
        {
            nodeGenerator.BuildFile();
        }
        if (GUILayout.Button("載入檔案"))
        {
            nodeGenerator.LoadFile();
        }
    }
}

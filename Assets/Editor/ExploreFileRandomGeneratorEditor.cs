using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ExploreFileRandomGenerator))]
public class ExploreFileRandomGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ExploreFileRandomGenerator exploreFileRandomGenerator = (ExploreFileRandomGenerator)target;
        if (GUILayout.Button("輸出檔案"))
        {
            exploreFileRandomGenerator.BuildFile();
        }
    }
}

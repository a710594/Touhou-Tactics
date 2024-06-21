using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleSeedFileGenerator))]
public class BattleSeedFileGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BattleSeedFileGenerator generator = (BattleSeedFileGenerator)target;
        if (GUILayout.Button("建立檔案"))
        {
            generator.Generate();
        }
    }
}

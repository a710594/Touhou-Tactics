using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleFileLoader))]
public class BattleFileLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BattleFileLoader loader = (BattleFileLoader)target;
        if (GUILayout.Button("載入檔案"))
        {
            loader.Load();
        }
    }
}

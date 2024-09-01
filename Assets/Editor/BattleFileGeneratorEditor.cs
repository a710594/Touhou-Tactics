using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Battle
{
    [CustomEditor(typeof(BattleFileGenerator))]
    public class MapFileGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            BattleFileGenerator nodeGenerator = (BattleFileGenerator)target;
            if (GUILayout.Button("建立檔案"))
            {
                nodeGenerator.BuildFile();
            }
        }
    }
}
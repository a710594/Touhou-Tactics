using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Battle
{
    [CustomEditor(typeof(BattleFileRandomGenerator))]
    public class BattleFileRandomGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            BattleFileRandomGenerator nodeGenerator = (BattleFileRandomGenerator)target;
            if (GUILayout.Button("«Ø¥ßÀÉ®×"))
            {
                nodeGenerator.BuildFile();
            }
        }
    }
}
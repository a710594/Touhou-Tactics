using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Battle
{
    [CustomEditor(typeof(BattleFileFixedGenerator))]
    public class MapFileFixedGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            BattleFileFixedGenerator nodeGenerator = (BattleFileFixedGenerator)target;
            if (GUILayout.Button("«Ø¥ßÀÉ®×"))
            {
                nodeGenerator.BuildFile();
            }
        }
    }
}

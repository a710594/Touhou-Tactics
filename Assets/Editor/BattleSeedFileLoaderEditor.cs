using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Battle
{
    [CustomEditor(typeof(BattleSeedFileLoader))]
    public class BattleSeedFileLoaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            BattleSeedFileLoader loader = (BattleSeedFileLoader)target;
            if (GUILayout.Button("載入檔案"))
            {
                loader.Load();
            }
        }
    }
}
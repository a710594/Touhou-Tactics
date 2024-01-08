using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleFileGenerator))]
public class MapFileGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BattleFileGenerator nodeGenerator = (BattleFileGenerator)target;
        if (GUILayout.Button("�إ��ɮ�"))
        {
            nodeGenerator.BuildFile();
        }
        if (GUILayout.Button("���J�ɮ�"))
        {
            nodeGenerator.LoadFile();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BattleQuickStarter))]
public class BattleQuickStarterEditor : Editor
{
    public override void OnInspectorGUI() 
    {
        //serializedObject.Update();

        BattleQuickStarter battleQuickStarter = (BattleQuickStarter)target;

        SerializedProperty currentMode = serializedObject.FindProperty("CurrentMode");
        EditorGUILayout.PropertyField(currentMode);

        SerializedProperty fileManager = serializedObject.FindProperty("FileManager");
        EditorGUILayout.PropertyField(fileManager);

        if (battleQuickStarter.CurrentMode == BattleQuickStarter.ModeEnum.Fixed)
        {
            SerializedProperty map = serializedObject.FindProperty("Map");
            EditorGUILayout.PropertyField(map);

            SerializedProperty tutorial = serializedObject.FindProperty("Tutorial");
            EditorGUILayout.PropertyField(tutorial);
        }
        else if (battleQuickStarter.CurrentMode == BattleQuickStarter.ModeEnum.Random)
        {
            SerializedProperty enemyGroupId = serializedObject.FindProperty("EnemyGroupId");
            EditorGUILayout.PropertyField(enemyGroupId);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

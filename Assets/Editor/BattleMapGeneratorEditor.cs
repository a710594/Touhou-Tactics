using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleMapBuilder))]
public class BattleMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BattleMapBuilder nodeGenerator = (BattleMapBuilder)target;
        if (GUILayout.Button("���ͦa��"))
        {
            nodeGenerator.Generate(out BattleMapInfo battleInfo);
            //BattleController.Instance.Init(tileComponentDic, tileInfoDic, attachDic, noAttachList);
        
        }
    }
}

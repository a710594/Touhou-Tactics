using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleMapGenerator))]
public class BattleMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BattleMapGenerator nodeGenerator = (BattleMapGenerator)target;
        if (GUILayout.Button("產生地圖"))
        {
            nodeGenerator.Generate(out BattleMapInfo battleInfo);
            //BattleController.Instance.Init(tileComponentDic, tileInfoDic, attachDic, noAttachList);
        
        }
    }
}

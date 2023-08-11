using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RandomMapGenerator))]
public class RandomMapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RandomMapGenerator nodeGenerator = (RandomMapGenerator)target;
        if (GUILayout.Button("���ͦa��"))
        {
            nodeGenerator.Generate(out BattleInfo battleInfo);
            //BattleController.Instance.Init(tileComponentDic, tileInfoDic, attachDic, noAttachList);
        
        }
    }
}

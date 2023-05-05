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
        if (GUILayout.Button("產生地圖"))
        {
            nodeGenerator.Generate(out Dictionary<Vector3, TileComponent> tileComponentDic, out Dictionary<Vector3, TileInfo> tileInfoDic, out Dictionary<Vector3, GameObject> attachDic, out List<Vector3> noAttachList);
            //BattleController.Instance.Init(tileComponentDic, tileInfoDic, attachDic, noAttachList);
        
        }
    }
}

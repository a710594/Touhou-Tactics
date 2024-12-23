using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Explore;

[CustomEditor(typeof(ExploreMapBuilder))]
public class ExploreMapBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ExploreMapBuilder builder = (ExploreMapBuilder)target;
        if (GUILayout.Button("¿é¥XÀÉ®×"))
        {
            RandomFloorModel data = DataContext.Instance.RandomFloorDic[builder.Floor];
            ExploreFile file = ExploreFileRandomGenerator.Instance.Create(data, builder.Seed);
            builder.GetRandom(file, out Dictionary<Vector2Int, ExploreFileTile> tileDic);
        }
    }
}

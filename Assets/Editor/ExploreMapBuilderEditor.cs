using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Explore;

/*[CustomEditor(typeof(ExploreMapBuilder))]
public class ExploreMapBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ExploreMapBuilder builder = (ExploreMapBuilder)target;
        if (GUILayout.Button("載入檔案")) 
        {
            builder.FileManager.Init();
            InputMamager.Instance.Init();
            DataTable.Instance.SetFileManager(builder.FileManager);
            SaveManager.Instance.SetFileManager(builder.FileManager);
            DataTable.Instance.Load(null);
        }
        if (GUILayout.Button("產生地圖"))
        {
            RandomFloorModel data = DataTable.Instance.RandomFloorDic[builder.Floor];
            ExploreFile file = ExploreFileRandomGenerator.Instance.Create(data, builder.Seed);
            builder.GetRandom(file, out Dictionary<Vector2Int, ExploreFileTile> tileDic);
        }
    }

    private void Awake()
    {
        DataTable.Instance.Load(null);
    }
}*/

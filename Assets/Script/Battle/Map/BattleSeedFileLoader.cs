using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class BattleSeedFileLoader : MonoBehaviour
{
    public string FileName;
    public BattleSeedFileGenerator Generator;
    public Transform Tilemap;
    public Transform EnemyGroup;

    public void Load() 
    {
        string path = Application.streamingAssetsPath + "/MapSeed/" + FileName + ".txt";

        DataContext.Instance.Init();
        BattleFileReader reader = new BattleFileReader();
        BattleInfo info = reader.Read(path);

        for (int i = Tilemap.childCount; i > 0; --i)
        {
            DestroyImmediate(Tilemap.GetChild(0).gameObject);
        }

        for (int i = EnemyGroup.childCount; i > 0; --i)
        {
            DestroyImmediate(EnemyGroup.GetChild(0).gameObject);
        }

        GameObject obj;
        int count = 0;
        Transform[] noAttach = new Transform[info.NoAttachList.Count];
        foreach (KeyValuePair<Vector2Int, TileAttachInfo> pair in info.TileAttachInfoDic)
        {
            obj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + pair.Value.TileID), Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(Tilemap);
            obj.transform.position = new Vector3(pair.Key.x, 0, pair.Key.y);
            if (info.NoAttachList.Contains(pair.Key))
            {
                noAttach[count] = obj.transform;
                count++;
            }

            Generator.FileName = FileName;
            Generator.NeedCount = info.NeedCount;
            Generator.MustBeEqualToNeedCount = info.MustBeEqualToNeedCount;
            Generator.Exp = info.Exp;
            Generator.NoAttach = noAttach;
        }

        BattleMapEnemy battleMapEnemy;
        foreach (KeyValuePair<Vector3Int, int> pair in info.EnemyDic)
        {
            obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Other/BattleMapEnemy"), Vector3.zero, Quaternion.identity);
            battleMapEnemy = obj.GetComponent<BattleMapEnemy>();
            battleMapEnemy.Init(pair.Value);
            battleMapEnemy.transform.SetParent(EnemyGroup);
            battleMapEnemy.transform.position = pair.Key;
        }
    }
}

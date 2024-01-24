using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BattleFileGenerator : MonoBehaviour
{
    public bool IsTutorial;
    public bool IsSeed;
    public string FileName;
    public int PlayerCount;
    public int Exp;
    public Transform Tilemap;
    public Transform EnemyGroup;
    public Transform[] NoAttach;

    private bool _isInit = false;
    private string _prePath;

    public void BuildFile() 
    {
        TileComponent component;
        List<string[]> tileList = new List<string[]>();
        foreach (Transform child in Tilemap)
        {
            component = child.GetComponent<TileComponent>();
            tileList.Add(new string[3] { child.position.x.ToString(), child.position.z.ToString(), component.ID });
        }

        List<int[]> noAttachList = new List<int[]>(); //禁建區,不會有附加物件,放置玩家角色的區域
        for (int i=0; i<NoAttach.Length; i++) 
        {
            noAttachList.Add(new int[2] { (int)NoAttach[i].position.x, (int)NoAttach[i].position.z });
        }

        BattleMapEnemy battleMapEnemy;
        List<int[]> enemyList = new List<int[]>(); //敵人的位置和ID
        foreach (Transform child in EnemyGroup) 
        {
            battleMapEnemy = child.GetComponent<BattleMapEnemy>();
            enemyList.Add(new int[4] { (int)child.position.x, (int)child.position.y, (int)child.position.z , battleMapEnemy.ID});
        }

        string path = GetPath();
        BattleFile battleFile = new BattleFile();
        battleFile.IsTutorial = IsTutorial;
        battleFile.PlayerCount = PlayerCount;
        battleFile.Exp = Exp;
        battleFile.TileList = tileList;
        battleFile.NoAttachList = noAttachList;
        battleFile.EnemyList = enemyList;
        File.WriteAllText(path, JsonConvert.SerializeObject(battleFile));
    }

    public void LoadFile() 
    {
        NewLoad();
        //OldLoad();
    }

    private void NewLoad() 
    {
        string path = GetPath();

        DataContext.Instance.Init();

        BattleFileReader reader = new BattleFileReader();
        BattleInfo battleInfo = reader.Read(path);

        IsTutorial = battleInfo.IsTutorial;
        PlayerCount = battleInfo.PlayerCount;
        Exp = battleInfo.Exp;

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
        NoAttach = new Transform[battleInfo.NoAttachList.Count];
        foreach (KeyValuePair<Vector2Int, TileAttachInfo> pair in battleInfo.TileAttachInfoDic)
        {
            obj = (GameObject)GameObject.Instantiate(Resources.Load("Tile/" + pair.Value.TileID), Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(Tilemap);
            obj.transform.position = new Vector3(pair.Key.x, 0, pair.Key.y);
            if (battleInfo.NoAttachList.Contains(pair.Key))
            {
                NoAttach[count] = obj.transform;
                count++;
            }
        }

        BattleMapEnemy battleMapEnemy;
        foreach (KeyValuePair<Vector3Int, int> pair in battleInfo.EnemyDic)
        {
            obj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Other/BattleMapEnemy"), Vector3.zero, Quaternion.identity);
            battleMapEnemy = obj.GetComponent<BattleMapEnemy>();
            battleMapEnemy.Init(pair.Value);
            battleMapEnemy.transform.SetParent(EnemyGroup);
            battleMapEnemy.transform.position = pair.Key;
        }
    }

    private string GetPath() 
    {
        _prePath = Application.streamingAssetsPath;
        if (IsSeed)
        {
            _prePath += "/MapSeed";
        }
        else
        {
            _prePath += "/Map/Battle";
        }

        string path = Path.Combine(_prePath, FileName + ".txt");

        return path;
    }
}

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BattleFileGenerator : MonoBehaviour
{
    //public bool IsSeed;
    public string FileName;
    public int NeedCount;
    public bool MustBeEqualToNeedCount;
    public int Exp;
    public Transform Tilemap;
    public Transform EnemyGroup;
    public Transform[] NoAttach;

    private bool _isInit = false;
    private string _prePath;

    public void BuildFile() 
    {
        int minX = int.MaxValue;
        int maxX = int.MinValue;
        int minY = int.MaxValue;
        int maxY = int.MinValue;
        Vector3 position;
        TileComponent component;
        List<string[]> tileList = new List<string[]>();
        foreach (Transform child in Tilemap)
        {
            component = child.GetComponent<TileComponent>();
            position = child.position;
            if(position.x < minX)
            {
                minX = Mathf.RoundToInt(position.x);
            }
            if(position.x > maxX) 
            {
                maxX = Mathf.RoundToInt(position.x);
            }
            if (position.z < minY)
            {
                minY = Mathf.RoundToInt(position.z);
            }
            if (position.z > maxY)
            {
                maxY = Mathf.RoundToInt(position.z);
            }
            tileList.Add(new string[3] { Mathf.RoundToInt(child.position.x).ToString(), Mathf.RoundToInt(child.position.z).ToString(), component.ID });
        }

        List<int[]> noAttachList = new List<int[]>(); //禁建區,不會有附加物件,放置玩家角色的區域
        for (int i=0; i<NoAttach.Length; i++) 
        {
            noAttachList.Add(new int[2] { Mathf.RoundToInt(NoAttach[i].position.x), Mathf.RoundToInt(NoAttach[i].position.z) });
        }

        BattleMapEnemy battleMapEnemy;
        List<int[]> enemyList = new List<int[]>(); //敵人的位置和ID
        foreach (Transform child in EnemyGroup) 
        {
            battleMapEnemy = child.GetComponent<BattleMapEnemy>();
            enemyList.Add(new int[4] { Mathf.RoundToInt(child.position.x), Mathf.RoundToInt(child.position.y), Mathf.RoundToInt(child.position.z) , battleMapEnemy.ID});
        }

        string path = Application.streamingAssetsPath + "/Map/Battle/" + FileName + ".txt";
        BattleFile battleFile = new BattleFile();
        battleFile.PlayerCount = NeedCount;
        battleFile.MustBeEqualToNeedCount = MustBeEqualToNeedCount;
        battleFile.Exp = Exp;
        battleFile.TileList = tileList;
        battleFile.NoAttachList = noAttachList;
        battleFile.EnemyList = enemyList;
        battleFile.MinX = minX;
        battleFile.MaxX = maxX;
        battleFile.MinY = minY;
        battleFile.MaxY = maxY;
        File.WriteAllText(path, JsonConvert.SerializeObject(battleFile));
    }
}

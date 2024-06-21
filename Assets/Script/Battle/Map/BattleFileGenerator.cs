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
        TileComponent component;
        List<string[]> tileList = new List<string[]>();
        foreach (Transform child in Tilemap)
        {
            component = child.GetComponent<TileComponent>();
            tileList.Add(new string[3] { Mathf.RoundToInt(child.position.x).ToString(), Mathf.RoundToInt(child.position.z).ToString(), component.ID });
        }

        List<int[]> noAttachList = new List<int[]>(); //�T�ذ�,���|�����[����,��m���a���⪺�ϰ�
        for (int i=0; i<NoAttach.Length; i++) 
        {
            noAttachList.Add(new int[2] { Mathf.RoundToInt(NoAttach[i].position.x), Mathf.RoundToInt(NoAttach[i].position.z) });
        }

        BattleMapEnemy battleMapEnemy;
        List<int[]> enemyList = new List<int[]>(); //�ĤH����m�MID
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
        File.WriteAllText(path, JsonConvert.SerializeObject(battleFile));
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BattleFileReader
{
    public BattleInfo Read(string path) 
    {
        string jsonString = File.ReadAllText(path);
        BattleFile file = JsonConvert.DeserializeObject<BattleFile>(jsonString);
        BattleInfo battleInfo = new BattleInfo();

        battleInfo.MinX = file.MinX;
        battleInfo.MaxX = file.MaxX;
        battleInfo.MinY = file.MinY;
        battleInfo.MinY = file.MinY;
        battleInfo.NeedCount = file.PlayerCount;
        battleInfo.MustBeEqualToNeedCount = file.MustBeEqualToNeedCount;
        battleInfo.Exp = file.Exp;

        int x;
        int y;
        for(int i=0; i<file.TileList.Count; i++)
        {
            x = int.Parse(file.TileList[i][0]);
            y = int.Parse(file.TileList[i][1]);
            battleInfo.TileAttachInfoDic.Add(new Vector2Int(x, y), new TileAttachInfo(file.TileList[i][2])); ;
        }

        for (int i=0; i<file.NoAttachList.Count; i++) 
        {
            battleInfo.NoAttachList.Add(new Vector2Int(file.NoAttachList[i][0], file.NoAttachList[i][1]));
        }

        for (int i=0; i<file.EnemyList.Count; i++) 
        {
            battleInfo.EnemyDic.Add(new Vector3Int(file.EnemyList[i][0], file.EnemyList[i][1], file.EnemyList[i][2]), file.EnemyList[i][3]);
        }

        return battleInfo;
    }
}


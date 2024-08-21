using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace Battle
{
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
            battleInfo.MaxY = file.MaxY;
            battleInfo.NeedCount = file.PlayerCount;
            battleInfo.MustBeEqualToNeedCount = file.MustBeEqualToNeedCount;
            battleInfo.Exp = file.Exp;

            int x;
            int y;
            for (int i = 0; i < file.TileList.Count; i++)
            {
                x = int.Parse(file.TileList[i][0]);
                y = int.Parse(file.TileList[i][1]);
                battleInfo.TileAttachInfoDic.Add(new Vector2Int(x, y), new TileAttachInfo(file.TileList[i][2])); ;
            }

            for (int i = 0; i < file.NoAttachList.Count; i++)
            {
                battleInfo.PlayerPositionList.Add(new Vector2Int(file.NoAttachList[i][0], file.NoAttachList[i][1]));
            }

            Vector3 enemyPosition;
            EnemyModel enemyData;
            BattleCharacterInfo battleCharacterInfo;
            for (int i = 0; i < file.EnemyList.Count; i++)
            {
                enemyData = DataContext.Instance.EnemyDic[file.EnemyList[i][3]];
                battleCharacterInfo = new BattleCharacterInfo(battleInfo.Lv, enemyData);
                Type t = Type.GetType("Battle." + enemyData.AI);
                battleCharacterInfo.AI = (BattleAI)Activator.CreateInstance(t);
                battleCharacterInfo.AI.Init(battleCharacterInfo);
                enemyPosition = new Vector3(file.EnemyList[i][0], file.EnemyList[i][1], file.EnemyList[i][2]);
                battleCharacterInfo.Position = enemyPosition;
                battleInfo.EnemyList.Add(battleCharacterInfo);
            }

            return battleInfo;
        }
    }
}
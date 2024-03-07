using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreFile
    {
        public int Floor;
        public Vector2Int Size;
        public Vector2Int Start;
        public Vector2Int Goal;
        public Vector2Int PlayerPosition;
        public int PlayerRotation;
        public List<Vector2Int> GroundList = new List<Vector2Int>(); //房間或走廊等可行走的地形
        public List<Vector2Int> VisitedList = new List<Vector2Int>();
        public List<ExploreEnemyInfo> EnemyInfoList = new List<ExploreEnemyInfo>();
        //json 不能以 Vector2Int 作為 dictionary 的 key
        //所以要把它的 keys 和 values 分開來儲存
        public List<Vector2Int> TileKeys = new List<Vector2Int>();
        public List<string> TileValues = new List<string>();
        public List<Vector2Int> TreasureKeys = new List<Vector2Int>();
        public List<Treasure> TreasureValues = new List<Treasure>();
        public List<Vector2Int> TriggerKeys = new List<Vector2Int>();
        public List<string> TriggerValues = new List<string>();

        public ExploreFile() { }

        public ExploreFile(ExploreInfo info)
        {
            Floor = info.Floor;
            Size = info.Size;
            Start = info.Start;
            Goal = info.Goal;
            PlayerPosition = info.PlayerPosition;
            PlayerRotation = info.PlayerRotation;
            GroundList = info.GroundList;
            VisitedList = info.VisitedList;
            EnemyInfoList = info.EnemyInfoList;
            TileKeys = new List<Vector2Int>(info.TileDic.Keys);
            List<TileObject> tileObjList = new List<TileObject>(info.TileDic.Values); 
            for(int i=0; i<tileObjList.Count; i++)
            {
                TileValues.Add(tileObjList[i].Name);
            }
            TreasureKeys = new List<Vector2Int>(info.TreasureDic.Keys);
            TreasureValues = new List<Treasure>(info.TreasureDic.Values);
            TriggerKeys = new List<Vector2Int>(info.TriggerDic.Keys);
            TriggerValues = new List<string>(info.TriggerDic.Values);
        }
    }
}
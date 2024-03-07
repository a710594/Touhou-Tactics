using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreInfo
    {
        public int Floor;
        public Vector2Int Size;
        public Vector2Int Start;
        public Vector2Int Goal;
        public Vector2Int PlayerPosition;
        public int PlayerRotation;
        public List<Vector2Int> GroundList = new List<Vector2Int>(); //房間或走廊等可行走的地形
        public List<Vector2Int> VisitedList = new List<Vector2Int>(); //探索過的地區
        public List<Vector2Int> WalkableList = new List<Vector2Int>(); //當前可行走的地區(排除寶箱和敵人等擋路的東西)
        public List<ExploreEnemyInfo> EnemyInfoList = new List<ExploreEnemyInfo>();
        public Dictionary<Vector2Int, TileObject> TileDic = new Dictionary<Vector2Int, TileObject>();
        public Dictionary<Vector2Int, Treasure> TreasureDic = new Dictionary<Vector2Int, Treasure>();
        public Dictionary<Vector2Int, string> TriggerDic = new Dictionary<Vector2Int, string>();

        public ExploreInfo() { }

        public ExploreInfo(ExploreFile file) 
        {
            Floor = file.Floor;
            Size = file.Size;
            Start = file.Start;
            Goal = file.Goal;
            PlayerPosition = file.PlayerPosition;
            PlayerRotation = file.PlayerRotation;
            GroundList = file.GroundList;
            VisitedList = file.VisitedList;
            EnemyInfoList = file.EnemyInfoList;

            for (int i=0; i<file.TileKeys.Count; i++) 
            {
                TileDic.Add(file.TileKeys[i], new TileObject(file.TileValues[i]));
            }

            for (int i=0; i<file.TreasureKeys.Count; i++) 
            {
                TreasureDic.Add(file.TreasureKeys[i], file.TreasureValues[i]);
            }

            for (int i = 0; i < file.TriggerKeys.Count; i++)
            {
                TriggerDic.Add(file.TriggerKeys[i], file.TriggerValues[i]);
            }
        }

        public int Width() 
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            foreach(KeyValuePair<Vector2Int, TileObject> pair in TileDic) 
            {
                if(pair.Key.x < minX)
                {
                    minX = pair.Key.x;
                }

                if (pair.Key.x > maxX)
                {
                    maxX = pair.Key.x;
                }
            }

            return maxX - minX;
        }

        public int Height()
        {
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            foreach (KeyValuePair<Vector2Int, TileObject> pair in TileDic)
            {
                if (pair.Key.y < minY)
                {
                    minY = pair.Key.y;
                }

                if (pair.Key.y > maxY)
                {
                    maxY = pair.Key.y;
                }
            }

            return maxY - minY;
        }

        public void SetWalkableList() 
        {
            WalkableList = new List<Vector2Int>(GroundList);
            foreach (KeyValuePair<Vector2Int, Treasure> pair in TreasureDic)
            {
                WalkableList.Remove(pair.Key);
            }
            for (int i = 0; i < EnemyInfoList.Count; i++)
            {
                WalkableList.Remove(EnemyInfoList[i].Position);
            }
        }
    }
}

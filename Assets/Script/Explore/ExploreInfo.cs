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
        public List<Vector2Int> WalkableList = new List<Vector2Int>(); //房間或走廊等可行走的地形
        public List<Vector2Int> VisitedList = new List<Vector2Int>();
        public List<ExploreEnemyInfo> EnemyInfoList = new List<ExploreEnemyInfo>();
        public Dictionary<Vector2Int, CellInfo> CellDic = new Dictionary<Vector2Int, CellInfo>();
        public Dictionary<Vector2Int, Treasure> TreasureDic = new Dictionary<Vector2Int, Treasure>();

        public ExploreInfo() { }

        public ExploreInfo(ExploreFile file) 
        {
            Floor = file.Floor;
            Size = file.Size;
            Start = file.Start;
            Goal = file.Goal;
            PlayerPosition = file.PlayerPosition;
            PlayerRotation = file.PlayerRotation;
            WalkableList = file.WalkableList;
            VisitedList = file.VisitedList;
            EnemyInfoList = file.EnemyInfoList;

            for (int i=0; i<file.CellKeys.Count; i++) 
            {
                CellDic.Add(file.CellKeys[i], file.CellValues[i]);
            }

            for (int i=0; i<file.TreasureKeys.Count; i++) 
            {
                TreasureDic.Add(file.TreasureKeys[i], file.TreasureValues[i]);
            }
        }
    }
}

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
        public List<Vector2Int> WalkableList = new List<Vector2Int>(); //�ж��Ψ��Y���i�樫���a��
        public List<Vector2Int> VisitedList = new List<Vector2Int>();
        public List<ExploreEnemyInfo> EnemyInfoList = new List<ExploreEnemyInfo>();
        //json ����H Vector2Int �@�� dictionary �� key
        //�ҥH�n�⥦�� keys �M values ���}���x�s
        public List<Vector2Int> CellKeys = new List<Vector2Int>();
        public List<CellInfo> CellValues = new List<CellInfo>();
        public List<Vector2Int> TreasureKeys = new List<Vector2Int>();
        public List<Treasure> TreasureValues = new List<Treasure>();

        public ExploreFile() { }

        public ExploreFile(ExploreInfo info)
        {
            Floor = info.Floor;
            Size = info.Size;
            Start = info.Start;
            Goal = info.Goal;
            PlayerPosition = info.PlayerPosition;
            PlayerRotation = info.PlayerRotation;
            WalkableList = info.WalkableList;
            VisitedList = info.VisitedList;
            EnemyInfoList = info.EnemyInfoList;
            CellKeys = new List<Vector2Int>(info.CellDic.Keys);
            CellValues = new List<CellInfo>(info.CellDic.Values);
            TreasureKeys = new List<Vector2Int>(info.TreasureDic.Keys);
            TreasureValues = new List<Treasure>(info.TreasureDic.Values);
        }
    }
}
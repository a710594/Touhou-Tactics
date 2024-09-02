using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreInfo
    {
        public bool IsArrive = false;
        public int Floor;
        public Vector2Int Size;
        public Vector2Int Start;
        public Vector2Int Goal;
        public ExploreCharacterController Player;
        public List<ExploreInfoEnemy> EnemyList = new List<ExploreInfoEnemy>();
        public Dictionary<Vector2Int, ExploreInfoTile> TileDic = new Dictionary<Vector2Int, ExploreInfoTile>();
    
        public ExploreInfo(){}

        public ExploreInfo(ExploreFile file)
        {
            IsArrive = file.IsArrive;
            Floor = file.Floor;
            Size = file.Size;
            Start = file.Start;
            Goal = file.Goal;   
        }
    }
}

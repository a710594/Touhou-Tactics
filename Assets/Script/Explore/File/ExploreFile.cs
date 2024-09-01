using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreFile
    {
        public bool IsArrive = false; //��F���I
        public int Floor;
        public Vector2Int Size;
        public Vector2Int Start;
        public Vector2Int Goal;
        public Vector2Int PlayerPosition;
        public int PlayerRotation;
        public List<ExploreFileEnemy> EnemyList = new List<ExploreFileEnemy>();
        public List<ExploreFileTile> TileList = new List<ExploreFileTile>();
        public List<ExploreFileTreasure> TreasureList = new List<ExploreFileTreasure>();
        public List<ExploreFileTrigger> TriggerList = new List<ExploreFileTrigger>();
    }
}
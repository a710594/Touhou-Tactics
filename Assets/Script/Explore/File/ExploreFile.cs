using System;
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
        public float PlayerPositionX;
        public float PlayerPositionZ;
        public float PlayerRotationY;
        public List<ExploreFileEnemy> EnemyList = new List<ExploreFileEnemy>();
        public List<ExploreFileTile> TileList = new List<ExploreFileTile>();
        public List<ExploreFileTreasure> TreasureList = new List<ExploreFileTreasure>();
        public List<ExploreFileEvent> EventList = new List<ExploreFileEvent>();
        public List<ExploreFileDoor> DoorList = new List<ExploreFileDoor>();

        [NonSerialized]
        public Goal GoalObj;
    }
}
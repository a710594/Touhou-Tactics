using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreFile
{
    public bool IsArrive = false; //��F���I
    public int Floor;
    public Vector2Int Size;
    public Vector2Int Start;
    public Vector2Int Goal;
    public Vector2Int PlayerPosition;
    public int PlayerRotation;
    public List<Vector2Int> VisitedList = new List<Vector2Int>();
    public List<Vector2Int> WalkableList = new List<Vector2Int>();
    public List<ExploreFileEnemy> EnemyInfoList = new List<ExploreFileEnemy>();
    public List<TileInfo> TileList = new List<TileInfo>();
    public List<Treasure> TreasureList = new List<Treasure>();
    public List<TriggerInfo> TriggerList = new List<TriggerInfo>();

    public class TileInfo
    {
        public Vector2Int Position;
        public string Prefab;
        public string Tag;

        public TileInfo(Vector2Int position, string prefab, string tag)
        {
            Position = position;
            Prefab = prefab;
            Tag = tag;
        }
    }

    public class TriggerInfo
    {
        public Vector2Int Position;
        public string Name;

        public TriggerInfo(Vector2Int position, string name)
        {
            Position = position;
            Name = name;
        }
    }
}

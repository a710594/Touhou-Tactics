using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewExploreFile
{
    public int Floor;
    public Vector2Int Size;
    public Vector2Int Start;
    public Vector2Int Goal;
    public Vector2Int PlayerPosition;
    public int PlayerRotation;
    public List<Vector2Int> VisitedList = new List<Vector2Int>();
    public List<Vector2Int> WalkableList = new List<Vector2Int>();
    public List<EnemyInfo> EnemyInfoList = new List<EnemyInfo>();
    public List<TileInfo> TileList = new List<TileInfo>();
    public List<Treasure> TreasureList = new List<Treasure>();
    public List<TriggerInfo> TriggerList = new List<TriggerInfo>();

    public class EnemyInfo
    {
        public string Prefab;
        public string Map;
        public string Tutorial;
        public Vector2Int Position;
        public int Rotation;

        public EnemyInfo() { }

        public EnemyInfo(string name, string map, string tutorial, Vector2Int position, int rotation)
        {
            Prefab = name;
            Map = map;
            Tutorial = tutorial;
            Position = position;
            Rotation = rotation;
        }
    }

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

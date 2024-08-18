using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreFile
{
    public bool IsArrive = false; //抵達終點
    public int Floor;
    public Vector2Int Size;
    public Vector2Int Start;
    public Vector2Int Goal;
    public Vector2Int PlayerPosition;
    public int PlayerRotation;
    public List<Vector2Int> VisitedList = new List<Vector2Int>();
    public List<Vector2Int> WalkableList = new List<Vector2Int>();
    public List<EnemyExplorer> EnemyInfoList = new List<EnemyExplorer>();
    public List<TileInfo> TileList = new List<TileInfo>();
    public List<Treasure> TreasureList = new List<Treasure>();
    public List<TriggerInfo> TriggerList = new List<TriggerInfo>();

    /*public class EnemyInfo
    {
        public string Explorer; //探索地圖上的模型
        public string Map = null;
        public string MapSeed = null;
        public string Tutorial;
        public Vector2Int Position;
        public int Rotation;

        public EnemyInfo() { }

        public EnemyInfo(string explorer, string map, string tutorial, Vector2Int position, int rotation)
        {
            Explorer = explorer;
            Map = map;
            Tutorial = tutorial;
            Position = position;
            Rotation = rotation;
        }

        public EnemyInfo(string explorer, bool isSeed, string map, string tutorial, Vector2Int position, int rotation)
        {
            Explorer = explorer;
            IsSeed = isSeed;
            Map = map;
            Tutorial = tutorial;
            Position = position;
            Rotation = rotation;
        }
    }*/

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

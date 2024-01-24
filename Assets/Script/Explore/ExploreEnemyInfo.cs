using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreEnemyInfo
{
    public string Prefab;
    public string Map;
    public Vector2Int Position;
    public int Rotation;

    public ExploreEnemyInfo() { }

    public ExploreEnemyInfo(string name, string map, Vector2Int position, int rotation)
    {
        Prefab = name;
        Map = map;
        Position = position;
        Rotation = rotation;
    }
}

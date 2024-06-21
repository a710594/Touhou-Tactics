using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreEnemyInfo
{
    public string Prefab;
    public string Map;
    public string Tutorial;
    public Vector2Int Position;
    public int Rotation;

    public ExploreEnemyInfo() { }

    public ExploreEnemyInfo(string name, string map, string tutorial, Vector2Int position, int rotation)
    {
        Prefab = name;
        Map = map;
        Tutorial = tutorial;
        Position = position;
        Rotation = rotation;
    }
}

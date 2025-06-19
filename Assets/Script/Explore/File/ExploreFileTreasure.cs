using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreFileTreasure
{
    public bool IsVisited;
    public int ItemID;
    public string Prefab;
    public Vector2Int Position;

    public ExploreFileTreasure() { }
    public ExploreFileTreasure(int itemId, string prefab, Vector2Int position) 
    {
        IsVisited = false;
        ItemID = itemId;
        Prefab = prefab;
        Position = position;
     }
}

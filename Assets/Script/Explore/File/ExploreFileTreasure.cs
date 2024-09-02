using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreFileTreasure
{
    public int ItemID;
    public string Prefab;
    public float Height; //Vector3 不能序列化,高度需要額外存
    public Vector2Int Position;
    public Vector3Int Rotation;

    public ExploreFileTreasure() { }
    public ExploreFileTreasure(int itemId, string prefab, float height, Vector2Int position, Vector3Int rotation) 
    {
        ItemID = itemId;
        Prefab = prefab;
        Height = height;
        Position = position;
        Rotation = rotation;
     }
}

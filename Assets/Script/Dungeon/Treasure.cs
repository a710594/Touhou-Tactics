using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure
{
    public Vector2Int Position;
    public TreasureModel.TypeEnum Type;
    public int ItemID;
    public string Prefab;
    public float Height;
    public Vector3Int Rotation;

    public Treasure() { }

    public Treasure(Vector2Int position, TreasureModel data) 
    {
        int random = Random.Range(0, data.IDList.Count);
        Position = position;
        Type = data.Type;
        ItemID = data.IDList[random];
        Prefab = data.Prefab;
        Height = data.Height;
        Rotation = Utility.GetVector3Int(data.Rotation);
    }
}

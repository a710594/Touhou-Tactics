using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreFileTreasure
{
    public TreasureModel.TypeEnum Type;
    public int ItemID;
    public string Prefab;
    public float Height; //Vector3 ����ǦC��,���׻ݭn�B�~�s
    public Vector2Int Position;
    public Vector3Int Rotation;

    public ExploreFileTreasure() { }
}

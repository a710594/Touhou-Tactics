using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreInfoTreasure
{
    public int ItemID;
    public float Height;
    public string Prefab;
    public TreasureObject Object;

    public ExploreInfoTreasure(){}

    public ExploreInfoTreasure(ExploreFileTreasure file)
    {
        ItemID = file.ItemID;
        Prefab = file.Prefab;
        Height = file.Height;
    }
}

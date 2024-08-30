using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreInfoTreasure
{
    public TreasureModel.TypeEnum Type;
    public int ItemID;
    public string Prefab;
    public TreasureObject Object;

    public ExploreInfoTreasure(){}

    public ExploreInfoTreasure(ExploreFileTreasure file)
    {
        Type = file.Type;
        ItemID = file.ItemID;
        Prefab = file.Prefab;
    }
}

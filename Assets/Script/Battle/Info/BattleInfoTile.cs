using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInfoTile
{
    public TileModel TileData;
    public TileObject TileObject;

    public AttachModel AttachData;
    public GameObject AttachObject;

    public int MoveCost
    {
        get
        {
            if (AttachData == null)
            {
                return TileData.MoveCost;
            }
            else if (AttachData.MoveCost >= 0) 
            {
            return AttachData.MoveCost;
            }
            else
            {
                return -1;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInfoTile
{
    public TileModel TileData;
    public TileObject TileObject;

    public string AttachName;
    public GameObject AttachObject;

    public BattleInfoTile(int id) 
    {
         TileData = DataContext.Instance.TileDic[id];
    }
}

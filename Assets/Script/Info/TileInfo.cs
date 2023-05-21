using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo
{
    public bool HasCharacter;
    public string TileID;
    public int Height;
    public string AttachID; //Tile 上面的東西,例如草,樹,石頭...

    public int MoveCost 
    {
        get 
        {
            if (_attachMoveCost >= 0)
            {
                return _tileMoveCost + _attachMoveCost;
            }
            else
            {
                return -1;
            }
        }
    }

    private int _tileMoveCost;
    private int _attachMoveCost = 0;

    public TileInfo(TileScriptableObject tile) 
    {
        TileID = tile.ID;
        Height = tile.Height;
        _tileMoveCost = tile.MoveCost;
    }

    public void SetAttach(string id, int moveCost) 
    {
        AttachID = id;
        _attachMoveCost = moveCost;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo
{
    public bool HasCharacter;
    public string TileID;
    public int Height;
    public string AttachID; //Tile �W�����F��,�Ҧp��,��,���Y...

    public int MoveCost
    {
        get
        {
            if (_attachMoveCost >= 0 /*&& !HasCharacter*/)
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

    public TileInfo(TileSetting tile)
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAttachInfo
{
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

    public TileAttachInfo(string id)
    {
        try
        {
            TileSetting tileSetting = DataContext.Instance.TileSettingDic[id];
            TileID = id;
            Height = tileSetting.Height;
            _tileMoveCost = tileSetting.MoveCost;
        }
        catch(Exception ex) 
        {
            Debug.Log(ex);
        }
    }

    public TileAttachInfo(TileSetting tile)
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

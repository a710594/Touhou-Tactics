using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInfoTile
{
    public int Height;
    public int MoveCost;
    public MeshRenderer Quad;
    public SpriteRenderer Select;
    public GameObject Buff;

    public BattleInfoTile(BattleFileTile file) 
    {
        Height = file.Height;
        MoveCost = file.MoveCost;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFileRandom
{
    public int MinX;
    public int MaxX;
    public int MinY;
    public int MaxY;
    public List<Vector2Int> PlayerPositionList = new List<Vector2Int>();
    public List<BattleFileTile> TileList = new List<BattleFileTile>();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapFile
{
    public int MinX;
    public int MaxX;
    public int MinY;
    public int MaxY;
    public int MaxPlayerCount;
    public Dictionary<Vector2Int, string> TileList = new Dictionary<Vector2Int, string>();
    public List<Vector2Int> PlayerPositionList = new List<Vector2Int>();
}

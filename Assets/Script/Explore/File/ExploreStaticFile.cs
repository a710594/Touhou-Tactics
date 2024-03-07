using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreStaticFile
{
    public int Floor;
    public Vector2Int Size;
    public Vector2Int Start;
    public Vector2Int Goal;
    public List<Vector2Int> GroundList = new List<Vector2Int>(); //房間或走廊等可行走的地形
    //json 不能以 Vector2Int 作為 dictionary 的 key
    //所以要把它的 keys 和 values 分開來儲存
    public List<Vector2Int> TileKeys = new List<Vector2Int>();
    public List<string> TileValues = new List<string>();
}

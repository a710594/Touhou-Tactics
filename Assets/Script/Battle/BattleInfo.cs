using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BattleInfo
{
    public int Width;
    public int Height;
    public Dictionary<Vector2Int, TileInfo> TileInfoDic;
    public Dictionary<Vector2Int, TileComponent> TileComponentDic;
    public Dictionary<Vector2Int, GameObject> AttachDic;
}

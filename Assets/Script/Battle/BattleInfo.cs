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
    public Dictionary<Vector2, TileInfo> TileInfoDic;
    public Dictionary<Vector2, TileComponent> TileComponentDic;
    public Dictionary<Vector2, GameObject> AttachDic;
}

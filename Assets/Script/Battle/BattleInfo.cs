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
    public Dictionary<Vector2, TileInfo> tileInfoDic;
    public Dictionary<Vector2, TileComponent> tileComponentDic;
    public Dictionary<Vector2, GameObject> attachDic;
}

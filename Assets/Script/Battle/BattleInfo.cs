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
    public List<Vector2Int> NoAttachList = new List<Vector2Int>();
    public Dictionary<Vector2Int, TileInfo> TileInfoDic = new Dictionary<Vector2Int, TileInfo>();
    public Dictionary<Vector2Int, TileComponent> TileComponentDic = new Dictionary<Vector2Int, TileComponent>();
    public Dictionary<Vector2Int, GameObject> AttachDic = new Dictionary<Vector2Int, GameObject>();
}

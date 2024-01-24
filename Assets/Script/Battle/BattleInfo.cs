using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BattleInfo
{
    public bool IsTutorial;
    public int Width;
    public int Height;
    public int PlayerCount;
    public int Exp;
    public List<Vector2Int> NoAttachList = new List<Vector2Int>();
    public Dictionary<Vector2Int, TileAttachInfo> TileAttachInfoDic = new Dictionary<Vector2Int, TileAttachInfo>();
    public Dictionary<Vector2Int, TileComponent> TileComponentDic = new Dictionary<Vector2Int, TileComponent>();
    public Dictionary<Vector2Int, GameObject> AttachDic = new Dictionary<Vector2Int, GameObject>();
    public Dictionary<Vector3Int, int> EnemyDic = new Dictionary<Vector3Int, int>();
}

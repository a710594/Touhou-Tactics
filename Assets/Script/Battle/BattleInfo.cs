using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Battle
{
    public class BattleInfo
    {
        public int MinX;
        public int MaxX;
        public int MinY;
        public int MaxY;
        public int NeedCount;
        public bool MustBeEqualToNeedCount = false;
        public int Lv;
        public int Exp;
        public List<Vector2Int> NoAttachList = new List<Vector2Int>();
        public List<BattleCharacterInfo> EnemyList = new List<BattleCharacterInfo>();
        public Dictionary<Vector2Int, TileAttachInfo> TileAttachInfoDic = new Dictionary<Vector2Int, TileAttachInfo>();
        public Dictionary<Vector2Int, TileComponent> TileComponentDic = new Dictionary<Vector2Int, TileComponent>();
        public Dictionary<Vector2Int, GameObject> AttachDic = new Dictionary<Vector2Int, GameObject>();
    }
}
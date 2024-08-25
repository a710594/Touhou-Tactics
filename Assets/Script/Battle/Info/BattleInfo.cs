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
        public List<Vector2Int> PlayerPositionList = new List<Vector2Int>();
        public List<BattleCharacterInfo> EnemyList = new List<BattleCharacterInfo>();
        public Dictionary<Vector2Int, BattleInfoTile> TileDic = new Dictionary<Vector2Int, BattleInfoTile>();
        //public Dictionary<Vector2Int, TileComponent> TileComponentDic = new Dictionary<Vector2Int, TileComponent>();
        //public Dictionary<Vector2Int, GameObject> AttachDic = new Dictionary<Vector2Int, GameObject>();

        public BattleInfo() { }

        public BattleInfo(BattleFileFixed file) 
        {
            MinX = file.MinX;
            MaxX = file.MaxX;
            MinY = file.MinY;
            MaxY = file.MaxY;
            NeedCount = file.NeedCount;
            MustBeEqualToNeedCount = file.MustBeEqualToNeedCount;
            PlayerPositionList = file.PlayerPositionList;

            for (int i=0; i<file.EnemyList.Count; i++) 
            {
                EnemyList.Add(new BattleCharacterInfo(file.EnemyList[i]));
            }

            BattleInfoTile tile;
            for (int i=0; i<file.TileList.Count; i++) 
            {
                tile = new BattleInfoTile();
                tile.TileData = DataContext.Instance.TileDic[file.TileList[i].ID];
                TileDic.Add(file.TileList[i].Position, tile);
            }
        }
    }
}
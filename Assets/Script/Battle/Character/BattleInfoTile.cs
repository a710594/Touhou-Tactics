using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Battle
{
    public class BattleInfoTile
    {
        public TileModel TileData;
        public BattleTileObject TileObject;

        public AttachModel AttachData;
        public GameObject AttachObject;

        public int MoveCost
        {
            get
            {
                if (AttachData == null)
                {
                    return TileData.MoveCost;
                }
                else if (AttachData.MoveCost != -1) 
                {
                    return TileData.MoveCost + AttachData.MoveCost;
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}
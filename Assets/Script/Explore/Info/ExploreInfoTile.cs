using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreInfoTile
    {
        public enum CellType {
            None,
            Room,
            Hallway
        }

        public bool IsVisited;
        public bool IsWalkable;
        public string Event;
        public CellType Type;
        public TileObject Object;
        public ExploreInfoTreasure Treasure;
    }
}

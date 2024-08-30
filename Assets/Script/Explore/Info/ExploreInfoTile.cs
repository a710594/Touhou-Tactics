using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreInfoTile
    {
        public bool IsVisited = false;
        public bool IsWalkable = true;
        public string Event = null;
        public TileObject Object = null;
        public ExploreInfoTreasure Treasure = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreInfoTile
    {
        public bool IsWalkable;
        public bool IsVisited;
        public string Event = null;
        public string Prefab;
        public TileObject Object;
        public ExploreInfoTreasure Treasure;

        public ExploreInfoTile() { }

        public ExploreInfoTile(bool isWalkable, bool isVisited, string prefab) 
        {
            IsWalkable = isWalkable;
            IsVisited = isVisited;
            Prefab = prefab;
        }

        public ExploreInfoTile(ExploreFileTile file) 
        {
            IsWalkable = file.IsWalkable;
            IsVisited = file.IsVisited;
            Prefab = file.Prefab;
        }
    }
}

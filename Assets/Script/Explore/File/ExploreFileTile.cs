using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreFileTile
    {
        public bool IsWalkable;
        public bool IsVisited;
        public string Prefab;
        public Vector2Int Position;

        public ExploreFileTile() { }

        public ExploreFileTile(bool isWalkable, bool isVisited, string prefab, Vector2Int position)
        {
            IsWalkable = isWalkable; 
            IsVisited = isVisited;
            Prefab = prefab;
            Position = position;
        }

        public ExploreFileTile(ExploreInfoTile info)
        {
            IsWalkable = info.IsWalkable;
            IsVisited = info.IsVisited;
            Prefab = info.Prefab;
            Position = Utility.ConvertToVector2Int(info.Object.transform.position);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreFileTile
    {
        public bool IsVisited;
        public bool IsWalkable;
        public string Prefab;
        public Vector2Int Position;

        public ExploreFileTile() { }

        public ExploreFileTile(Vector2Int position, string prefab, string tag)
        {
            IsWalkable = tag != "Wall";
            IsVisited = false;
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreFileTile
    {
        public bool IsVisited;
        public string Prefab;
        public Vector2Int Position;

        public ExploreFileTile() { }

        public ExploreFileTile(string prefab, Vector2Int position)
        {
            IsVisited = false;
            Prefab = prefab;
            Position = position;
        }
    }
}
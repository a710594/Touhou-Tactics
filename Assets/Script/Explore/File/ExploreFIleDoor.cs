using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Explore
{
    public class ExploreFileDoor
    {
        public bool IsVisited;
        public Vector2Int Position;

        public ExploreFileDoor() { }

        public ExploreFileDoor(Vector2Int position) 
        {
            IsVisited = false;
            Position = position;
        }
    }
}
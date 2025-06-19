using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreFileEvent
{
        public Vector2Int Position;
        public string Name;

        public ExploreFileEvent(Vector2Int position, string name)
        {
            Position = position;
            Name = name;
        }
}

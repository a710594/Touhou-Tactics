using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreFileTrigger
{
        public Vector2Int Position;
        public string Name;

        public ExploreFileTrigger(Vector2Int position, string name)
        {
            Position = position;
            Name = name;
        }
}

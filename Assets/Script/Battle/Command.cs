using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Command
    {
        [NonSerialized]
        public Effect Effect;
        public int ID;
        public int Hit;
        public int Range;
        public string Name;
        public string Comment;
        public TargetEnum RangeTarget;
        public TargetEnum AreaTarget;
        public AreaTypeEnum AreaType;
        public TrackEnum Track;
        public Command SubCommand;
        public List<Vector2Int> ArrayList = new List<Vector2Int>();

    }
}

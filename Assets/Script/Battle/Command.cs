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
        public int Hit;
        public int Range;
        public TargetEnum CastTarget;
        public TargetEnum EffectTarget;
        public TrackEnum Track;
        public Command SubCommand;
        public List<Vector2Int> AreaList = new List<Vector2Int>();

    }
}

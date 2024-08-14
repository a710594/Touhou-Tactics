using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    //for main skill
    public enum TargetEnum 
    {
        All=0,
        Self,
        Us,
        Them, 
        None,
        UsMinHP,
    }

public enum AreaTypeEnum
{
    Point=1,
    Array,
    Global,
}

    public enum TrackEnum 
    {
        None = 1,
        Straight,
        Parabola,
        Through,
    }

            public enum HitType
        {
            Miss,
            Hit,
            Critical,
            NoDamage,
        }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public enum TargetEnum
    {
        Us = 1,
        Them,
        All,
        None, //只能在空地上使用
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
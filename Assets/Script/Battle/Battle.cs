using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    //for main skill
    public enum TargetEnum 
    {
        All=0,
        Us,
        Them, 
        None,
    }

    public enum SubTargetEnum 
    {
        Self=1,
        MinHp=2,
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

    public enum ResultType 
    {   
        None,
        Win,
        Lose,
    }
}
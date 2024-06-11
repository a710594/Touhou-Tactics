using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellModel
{
    public int ID;
    public int EffectID;
    public int Job;
    public int CD;
    public string Name;
    public string Comment;
    public int Hit;
    public int Range;
    public string Area;
    public Battle.TargetEnum CastTarget;
    public Battle.TargetEnum EffectTarget;
    public Battle.TrackEnum Track;
    public int SubSpellID;
}

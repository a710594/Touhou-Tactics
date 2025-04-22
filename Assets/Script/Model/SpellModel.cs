using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;

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
    public string AreaArray;
    public TargetEnum Target;
    public AreaTypeEnum AreaType;
    public TrackEnum Track;
    public int SubSpellID;
    public string Particle;
    public bool Shake;
}

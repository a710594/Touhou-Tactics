using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SkillModel
{
    public int ID;
    public string Name;
    public string Comment;
    public int CD;
    public int EffectID;
    public int Hit;
    public int Range;
    public string Area;
    public Battle.TargetEnum CastTarget;
    public Battle.TargetEnum EffectTarget;
    public Battle.TrackEnum Track;
}
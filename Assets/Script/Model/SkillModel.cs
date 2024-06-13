using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Battle;

public class SkillModel
{
    public int ID;
    public string Name;
    public string Comment;
    public int CD;
    public int EffectID;
    public int Hit;
    public int Range;
    public string Array;
    public TargetEnum RangeTarget;
    public TargetEnum AreaTarget;
    public AreaTypeEnum AreaType;
    public Battle.TrackEnum Track;
    public int SubSkillID;
}
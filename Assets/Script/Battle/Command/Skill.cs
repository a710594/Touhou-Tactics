using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Battle;

public class Skill : Command
{
    public int CD;
    public int CurrentCD;
    public Skill() { }

    public Skill(SkillModel data)
    {
        ID = data.ID;
        Name = data.Name;
        Comment = data.Comment;
        Hit = data.Hit;
        Range = data.Range;
        Target = data.Target;
        AreaType = data.AreaType;
        Track = data.Track;
        ArrayList = Utility.GetAreaList(data.AreaArray);
        Particle = data.Particle;
        Shake = data.Shake;

        CD = data.CD;
        CurrentCD = 0;
        Effect = EffectFactory.GetEffect(data.EffectID);
    }

    public void CheckCD() 
    {
        if (CurrentCD > 0) 
        {
            CurrentCD--;
        }
    }
}

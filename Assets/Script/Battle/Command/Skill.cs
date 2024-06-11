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
        CastTarget = data.CastTarget;
        EffectTarget = data.EffectTarget;
        Track = data.Track;
        AreaList = Utility.GetAreaList(data.Area);

        CD = data.CD;
        CurrentCD = 0;
        Effect = EffectFactory.GetEffect(data.EffectID);
        if(data.SubSkillID != -1)
        {
            SubCommand = new Skill(DataContext.Instance.SkillDic[data.SubSkillID]);
        }
    }

    public void CheckCD() 
    {
        if (CurrentCD > 0) 
        {
            CurrentCD--;
        }
    }
}

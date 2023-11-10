using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Skill
{
    public int CurrentCD;
    public SkillModel Data;
    public Effect Effect;

    public Skill() { }

    public Skill(SkillModel data)
    {
        Data = data;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Skill
{
    public SkillModel Data;
    public Effect Effect;

    public Skill() { }

    public Skill(SkillModel data)
    {
        Data = data;
        Effect = EffectFactory.GetEffect(data.EffectType, data.EffectID);
    }
}

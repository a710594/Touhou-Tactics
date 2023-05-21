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

    private BattleCharacterInfo _user;

    public Skill() { }

    public Skill(SkillModel data, BattleCharacterInfo user)
    {
        Data = data;
        Effect = EffectFactory.GetEffect(data.EffectType, data.EffectID, user);
        _user = user;
    }

    public bool CanUse()
    {
        return _user.CurrentMP >= Data.MP;
    }

    public void Use(List<BattleCharacterInfo> targetList) 
    {
        _user.CurrentMP -= Data.MP;
        for (int i=0; i<targetList.Count; i++) 
        {
            Effect.SetEffect(targetList[i]);
        }
    }
}

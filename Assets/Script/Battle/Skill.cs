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
        Effect = EffectFactory.GetEffect(data.EffectType, data.EffectID);
    }

    public virtual void SetEffect(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList)
    {
        Effect.SetEffect(user, target, floatingList, characterList);
        if (Data.CD > 0)
        {
            CurrentCD = Data.CD + 1; //要加一的原因是為了抵銷本回合的 CheckCD
        }
        user.CurrentSP -= Data.SP;
    }

    public void CheckCD() 
    {
        if (CurrentCD > 0) 
        {
            CurrentCD--;
        }
    }
}

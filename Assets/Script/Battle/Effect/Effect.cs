using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Effect
{
    public EffectModel Data;

    public Effect SubEffect;

    public Effect() { }

    public Effect(EffectModel data)
    {
        Data = data;
        if (data.SubType != EffectModel.TypeEnum.None)
        {
            SubEffect = EffectFactory.GetEffect(data.SubType, data.SubID);
        }
    }

    public virtual void  SetEffect(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList) 
    {
    }
}
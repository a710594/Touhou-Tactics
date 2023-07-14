using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support
{
    public SupportModel Data;
    public Effect Effect;

    public Support(SupportModel data)
    {
        Data = data;
        Effect = EffectFactory.GetEffect(data.EffectType, data.EffectID);
    }

    public virtual void SetEffect(BattleCharacterInfo user, BattleCharacterInfo target, List<FloatingNumberData> floatingList, List<BattleCharacterInfo> characterList)
    {
        Effect.SetEffect(user, target, floatingList, characterList);
        user.CurrentSP -= Data.SP;
    }
}
